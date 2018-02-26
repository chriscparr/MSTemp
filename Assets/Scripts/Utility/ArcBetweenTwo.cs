using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArcBetweenTwo : MonoBehaviour {

    public Subcell origin;
    public Subcell destination;

    private float MaximumDistanceRandomisation;

    private float MinimumDistanceRandomisation;

    List<Vector3> curvePoints = new List<Vector3>();
    public Vector3[] waypoints;
    int point = 0;

    private float speed = 50;

    string RNG = "";
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    string sString = "";

    float Velocity = 0;
    public float smoothTime = 0.3f;
    private float increment = 0;

    private bool amToing = true;
    private bool amFroing;

    // Use this for initialization
    public void ReceivePoints (Subcell orig, Subcell dest) {
        origin = orig;
        destination = dest;

        for (int i = 0; i < 5; i++)
        {
            RNG += glyphs[Random.Range(0, glyphs.Length)];
        }

        MaximumDistanceRandomisation = Vector3.Distance(origin.transform.position, destination.transform.position) / 2;
        MinimumDistanceRandomisation = (MaximumDistanceRandomisation / 5);

       // curvePoints.Add((origin.transform.position + destination.transform.position) / 2);

        // got our points here
        // now make them an itween path, using whatever type u want
        // then oncomplete, re-randomize the MIDPOINT
        // update the fuckin, start and end point on update.
        // in fact, no, dont use an itween path
        // use Curver.
 
        curvePoints.Add(origin.transform.position);
        curvePoints.Add(destination.transform.position);
        waypoints = Curver.MakeSmoothCurve(curvePoints.ToArray(), 5);



        MidpointGeneration();
	}

	
	// Update is called once per frame
	void Update () {


        if (amToing == true)
        {
            increment = Mathf.SmoothDamp(increment, 1, ref Velocity, smoothTime);
            iTween.PutOnPath(gameObject, waypoints, increment);

            if (increment >= 0.99f)
            {
                amFroing = true;
                increment = 0.99f;
                MidpointGeneration();
                amToing = false;
            }
        }
        if (amFroing == true)
        {
            increment = Mathf.SmoothDamp(increment, 0, ref Velocity, smoothTime);
            iTween.PutOnPath(gameObject, waypoints, increment);
            if (increment <= 0.01f)
            {
                
                amToing = true;
                increment = 0.01f;
                MidpointGeneration();
                amFroing = false;
            }
        }

       // waypoints = Curver.MakeSmoothCurve(curvePoints.ToArray(), 5);
       

	}

    Vector3 GetRandomDirection()
    {
        Vector3 direction = Random.insideUnitSphere * GetRandomDistance();
        return direction;

    }

    float GetRandomDistance()
    {
        return Random.Range(MinimumDistanceRandomisation, MaximumDistanceRandomisation);
    }

    void MidpointGeneration()
    {
        // consider getting my true destination cell from either Tracker or Fade and Track
        // if u always want it to hit its closest cell
        // otherwise, just fuck it, looks good as it is.
        waypoints[0] = origin.transform.position;
        waypoints[waypoints.Length-1] = destination.transform.position;

        float m = waypoints.Length / 2;
        int a = Mathf.RoundToInt(m);
        waypoints[a] = (GetRandomDistance() * GetRandomDirection());

        waypoints = Curver.MakeSmoothCurve(waypoints, 1);
      
    }

}
