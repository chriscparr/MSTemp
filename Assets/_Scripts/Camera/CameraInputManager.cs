using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraInputManager : MonoBehaviour {

	public enum Phase
	{
		SetupPhase,
		MainCellPhase,
		FocusedSubCellPhase,
		InsideSubCellPhase
	}

	public static CameraInputManager Instance { get { return s_instance; } }

	private float m_camVectorMagLowerBound = 5f;

	private static CameraInputManager s_instance = null;

	public Phase m_CurrentPhase; // so we can change phases from other scripts. could make it static?

	[SerializeField]
	private float m_RotationSpeed;
	[SerializeField]
	private float m_ZoomSpeed; // we are just going to move the camera
	[SerializeField]
	private GameObject m_MainCamera;
	[SerializeField]
	private float m_DistanceFromSubCell = 25;
	[SerializeField]
	private float m_touchThresholdX = 20f;
	[SerializeField]
	private float m_touchThresholdY = 20f;

	private Transform m_CurrentTarget;
	private Vector3 m_CachedPosition;
	private Subcell m_selectedCell;
	private RailMover m_Mover;


	private void Awake() 
	{
		s_instance = this;
		m_MainCamera = Camera.main.gameObject;
		m_CachedPosition = m_MainCamera.transform.position;
		SetPhase(Phase.SetupPhase);

		if (m_MainCamera.GetComponent<RailMover>() == null)
		{
			m_Mover = m_MainCamera.AddComponent<RailMover>();
		}
		else
			m_Mover = m_MainCamera.GetComponent<RailMover>();
	}

	public void SetLookAtTarget(Transform target)
	{
		m_CurrentTarget = target;
	}

	public void SetPhase(Phase newPhase)
	{
		m_CurrentPhase = newPhase;

		switch (newPhase) {
			case Phase.SetupPhase:
				m_selectedCell = null;
				// ban all movement here
				break;
			case Phase.MainCellPhase:
				if (GameObject.Find("SynapseGenerator"))
				{
					FindObjectOfType<SynapseGenerator>().enabled = true;
				}
				if (m_selectedCell != null)
				{
					FocusReset ();
				}
				// allow rotation and pinch and zoom to move in? google earth controls
				// dont forget to use the actual f'n event handlers
				break;
			case Phase.FocusedSubCellPhase:

				// here we pinch and zoom to scale the sub cell, so you may not need to do anything
				break;
			case Phase.InsideSubCellPhase:
				// here we want to do case study stuff, so enable BOTH OnRailsMovement and RailMover, put buttons to move to/from paths (do that listener and event stuff)
				// ps learn about listeners and events already. 
				break;

		}
	}

	public void ResetPosition(bool doSmoothly = false)
	{
        // TODO make me a tween, so we gradually go back to the main phase, dont just snap it back.
        if (!doSmoothly)
        {
            Camera.main.transform.position = m_CachedPosition;
            Camera.main.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            m_MainCamera.GetComponent<RailMover>().TweenToPosition(m_CachedPosition, 5, false, iTween.EaseType.easeOutSine);
            Camera.main.transform.eulerAngles = Vector3.zero;
        }
		if (m_selectedCell != null)
		{
			FocusReset ();
		}
	}

	public void FocusReset()
	{
		m_selectedCell.RigidBody.isKinematic = false;
		Destroy (m_selectedCell.GetComponent<RailMover> ());
		m_selectedCell = null;
		ModelManager.Instance.ShakeModel();
		SetPhase (Phase.MainCellPhase);
	}

	public void FocusOnSubCell(Subcell selectedCell)
	{
		if (m_selectedCell != null)
		{
			FocusReset ();
		}
		m_selectedCell = selectedCell;
		m_CurrentTarget = selectedCell.transform;

		Vector3 desiredPosition = Vector3.zero;	//m_mainContainer is always at zero, lets keep it private if we can...


		m_selectedCell.RigidBody.isKinematic = true;
		m_selectedCell.gameObject.AddComponent<RailMover>();
		m_Mover = selectedCell.GetComponent<RailMover>();

		m_Mover.TweenToPosition(desiredPosition, 3f, false, iTween.EaseType.easeInOutSine);

		desiredPosition.z -= m_DistanceFromSubCell;

		// as the main container scales up/down,
		// TODO: Consider increasing/decreasing the m_DistanceFromSubcell along with it
		// so you're always a good distance away from the subcell

		m_MainCamera.GetComponent<RailMover>().TweenToPosition(desiredPosition, 3f, false, iTween.EaseType.easeInOutSine);

		// UIManager.Instance.ShowServiceSummaryView (selectedCell.ServiceDat);
	}

	public void EnterSelectedSubCell()
	{
		//ModelManager.Instance.HaltSubcells ();

		Camera.main.GetComponent<OnRailsMovement> ().Init (m_selectedCell.CaseCells);
		m_CachedPosition = m_MainCamera.transform.position;
		Vector3 desiredPosition = m_selectedCell.transform.position;
		Camera.main.GetComponent<RailMover>().TweenToPosition(desiredPosition, m_ZoomSpeed, gameObject, "AfterSubCellEntry");

		SynapseGenerator.Instance.GenerateSynapses(desiredPosition, m_selectedCell.gameObject);
	}

	public void AfterSubCellEntry()
	{
		UIManager.Instance.ShowCaseStudyView();
		Camera.main.GetComponent<OnRailsMovement> ().BeginCases ();
	}

	private void Update()
	{
		if (Input.touchCount == 2 && m_CurrentPhase == Phase.FocusedSubCellPhase)
		{
			HandleTwoFingers();
			return;
		}
		if (Input.touchCount == 1)
		{
			HandleOneFinger ();
		}
	}

	private void HandleOneFinger()
	{
		Touch touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Moved)
		{
			if (touch.deltaPosition.x < -m_touchThresholdX || touch.deltaPosition.x > m_touchThresholdX)
			{
				float angleVal = touch.deltaPosition.x * Time.deltaTime;
				Camera.main.transform.RotateAround (Vector3.zero, Vector3.up, angleVal);
				Camera.main.transform.LookAt (Vector3.zero);
			}
			if (touch.deltaPosition.y < -m_touchThresholdY || touch.deltaPosition.y > m_touchThresholdY)
			{
				float camPosMagnitude = (m_MainCamera.transform.position - Vector3.zero).magnitude;
				if (camPosMagnitude > m_camVectorMagLowerBound || touch.deltaPosition.y < 0f)
				{
					m_MainCamera.transform.Translate (transform.forward * touch.deltaPosition.y * Time.fixedDeltaTime);
				}
			}
		}
	}

	private void HandleTwoFingers()
	{
		Touch touchZero = Input.GetTouch(0);
		Touch touchOne = Input.GetTouch(1);

		// Find the position in the previous frame of each touch.
		Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
		Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

		// Find the magnitude of the vector (the distance) between the touches in each frame.
		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

		// Find the difference in the distances between each frame.
		float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

		ApplySubcellScale (deltaMagnitudeDiff * Time.deltaTime * -0.5f);	//-0.5 to invert axis and reduce sensitivity
	}

	private void ApplySubcellScale(float a_newScale)
	{
		if (m_selectedCell != null)
		{
			ModelManager.Instance.ScaleSubcell (m_selectedCell, a_newScale);
		}
	}
}
