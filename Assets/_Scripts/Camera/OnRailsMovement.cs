using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    int nodeI = 0;

    public static bool amDoing;
    public static bool amGoing;

    CaseStudyData[] c;

	// Use this for initialization
	void Awake() {
		iTween.Init (this.gameObject);
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
            // after we have created our path positions (aka, locations of the case studies)

            c = CaseStudyManager.Instance.allCases.ToArray();

            // do this a different way later, please.
            foreach (CaseStudyData g in c)
            {
                caseStudyPositions.Add(g.transform.position);
            }

            arr = caseStudyPositions.ToArray();

            thisPath.nodeCount = caseStudyPositions.Count;
            thisPath.nodes = caseStudyPositions;

            MoveTo();
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
				CameraInputManager.Instance.ResetPosition();

            }
            else
            {
                MoveTo();
            }
        }
    }


	IEnumerator PlayAndWait() {
        Debug.Log("CURRENT VID INDEX = " + i.ToString());
        string url = c[i].VideoPath;

        Debug.Log(url);
		yield return new WaitForSeconds (2);
        amGoing = false;
        if (url == "" || url == null || nodeI >= thisPath.nodeCount)
        {
            nodeI = 0;
            i = 0;
			CameraInputManager.Instance.ResetPosition ();
        }
        else
        {
            VideoManager.Instance.PlayVideo(url);
        }
        i++;
        nodeI++;
	}
}
