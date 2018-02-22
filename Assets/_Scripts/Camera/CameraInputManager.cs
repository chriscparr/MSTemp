using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraInputManager : MonoBehaviour 
{

	public enum Phase
	{
		SetupPhase,
		MainCellPhase,
		FocusedSubCellPhase,
		InsideSubCellPhase
	}

	public static CameraInputManager Instance { get { return s_instance; } }
	private static CameraInputManager s_instance = null;

	private float m_camVectorMagLowerBound = 5f;
	private float m_camVectorMagHigherBound = -5f;

	public Subcell SelectedCell { get { return m_selectedCell; } }
	private Subcell m_selectedCell;

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

	private RailMover m_Mover;

	[Header("Preset rail positions")]
	public Vector3 homeVector = new Vector3(0, 0, -15f);
	public Vector3 focusVector = new Vector3(0, 0, -7.5f);


	private bool m_isTweening = false;


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
				// ban all movement here
				break;
			case Phase.MainCellPhase:
				if (GameObject.Find("SynapseGenerator"))
				{
					FindObjectOfType<SynapseGenerator>().enabled = true;
				}
				Debug.Log("RETURNING TO MAIN STATE");
				// allow rotation and pinch and zoom to move in? google earth controls
				// dont forget to use the actual f'n event handlers
				break;
			case Phase.FocusedSubCellPhase:
				Debug.Log("WE ARE NOW FOCUSED ON THE SUBCELL");
				// here we pinch and zoom to scale the sub cell, so you may not need to do anything
				break;
			case Phase.InsideSubCellPhase:
				// here we want to do case study stuff, so enable BOTH OnRailsMovement and RailMover, put buttons to move to/from paths (do that listener and event stuff)
				// ps learn about listeners and events already. 
				break;

		}
	}

	public void ResetPosition(bool doSmoothly = true)
	{
		// TODO make me a tween, so we gradually go back to the main phase, dont just snap it back.
		if (!doSmoothly)
		{
			Camera.main.transform.position = m_CachedPosition;
			Camera.main.transform.eulerAngles = Vector3.zero;
		}
		else
		{
			m_MainCamera.GetComponent<RailMover>().TweenToPosition(homeVector, 2, false, iTween.EaseType.easeInOutSine);

			Camera.main.transform.eulerAngles = Vector3.zero;
			UIManager.Instance.ShowPresentationView ();

			if (m_selectedCell != null)
			{
				FocusReset ();
			}
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
		m_selectedCell = selectedCell;
		m_CurrentTarget = selectedCell.transform;

		Vector3 desiredPosition = Vector3.zero;	//m_mainContainer is always at zero, lets keep it private if we can...

		m_selectedCell.CanScale = true;

		selectedCell.RigidBody.isKinematic = true;
		selectedCell.gameObject.AddComponent<RailMover>();
		m_Mover = selectedCell.GetComponent<RailMover>();

		m_Mover.TweenToPosition(desiredPosition, 2f, false, iTween.EaseType.easeInOutSine);

		m_MainCamera.GetComponent<RailMover>().TweenToPosition(focusVector, 2f, false, iTween.EaseType.easeInOutSine);
		UIManager.Instance.ShowManipulationView();
		// UIManager.Instance.ShowServiceSummaryView (selectedCell.ServiceDat);
	}

	public void EnterSelectedSubCell()
	{
		if (m_selectedCell != null)
		{
			SetPhase (Phase.InsideSubCellPhase);
			UIManager.Instance.HideAllViews ();
			Camera.main.GetComponent<OnRailsMovement> ().Init (m_selectedCell.CaseCells);
			m_CachedPosition = m_MainCamera.transform.position;
			Vector3 desiredPosition = m_selectedCell.transform.position;
			Camera.main.GetComponent<RailMover>().TweenToPosition(desiredPosition, m_ZoomSpeed, gameObject, "AfterSubCellEntry");
			
			SynapseGenerator.Instance.GenerateSynapses(desiredPosition, m_selectedCell.gameObject);
		}
	}

	public void AfterSubCellEntry()
	{
		UIManager.Instance.ShowCaseStudyView();
		Camera.main.GetComponent<OnRailsMovement> ().BeginCases ();
	}

	private void FixedUpdate()
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
		/*
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (m_isTweening != true)
			{
				m_Mover.TweenToPosition(homeVector, 2, true, iTween.EaseType.easeInOutSine);
				m_isTweening = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.U))
		{
			UIManager.Instance.HideManipulationView();
			ResetPosition();

		}

		if (Input.GetKeyDown(KeyCode.Y))
		{
			if (m_selectedCell.GetComponent<Renderer>().sharedMaterial != m_selectedCell.myOnMaterial && m_selectedCell != null)
				m_selectedCell.GetComponent<Renderer>().sharedMaterial = m_selectedCell.myOnMaterial;
		}
		*/
	}

	private void HandleOneFinger()
	{
		if (m_isTweening)
		{
			return;
		}

		Touch touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Moved)
		{
			//If we've passed the threshold for Horizontal swipe detection:
			if (touch.deltaPosition.x < -m_touchThresholdX || touch.deltaPosition.x > m_touchThresholdX)
			{
				switch (m_CurrentPhase)
				{
					case Phase.MainCellPhase:
						float angleVal = touch.deltaPosition.x * Time.deltaTime;
						Camera.main.transform.RotateAround (Vector3.zero, Vector3.up, angleVal);
						Camera.main.transform.LookAt (Vector3.zero);
						break;
				}
			}
			//If we've passed the threshold for Vertical swipe detection:
			else if (touch.deltaPosition.y < -m_touchThresholdY || touch.deltaPosition.y > m_touchThresholdY)
			{
				//swipe up
				if (touch.deltaPosition.y > 0f)
				{
					switch (m_CurrentPhase)
					{
						case Phase.MainCellPhase:
							Debug.Log("WE ARE MOVING TOWARDS THE MAIN CELL");
							m_Mover.TweenToPosition(homeVector, 2, true, iTween.EaseType.easeInOutSine);
							m_isTweening = true;
							break;
						case Phase.FocusedSubCellPhase:
							Debug.Log("WE ARE MOVING TOWARDS SUBCELL: " + m_selectedCell.ServiceDat.ServiceName);
							EnterSelectedSubCell();
							SetPhase(Phase.InsideSubCellPhase);
							break;
					}
				}
				//swipe down
				if (touch.deltaPosition.y < 0f)
				{
					switch (m_CurrentPhase)
					{
						case Phase.InsideSubCellPhase:
							Debug.Log("WE ARE LEAVING SUBCELL: " + m_selectedCell.ServiceDat.ServiceName);
							EnterSelectedSubCell();
							SetPhase(Phase.InsideSubCellPhase);
							break;
						case Phase.FocusedSubCellPhase:
							Debug.Log ("WE ARE MOVING BACK TOWARDS THE MAIN CELL");
							ResetPosition ();
							break;
					}
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
		if (m_selectedCell != null && m_selectedCell.CanScale == true)
		{
			ModelManager.Instance.ScaleSubcell (m_selectedCell, a_newScale);
			if (m_selectedCell.GetComponent<Renderer>().sharedMaterial != m_selectedCell.myOnMaterial && m_selectedCell != null)
				m_selectedCell.GetComponent<Renderer>().sharedMaterial = m_selectedCell.myOnMaterial;
		}
	}
}
