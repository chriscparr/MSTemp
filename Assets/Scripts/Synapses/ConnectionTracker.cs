﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class ConnectionTracker : MonoBehaviour {

    [HideInInspector]
    public Subcell trackCell;

    [HideInInspector]
    public Subcell originCell;

    [HideInInspector]
    public Subcell secondClosestCell;

    [HideInInspector]
    public Subcell thirdClosestCell;

    [HideInInspector]
    public Subcell fourthClosestCell;

    [HideInInspector]
    public List<float> distances = new List<float>();

    private GameObject secondLink;
    private GameObject thirdLink;
    private GameObject fourthLink;

    private LineRenderer line;
    private ConnectionTracker[] existingConnections;
    private int existingConnectionsAmount;

    private GameObject cachedTrail;
    private bool alsoUsingTrails;

    public static List<Subcell> allCells = new List<Subcell>();

    private List<Subcell> tempCells = new List<Subcell>();

    List<Subcell> temps = new List<Subcell>();

    private GameObject extraConnections;
    private GameObject trailRenders;

    // Use this for initialization

    private void OnEnable()
    {
        line = GetComponent<LineRenderer>();
        allCells = ModelManager.Instance.m_subcells;
        tempCells = allCells;

        line.positionCount++;

        alsoUsingTrails = ConnectionGenerator.Instance.useTrails;
       

        existingConnections = GetComponentsInChildren<ConnectionTracker>();
        existingConnectionsAmount = existingConnections.Length;
        Debug.Log("NUmber of existing connections = " + existingConnections.Length);


            InvokeRepeating("FindClosest", 0.5f, ConnectionGenerator.Instance.ConnectionRefreshRate);

    }

    private void Update()
    {
        line.SetPosition(0, originCell.transform.position);
        line.SetPosition(1, trackCell.transform.position);
    }

    // Update is called once per frame
    void FinalizeConnections () {
        Debug.Log(line.positionCount);
        switch (line.positionCount)
        {
            //case 0:
            //line.SetPosition(0, originCell.transform.position);
            //break;
            //case 1:
            //line.SetPosition(0, originCell.transform.position);
            //line.SetPosition(1, trackCell.transform.position);
            //break;
            case 2:
                line.SetPosition(0, originCell.transform.position);
                line.SetPosition(1, trackCell.transform.position);

                if (line.GetComponent<ConnectionFadeAndTrack>() == null)
                {
                    line.gameObject.AddComponent<ConnectionFadeAndTrack>();
                    line.gameObject.GetComponent<ConnectionFadeAndTrack>().fadeOnDistance = ConnectionGenerator.Instance.MinimumDetectionDistance;
                    // line.gameObject.AddComponent<LavaMoving>();
                    line.gameObject.GetComponent<ConnectionFadeAndTrack>().disappearOnDistance = ConnectionGenerator.Instance.MaximumDetectionDistance;
                    line.gameObject.GetComponent<ConnectionFadeAndTrack>().fadeDivider = ConnectionGenerator.Instance.FadeDivisionRate;
                    line.gameObject.GetComponent<LavaMoving>().uvAnimationRate = ConnectionGenerator.Instance.BasicAnimationDirections;

                }
                line.gameObject.GetComponent<ConnectionFadeAndTrack>().Assign(originCell, trackCell);

                break;
            case 3:
                line.SetPosition(0, originCell.transform.position);
                line.SetPosition(1, trackCell.transform.position);
                // line.SetPosition(2, secondClosestCell.transform.position);

                if (secondLink == null)
                {
                    secondLink = new GameObject();
                    // secondLink.transform.parent = extraConnections.transform;
                    secondLink.AddComponent<LineRenderer>();

                    secondLink.GetComponent<LineRenderer>().material = ConnectionGenerator.Instance.StartingMaterial;
                    secondLink.GetComponent<LineRenderer>().startWidth = ConnectionGenerator.Instance.StartingWidth;
                    secondLink.GetComponent<LineRenderer>().endWidth = ConnectionGenerator.Instance.EndingWidth;
                    secondLink.GetComponent<LineRenderer>().positionCount = 2;

                    secondLink.AddComponent<ConnectionFadeAndTrack>();
                    secondLink.GetComponent<ConnectionFadeAndTrack>().fadeOnDistance = ConnectionGenerator.Instance.MinimumDetectionDistance;
                    secondLink.GetComponent<ConnectionFadeAndTrack>().disappearOnDistance = ConnectionGenerator.Instance.MaximumDetectionDistance;
                    secondLink.GetComponent<ConnectionFadeAndTrack>().fadeDivider = ConnectionGenerator.Instance.FadeDivisionRate;
                    secondLink.AddComponent<LavaMoving>();
                    secondLink.GetComponent<LavaMoving>().uvAnimationRate = ConnectionGenerator.Instance.BasicAnimationDirections;
                }
                // ConnectionFadeAndTrack cTrack = mline.GetComponent<ConnectionFadeAndTrack>();
                secondLink.GetComponent<ConnectionFadeAndTrack>().Assign(trackCell, secondClosestCell);


                break;



            case 4:
                line.SetPosition(0, originCell.transform.position);
                line.SetPosition(1, trackCell.transform.position);
                //line.SetPosition(2, secondClosestCell.transform.position);
                //line.SetPosition(3, thirdClosestCell.transform.position);

                if (secondLink == null)
                {
                secondLink = new GameObject();
                    // secondLink.transform.parent = extraConnections.transform;
                secondLink.AddComponent<LineRenderer>();

                secondLink.GetComponent<LineRenderer>().material = ConnectionGenerator.Instance.StartingMaterial;
                secondLink.GetComponent<LineRenderer>().startWidth = ConnectionGenerator.Instance.StartingWidth;
                secondLink.GetComponent<LineRenderer>().endWidth = ConnectionGenerator.Instance.EndingWidth;
                secondLink.GetComponent<LineRenderer>().positionCount = 2;

                secondLink.AddComponent<ConnectionFadeAndTrack>();
                secondLink.GetComponent<ConnectionFadeAndTrack>().fadeOnDistance = ConnectionGenerator.Instance.MinimumDetectionDistance;
                    secondLink.GetComponent<ConnectionFadeAndTrack>().disappearOnDistance = ConnectionGenerator.Instance.MaximumDetectionDistance;
                    secondLink.GetComponent<ConnectionFadeAndTrack>().fadeDivider = ConnectionGenerator.Instance.FadeDivisionRate;
                    secondLink.AddComponent<LavaMoving>();
                secondLink.GetComponent<LavaMoving>().uvAnimationRate = ConnectionGenerator.Instance.BasicAnimationDirections;
        }
                secondLink.GetComponent<ConnectionFadeAndTrack>().Assign(secondClosestCell, thirdClosestCell);

                if (thirdLink == null)
                {
                    thirdLink = new GameObject();
                    // thirdLink.transform.parent = extraConnections.transform;
                    thirdLink.AddComponent<LineRenderer>();

                    thirdLink.GetComponent<LineRenderer>().material = ConnectionGenerator.Instance.StartingMaterial;
                    thirdLink.GetComponent<LineRenderer>().startWidth = ConnectionGenerator.Instance.StartingWidth;
                    thirdLink.GetComponent<LineRenderer>().endWidth = ConnectionGenerator.Instance.EndingWidth;
                    thirdLink.GetComponent<LineRenderer>().positionCount = 2;

                    thirdLink.AddComponent<ConnectionFadeAndTrack>();
                    thirdLink.GetComponent<ConnectionFadeAndTrack>();
                    thirdLink.GetComponent<ConnectionFadeAndTrack>().fadeOnDistance = ConnectionGenerator.Instance.MinimumDetectionDistance;
                    thirdLink.GetComponent<ConnectionFadeAndTrack>().disappearOnDistance = ConnectionGenerator.Instance.MaximumDetectionDistance;
                    thirdLink.GetComponent<ConnectionFadeAndTrack>().fadeDivider = ConnectionGenerator.Instance.FadeDivisionRate;
                    thirdLink.AddComponent<LavaMoving>();
                    thirdLink.GetComponent<LavaMoving>().uvAnimationRate = ConnectionGenerator.Instance.BasicAnimationDirections;
                }
                thirdLink.GetComponent<ConnectionFadeAndTrack>().Assign(thirdClosestCell, fourthClosestCell);


                    break;
                default:
                    line.SetPosition(0, originCell.transform.position);
                    line.SetPosition(1, trackCell.transform.position);
                    break;
            }
        line.positionCount = 2;
        //Debug.Log("CONNECTIONS FINALIZED");


        if (alsoUsingTrails == true && cachedTrail == null)
        {
            GameObject trail = Instantiate(ConnectionGenerator.Instance.baseTrailObj, transform.position, Quaternion.identity);
            cachedTrail = trail;
            // trail.transform.parent = trailRenders.transform;
            if (trail.GetComponent<Renderer>() != null)
            {
                trail.GetComponent<Renderer>().sharedMaterial = ConnectionGenerator.Instance.baseTrailMaterial;
            }
            trail.AddComponent<TrailRenderer>();
            trail.GetComponent<TrailRenderer>().time = ConnectionGenerator.Instance.trailDuration;
            trail.GetComponent<TrailRenderer>().sharedMaterial = ConnectionGenerator.Instance.trailMat;
            trail.GetComponent<TrailRenderer>().widthMultiplier = ConnectionGenerator.Instance.trailWidth;
            trail.GetComponent<TrailRenderer>().textureMode = ConnectionGenerator.Instance.trailTexType;

            trail.AddComponent<ArcBetweenTwo>();
            trail.GetComponent<ArcBetweenTwo>().ReceivePoints(originCell, trackCell);
            trail.GetComponent<ArcBetweenTwo>().smoothTime = ConnectionGenerator.Instance.trailSpeed;
        }

	}




    void FindClosest()
    {
        distances.Clear();

        //if (existingConnectionsAmount <= 1)
        //{
            foreach (Subcell c in allCells)
            {
                float d = Vector3.Distance(originCell.transform.position, c.transform.position);
                if (c != originCell)
                {
                    distances.Add(d);

                }
         }

        float closest = 0;

        distances.Min();

        for (int i = 0; i < distances.Count; i++)
        {
            if (i == 0)
            {
                closest = distances[i];
                trackCell = tempCells[i];

            }
            if (distances[i] < closest)
            {
                closest = distances[i];
                trackCell = tempCells[i];

            }
        }
        if (line.positionCount > 1)
        {
            FindSecondClosest();
        }
        else
        {
            FinalizeConnections();
        }
    }

    void FindSecondClosest() {
        distances.Clear();
        temps.Clear();
        //if (existingConnectionsAmount <= 1)
        //{
        foreach (Subcell c in allCells)
        {
            float d = Vector3.Distance(originCell.transform.position, c.transform.position);
            if (c != originCell && c != trackCell)
            {
                distances.Add(d);
                temps.Add(c);
            }
        }

        distances.Min();

        float closest = 0;

        for (int i = 0; i < distances.Count; i++)
        {
            if (i == 0)
            {
                closest = distances[i];
                secondClosestCell = temps[i];
            }

            if (distances[i] < closest)
            {
                closest = distances[i];
                secondClosestCell = temps[i];
            }

        }

        if (line.positionCount > 2)
        {
            FindThirdClosest();
        }
        else
        {
            FinalizeConnections();
        }

    }

    void FindThirdClosest()
    {
        distances.Clear();
        temps.Clear();
        //if (existingConnectionsAmount <= 1)
        //{
        foreach (Subcell c in allCells)
        {
            float d = Vector3.Distance(originCell.transform.position, c.transform.position);
            if (c != originCell && c != trackCell && c != secondClosestCell)
            {
                distances.Add(d);
                temps.Add(c);
            }
        }

        distances.Min();

        float closest = 0;

        for (int i = 0; i < distances.Count; i++)
        {
            if (i == 0)
            {
                closest = distances[i];
                thirdClosestCell = temps[i];
            }

            if (distances[i] < closest)
            {
                closest = distances[i];
                thirdClosestCell = temps[i];
            }
        }

        if (line.positionCount > 3)
        {
            FindFourthClosest();
        }
        else
        {
            FinalizeConnections();
        }

    }

    void FindFourthClosest()
    {
        distances.Clear();
        temps.Clear();
        //if (existingConnectionsAmount <= 1)
        //{
        foreach (Subcell c in allCells)
        {
            float d = Vector3.Distance(originCell.transform.position, c.transform.position);
            if (c != originCell && c != trackCell && c != secondClosestCell && c != thirdClosestCell)
            {
                distances.Add(d);
                temps.Add(c);
            }
        }

        distances.Min();

        float closest = 0;

        for (int i = 0; i < distances.Count; i++)
        {
            if (i == 0)
            {
                closest = distances[i];
                fourthClosestCell = temps[i];
            }

            if (distances[i] < closest)
            {
                closest = distances[i];
                fourthClosestCell = temps[i];
            }
        }

//        Debug.Log(secondClosestCell.name + "/' " + thirdClosestCell.name + "/" + fourthClosestCell.name);

     //   Debug.Log("FOUND FOURTH CLOSEST YES");
        FinalizeConnections();

        //if (line.positionCount > 3)
        //{
        //    FindThirdClosest();
        //}

    }

}
