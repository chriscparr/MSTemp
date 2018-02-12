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

    private void Awake() 
    {
        s_instance = this;
        m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
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

    public void FocusOnSubCell(Subcell selectedCell)
    {
        m_selectedCell = selectedCell;
        m_CachedPosition = m_MainCamera.transform.position;
        m_CurrentTarget = selectedCell.transform;
        Vector3 desiredPosition = selectedCell.transform.position;
        desiredPosition.z -= m_DistanceFromSubCell;

        SetLookAtTarget(m_selectedCell.transform);
        m_Mover.TweenToPosition(desiredPosition, m_ZoomSpeed, false, iTween.EaseType.easeInOutSine);
		UIManager.Instance.ShowServiceSummaryView (selectedCell.ServiceDat);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_CurrentPhase != Phase.SetupPhase)
        {
            #region Main Cell Movement
            if (m_CurrentPhase == Phase.MainCellPhase)
            {
                // ONE TOUCH TO SWIPE
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);

                    Vector3 axis = touch.deltaPosition;

                    // i swear this is needed
                    Vector3 correction = axis;
                    axis.x = correction.y;
                    axis.y = correction.x;

                    // Vector3 axis = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

                    m_MainCamera.transform.LookAt(m_CurrentTarget);
                    m_MainCamera.transform.RotateAround(m_CurrentTarget.position, axis, Time.deltaTime * m_RotationSpeed);
                }
            }
            #endregion

            // If there are two touches on the device...
            if (Input.touchCount == 2)
            {
                HandleTwoFingers();
            }

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

        if (m_CurrentPhase == Phase.MainCellPhase)
        {
            m_MainCamera.transform.Translate(transform.forward * (deltaMagnitudeDiff * Time.deltaTime));
        }
        if (m_CurrentPhase == Phase.FocusedSubCellPhase)
        {
            // there is probably a much nicer way of doing this - consider Tween pls
            float gentleDelta = (deltaMagnitudeDiff / 100);
            m_selectedCell.transform.localScale -= new Vector3(gentleDelta, gentleDelta, gentleDelta);
        }
    }
     

}
