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

    float Velocity = 0;
    public float smoothTime = 0.3f;
    private float increment = 0;

    private bool amToing = true;
    private bool amFroing;

    // Use this for initialization
    public void ReceivePoints (Subcell orig, Subcell dest) {
        origin = orig;
        destination = dest;

        MaximumDistanceRandomisation = Vector3.Distance(origin.transform.position, destination.transform.position) / 2;
        MinimumDistanceRandomisation = (MaximumDistanceRandomisation / 5);
 
        curvePoints.Add(origin.transform.position);
        curvePoints.Add(destination.transform.position);
        waypoints = Curver.MakeSmoothCurve(curvePoints.ToArray(), 5);

        if (destination == origin)
        {
            destination = ModelManager.Instance.m_subcells[Random.Range(0, 7)];
        }

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
        waypoints[0] = origin.transform.position;
        waypoints[waypoints.Length-1] = destination.transform.position;

        float m = waypoints.Length / 2;
        int a = Mathf.RoundToInt(m);
        waypoints[a] = (GetRandomDistance() * GetRandomDirection());

        waypoints = Curver.MakeSmoothCurve(waypoints, 1);
      
    }

}
