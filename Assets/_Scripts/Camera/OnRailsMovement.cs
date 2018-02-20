using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(iTweenPath))]
public class OnRailsMovement : MonoBehaviour {

	public List<Vector3> caseStudyPositions = new List<Vector3>();
	private Vector3[] arr;
	public float speed;
	public iTween.EaseType ease;
	public iTweenPath thisPath;
	public float maxTime = 30;

	private float increment = 0;
	private float distanceBetweenPositions = 0;
	private float marginForError = 0;
	private bool allowMove;

	private float originAsIncrement = 0;
	private float destinationAsIncrement = 1;

    private string url = "";
    private int currentCaseStudy = 0;

	private float ourNodeCount = 1;
	int i = 0;

    [SerializeField]
    public int nodeI = 0;

    public static bool amDoing;
    public static bool amGoing;

    List<VideoClip> currentVids = new List<VideoClip>();

    [Header("leave me blank")]
    public string currentServiceType;


    CaseStudyDataOld[] c;

	// Use this for initialization
	void Awake() {
		iTween.Init (this.gameObject);
	}

    void Start()
    {
        
    }

	// IMPORTANT NOTE:
	// ONLY USE THIS SCRIPT IF THE CASE STUDY VIDEOS INSIDE THE SUBCELLS ARE GOING TO MOVE (THEY PROBABLY WONT)

	public void Initialise () {



        if (amDoing != true)
        {
            amDoing = true;
            i = 0;
            nodeI = 0;
            thisPath = GetComponent<iTweenPath>();
			/*
            // after we have created our path positions (aka, locations of the case studies)

            c = CaseStudyManager.Instance.allCases.ToArray();

            // do this a different way later, please.
            foreach (CaseStudyDataOld g in c)
            {
                caseStudyPositions.Add(g.transform.position);
            }

            arr = caseStudyPositions.ToArray();


			thisPath.nodeCount = caseStudyPositions.Count;
			thisPath.nodes = caseStudyPositions;

			*/
			arr = ModelManager.Instance.GetAllCaseCellPositions ();
			List<Vector3> csPositions = new List<Vector3> (arr);
			thisPath.nodeCount = csPositions.Count;
			thisPath.nodes = csPositions;


            MoveTo();
        }

        InitiliaseVideos();
	}

    void InitiliaseVideos()
    {
        VideoManager vidManager = VideoManager.Instance;
        switch (currentServiceType)
        {
            //case "AGILE":
            //    m_serviceType = ServiceType.AGILE;
            //    col = Color.magenta;
            //    break;
            //case "CONTENT":
            //    m_serviceType = ServiceType.CONTENT;
            //    col = Color.yellow;
            //    break;
            //case "DATA":
            //m_serviceType = ServiceType.DATA;
            //col = Color.grey;
            //break;
            case "FAST":
                currentVids = vidManager.m_FastVideos;
                break;
                //case "GROWTH":
                //    m_serviceType = ServiceType.GROWTH;
                //    col = Color.green;
                //    break;
                //case "LIFE":
                //    m_serviceType = ServiceType.LIFE;
                //    ColorUtility.TryParseHtmlString("#800080", out col);
                //    break;
                //case "LOOP":
                //    m_serviceType = ServiceType.LOOP;
                //    col = Color.cyan;
                //    break;
                //case "SHOP":
                //    m_serviceType = ServiceType.SHOP;
                //    col = Color.red;
                //    break;
                //default:
                //m_serviceType = ServiceType.FAST;
                //col = Color.blue;
                //break;
        }
    }

    void MoveTo()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", thisPath.nodes[nodeI], "time", 5, "easetype", iTween.EaseType.easeInOutSine, "orienttopath", true, "lookTime", 5, "oncomplete", "VideoReached"));
    }

    void VideoReached() 
    {
        StartCoroutine("PlayAndWait");
    }
	

    public void GoToNextPoint()
    {
        if (amGoing != true)
        {
            amGoing = true;

            VideoManager.Instance.StopVideo();
            Debug.Log("CURRENT NODE = " + nodeI.ToString() + " / and OUR NODE LIMIT = " + thisPath.nodeCount.ToString());
            if (nodeI >= thisPath.nodeCount)
            {
                // get out, we are done.
                CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
				CameraInputManager.Instance.ResetPosition();

            }
            else
            {
                MoveTo();
            }
        }
    }

    public void GoToPreviousPoint()
    {
        if (amGoing != true)
        {
            amGoing = true;

            VideoManager.Instance.StopVideo();
            Debug.Log("CURRENT NODE = " + nodeI.ToString() + " / and OUR NODE LIMIT = " + thisPath.nodeCount.ToString());
            if (nodeI <= 0)
            {
                // get out, we are done.

                CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
                CameraInputManager.Instance.ResetPosition(true);

            }
            else
            {
                nodeI -= 2;
                i -= 2;
                MoveTo();
            }
        }
    }

    public void GoHome()
    {
        VideoManager.Instance.StopVideo();

        nodeI = 0;
        i = 0;
        CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
        CameraInputManager.Instance.ResetPosition(true);
    }


	IEnumerator PlayAndWait() {
        Debug.Log("CURRENT VID INDEX = " + i.ToString());

		yield return new WaitForSeconds (2);
        amGoing = false;
        if ( nodeI > thisPath.nodeCount)
        {
            nodeI = 0;
            i = 0;
            CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
			CameraInputManager.Instance.ResetPosition (true);
        }
        else
        {
            VideoManager.Instance.PlayVideo(currentVids[nodeI]);
        }
        i++;
        nodeI++;
	}
}
