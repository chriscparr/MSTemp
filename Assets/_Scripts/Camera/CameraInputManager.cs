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

    public float zAxisLimit = 0;

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

    private Transform m_CurrentTarget;
    private Vector3 m_CachedPosition;
    private Subcell m_selectedCell;
    private RailMover m_Mover;
    private Vector3 m_SafetyScaleVector = new Vector3(3,3,3);

	private Vector2 m_initialTouchPosition1;

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

	public void ResetPosition()
	{
		Camera.main.transform.position = m_CachedPosition;
		Camera.main.transform.LookAt (Vector3.zero);
	}

    public void FocusOnSubCell(Subcell selectedCell)
    {
        m_selectedCell = selectedCell;
        m_CachedPosition = m_MainCamera.transform.position;
        m_CurrentTarget = selectedCell.transform;

		Vector3 desiredPosition = Vector3.zero;	//m_mainContainer is always at zero, lets keep it private if we can...
       

        selectedCell.RigidBody.isKinematic = true;
        selectedCell.gameObject.AddComponent<RailMover>();
        m_Mover = selectedCell.GetComponent<RailMover>();

        m_Mover.TweenToPosition(desiredPosition, 5f, false, iTween.EaseType.easeInOutSine);

        desiredPosition.z -= m_DistanceFromSubCell;

        // as the main container scales up/down,
        // TODO: Consider increasing/decreasing the m_DistanceFromSubcell along with it
        // so you're always a good distance away from the subcell
        m_MainCamera.GetComponent<RailMover>().TweenToPosition(desiredPosition, 5f, false, iTween.EaseType.easeInOutSine);

		// UIManager.Instance.ShowServiceSummaryView (selectedCell.ServiceDat);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (Input.GetKey(KeyCode.P))
		{
			DebugMoveCamera ();
		}
		// If there are two touches on the device...
		if (Input.touchCount == 2 && m_CurrentPhase == Phase.FocusedSubCellPhase)
		{
			HandleTwoFingers();
			return;
		}

		if (Input.touchCount == 1)
		{
			if (Input.GetTouch (0).phase == TouchPhase.Began)
			{
				// i swear this is needed
				Vector2 output = Vector2.zero;
				Vector2 correction = Input.GetTouch (0).position;
				output.x = correction.y;
				output.y = correction.x;
				m_initialTouchPosition1 = output;
			}
			HandleOneFinger ();
		}
    }

	private void DebugMoveCamera()
	{
		Vector3 origin = Vector3.zero;
		//Vector3 camera = Camera.main.transform.localPosition;
		float angleVal = 10f * m_ZoomSpeed * Time.deltaTime;
		Camera.main.transform.RotateAround (origin, Vector3.up, angleVal);
		Camera.main.transform.LookAt (origin);
	}

	private void HandleOneFinger()
	{
		Touch touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Moved)
		{
			Vector2 axis = touch.deltaPosition;
			
			// i swear this is needed
			Vector2 correction = axis;
			axis.x = correction.y;
			axis.y = correction.x;

			Vector2 touchPositionDelta = axis - m_initialTouchPosition1;

			Vector3 origin = Vector3.zero;
			float angleVal = touchPositionDelta.x * Time.fixedDeltaTime;
			Camera.main.transform.RotateAround (origin, Vector3.up, angleVal);
			Camera.main.transform.LookAt (origin);


			m_MainCamera.transform.Translate (origin * touchPositionDelta.y * Time.fixedDeltaTime);

			//m_MainCamera.transform.Translate(transform.forward * touchPositionDelta.y * Time.deltaTime);

			if (m_MainCamera.transform.position.z >= zAxisLimit)
			{
				m_MainCamera.transform.position = new Vector3(
					m_MainCamera.transform.position.x,
					m_MainCamera.transform.position.y,
					zAxisLimit);
			}


			/*
			if (touchPositionDelta.x > touchPositionDelta.y * 5 || touchPositionDelta.x < touchPositionDelta.y * -5)
			{
				Debug.Log ("HORIZONTAL DRAG!");
				if (touch.deltaPosition.x > 0f)
				{
					Vector3 origin = Vector3.zero;
					float angleVal = 10f * m_ZoomSpeed * Time.deltaTime;
					Camera.main.transform.RotateAround (origin, Vector3.up, angleVal);
					Camera.main.transform.LookAt (origin);
				}
				if (touch.deltaPosition.x < 0f)
				{
					Vector3 origin = Vector3.zero;
					float angleVal = -10f * m_ZoomSpeed * Time.deltaTime;
					Camera.main.transform.RotateAround (origin, Vector3.up, angleVal);
					Camera.main.transform.LookAt (origin);
				}
				return;
			}
			if (touchPositionDelta.y > touchPositionDelta.x * 5 || touchPositionDelta.y < touchPositionDelta.x * -5)
			{
				Debug.Log ("VERTICAL DRAG!");
				if (touch.deltaPosition.y > 0f)
				{
					m_MainCamera.transform.Translate(transform.forward * m_ZoomSpeed * Time.deltaTime);
				}
				if (touch.deltaPosition.y < 0f)
				{
					m_MainCamera.transform.Translate(-transform.forward * m_ZoomSpeed * Time.deltaTime);
				}


				if (m_MainCamera.transform.position.z >= zAxisLimit)
				{
					m_MainCamera.transform.position = new Vector3(
						m_MainCamera.transform.position.x,
						m_MainCamera.transform.position.y,
						zAxisLimit);
				}
			}
			*/


		}

	}

    void HandleTwoFingers()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        float gentleDelta = (deltaMagnitudeDiff / 100);



        Debug.Log("OUR DELTA MAGNITUDE IS " + gentleDelta + " AND OUR SCALE IS " + m_selectedCell.transform.localScale );
        m_selectedCell.transform.localScale = new Vector3(m_selectedCell.transform.localScale.x + (gentleDelta * -1),
                                                          m_selectedCell.transform.localScale.y + (gentleDelta * -1),
                                                          m_selectedCell.transform.localScale.z + (gentleDelta * -1));

        if (m_selectedCell.transform.localScale.x >= 3)
        {
            m_selectedCell.transform.localScale = m_SafetyScaleVector;
        }
        if (m_selectedCell.transform.localScale.x <= 0)
        {
            m_selectedCell.transform.localScale = Vector3.zero;
        }

        ModelManager.Instance.ScaleSubcell(m_selectedCell, m_selectedCell.transform.localScale.z);
    }
     

}
