using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SynapseMover : MonoBehaviour {
	bool allowMove;
	float val = 0;
	float velocity = 1;
	Vector3[] arr;

	public float maxTime = 30;

    float elapsedTime = 0;
    float reversalTime;

    public static bool reversed = false;
    TrailRenderer thisTrail;

    public bool followEndCell;
    public Subcell endcell;
    public bool followStartCell;
    public Subcell startcell;

    List<Subcell> allCells = new List<Subcell>();

    public bool trackPositionsRealTime;
    Vector3 Mvelocity = Vector3.zero;
	// Use this for initialization
	public void Tweener(iTweenPath p) {
		arr = p.nodes.ToArray ();
		allowMove = true;
		// iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath(p.pathName), "time", (Speed), "orientToPath", true, "looktime", 2f, "oncomplete", "Reset", "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.easeInOutCubic));
	}

	void Awake() {
		iTween.Init (this.gameObject);
        thisTrail = GetComponent<TrailRenderer>();
        allCells = ModelManager.Instance.m_subcells;
        //Invoke("FindClosest", 0.5f);
       
	}


    private void OnEnable()
    {
        //InvokeRepeating("DoMovement", 0.1f, 3f);
    }

    void DoMovement()
    {

            if (trackPositionsRealTime == true)
        {
            FindClosest();
            StartCoroutine("RecalculatePositions",(this.GetComponent<LineRenderer>()));
        }
    }

    void FindClosest()
    {
        List<float> distances = new List<float>();

        foreach (Subcell c in allCells)
        {
            float d = Vector3.Distance(this.transform.position, c.transform.position);
            if (d > 1)
            {
                distances.Add(d);
            }
        }

        // AFTER U GET BACK, DO A DEBUG LOG HERE SHOWING THE CLOSEST SUBCELL AND THE ENDCELL POSITION
        // BECAUSE I DONT THINK THIS IS WORKING PROPERLY
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO 

        float closest = 0;

        for (int i = 0; i < distances.Count - 1; i++)
        {
            if (i == 0)
            {
                closest = distances[i];
                endcell = allCells[i];
            }
            if (distances[i] < closest)
            {
                closest = distances[i];
                endcell = allCells[i];
            }
        }
      
//        Debug.Log("FOUND CLOSEST = " + endcell.name);
    }

    IEnumerator RecalculatePositions(LineRenderer lineRend)
    {
        Vector3 startDis = lineRend.GetPosition(0);
        Vector3 endDis = lineRend.GetPosition(lineRend.positionCount-1);

        Vector3 trueVec = endDis / (lineRend.positionCount - 2);

        for (int i = 0; i < lineRend.positionCount; i++)
        {
            if (i != 0 && i != lineRend.positionCount)
            {
                lineRend.SetPosition(i, lineRend.GetPosition(i) + trueVec);
                yield return new WaitForSeconds(0.01f);
            }
        }


    }

	void Update() {

        if (followEndCell)
        {
            GetComponent<iTweenPath>().nodes[GetComponent<iTweenPath>().nodeCount-1] = endcell.transform.position;
        }
        if (followStartCell)
        {
            GetComponent<iTweenPath>().nodes[0] = startcell.transform.position;
        }

        if (followEndCell == true && followStartCell == true)
        {
            //TODO cache these come on man thats beginner shit
            // TODO do we even need to use the iTween path functionality anymore?
            GetComponent<LineRenderer>().SetPositions(Curver.MakeSmoothCurve(GetComponent<iTweenPath>().nodes.ToArray(), 0.25f));
            // GetComponent<LineRenderer>().SetPosition(GetComponent<LineRenderer>().positionCount-1, Vector3.SmoothDamp(transform.position, endcell.transform.position, ref Mvelocity, 2f));
            GetComponent<LineRenderer>().SetPosition(0, startcell.transform.position);
            GetComponent<LineRenderer>().SetPosition(GetComponent<LineRenderer>().positionCount - 1, endcell.transform.position);
            GetComponent<TrailRenderer>().enabled = false;
            //DoMovement();
        }

		if (allowMove)
			val = Mathf.SmoothStep(0, 1, Mathf.PingPong(Time.time/maxTime, 1));
        //Debug.Log("SYNAPSE MOVING");
		iTween.PutOnPath (this.gameObject, arr, val);

        if (this.val >= 0.99f && !followEndCell) {
       
            //Debug.Log ("DISABLE THE MOVER");
            allowMove = false;
		}
        else if (this.val >= 0.99f && followEndCell == true)
        {
            val = 0.99f;
            allowMove = true;
        }



        if (!reversed)
        {
            elapsedTime += Time.deltaTime;
        }

        else if (reversed==true)
        {
            thisTrail.time = (maxTime + elapsedTime);
                    // val = Mathf.SmoothStep(1, 0, Mathf.PingPong(Time.time / maxTime, 1));
            ////Debug.Log("SYNAPSE MOVING");
            //iTween.PutOnPath(this.gameObject, arr, val);

            //if (this.val <= 0.01f)
            //{
            //    //Debug.Log ("DISABLE THE MOVER");
            //    Destroy(this);
            //}
        }
	
	}
}
