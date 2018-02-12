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
    private float m_DistanceFromSubCell = -25;
    [SerializeField]
    private float m_DistanceInsideSubcell = -3;
    [SerializeField]
    private float zLimitForMainView = 0;

    private Transform m_CurrentTarget;
    private Vector3 m_CachedPosition;
    private Subcell m_selectedCell;
    private RailMover m_Mover;
    private bool m_Following;
    private bool m_Entering;
    float dist;


    private void Awake() 
    {
        s_instance = this;
        m_MainCamera = Camera.main.gameObject;
        m_CachedPosition = m_MainCamera.transform.position;
        dist = m_DistanceFromSubCell;
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

    public void FollowTarget()
    {
        m_Following = true;
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
                    // temporary as each subcell SHOULD hve its own synapse generator i guess?
                    FindObjectOfType<SynapseGenerator>().enabled = true;
                }
                // allow rotation and pinch and zoom to move in? google earth controls
                // dont forget to use the actual f'n event handlers
                break;
            case Phase.FocusedSubCellPhase:
                // here we pinch and zoom to scale the sub cell, so you may not need to do anything
                break;
            case Phase.InsideSubCellPhase:
                DealWithSubCell(CameraInputManager.Instance.m_DistanceFromSubCell, CameraInputManager.Instance.m_DistanceInsideSubcell);
                break;

        }
    }

    public void FocusOnSubCell(Subcell selectedCell)
    {
        m_selectedCell = selectedCell;
        m_CachedPosition = m_MainCamera.transform.position;
        m_CurrentTarget = selectedCell.transform;
        Vector3 desiredPosition = m_MainCamera.transform.position;
        desiredPosition.z -= m_DistanceFromSubCell;

        selectedCell.RigidBody.isKinematic = true;

        selectedCell.GetComponent<RailMover>().TweenToPosition(desiredPosition, 5f, true, iTween.EaseType.easeInSine);

    }

    public void DealWithSubCell(float from, float to)
    {
        float m = m_DistanceFromSubCell;
        Debug.Log("DAMP ON THEM HATERS");
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", from,
            "to", to,
            "time", m_ZoomSpeed,
    "onupdatetarget", gameObject,
    "onupdate", "ReassignDistance",
    "easetype", iTween.EaseType.easeOutQuad

    )
);
    }

        void ReassignDistance(float newValue)
        {
        Debug.Log("DAMPING ON THEM HATES ALL DAY BABEY");
            dist = newValue;
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

                    //// Vector3 axis = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

                    //m_MainCamera.transform.LookAt(m_CurrentTarget);
                    //m_MainCamera.transform.RotateAround(m_CurrentTarget.position, axis, Time.deltaTime * m_RotationSpeed);


                    // Move object across XY plane
                    if (touch.deltaPosition.y > 0)
                    {
                        m_MainCamera.transform.Translate(transform.forward * m_ZoomSpeed * Time.deltaTime);
                    }
                    if (touch.deltaPosition.y < 0)
                    {
                        m_MainCamera.transform.Translate(-transform.forward * m_ZoomSpeed * Time.deltaTime);
                    }

                    if (m_MainCamera.transform.position.z >= zLimitForMainView)
                    {
                        m_MainCamera.transform.position = new Vector3(m_MainCamera.transform.position.x, m_MainCamera.transform.position.y, zLimitForMainView);
                    }

                }
            }

                //if (m_Following)
                //{
                //// spoiler alert, this doesnt point to the very center of the cell, it points to the edge of it?
                //    m_MainCamera.transform.position = m_CurrentTarget.transform.position + new Vector3(0f, 0f, dist);
                //}

            #endregion

            // If there are two touches on the device...
            if (Input.touchCount == 2 && m_CurrentPhase == Phase.FocusedSubCellPhase)
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

        // Find the magnitude of the vector (the distance) between the touches in each frame.
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        if (m_CurrentPhase == Phase.FocusedSubCellPhase)
        {
            Debug.Log("OUR DELTA IS " + deltaMagnitudeDiff + "AND OUR SCALE IS " + m_selectedCell.transform.localScale);
            // there is probably a much nicer way of doing this - consider Tween pls
            // m_selectedCell.transform.localScale = new Vector3(m_sel)
        }
    }
     

}
