using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class ConnectionTracker : MonoBehaviour
{

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
    //private int existingConnectionsAmount;

    private GameObject cachedTrail;
    private bool alsoUsingTrails;

    public static List<Subcell> allCells = new List<Subcell>();

    private List<Subcell> tempCells = new List<Subcell>();

    List<Subcell> temps = new List<Subcell>();

    private GameObject extraConnections;
    private GameObject trailRenders;

    private Transform parenter;

    Subcell[] sortedCells = new Subcell[0];

    // Use this for initialization

    private void OnEnable()
    {
        line = GetComponent<LineRenderer>();
        allCells = new List<Subcell>(ModelManager.Instance.GetAllSubCells());

        if (allCells.Count < 2)
        {
            return;
        }

        tempCells = allCells;

        line.positionCount++;

        alsoUsingTrails = ConnectionGenerator.Instance.useTrails;

        existingConnections = GetComponentsInChildren<ConnectionTracker>();
        //existingConnectionsAmount = existingConnections.Length;
        parenter = ConnectionGenerator.Instance.baseParent.transform;
        this.transform.parent = parenter.transform;
        this.gameObject.name = "LineConnection";

        InvokeRepeating("FindClosest", 0.5f, ConnectionGenerator.Instance.ConnectionRefreshRate);

    }

    private void Update()
    {
        line.SetPosition(0, originCell.transform.position);
        line.SetPosition(1, trackCell.transform.position);
    }


    void FinalizeConnections()
    {

        switch (line.positionCount)
        {

            case 2:
                line.SetPosition(0, originCell.transform.position);
                line.SetPosition(1, trackCell.transform.position);

                if (line.GetComponent<ConnectionFadeAndTrack>() == null)
                {
                    line.gameObject.AddComponent<ConnectionFadeAndTrack>();
                    line.gameObject.GetComponent<ConnectionFadeAndTrack>().fadeOnDistance = ConnectionGenerator.Instance.MinimumDetectionDistance;
                    line.gameObject.GetComponent<ConnectionFadeAndTrack>().disappearOnDistance = ConnectionGenerator.Instance.MaximumDetectionDistance;
                    line.gameObject.GetComponent<ConnectionFadeAndTrack>().fadeDivider = ConnectionGenerator.Instance.FadeDivisionRate;
                    line.gameObject.GetComponent<LavaMoving>().uvAnimationRate = ConnectionGenerator.Instance.BasicAnimationDirections;

                }
                line.gameObject.GetComponent<ConnectionFadeAndTrack>().Assign(originCell, trackCell);

                break;
            case 3:
                line.SetPosition(0, originCell.transform.position);
                line.SetPosition(1, trackCell.transform.position);

                if (secondLink == null)
                {
                    GenerateSecondLink();
                }

                secondLink.GetComponent<ConnectionFadeAndTrack>().Assign(trackCell, secondClosestCell);

                break;



            case 4:
                line.SetPosition(0, originCell.transform.position);
                line.SetPosition(1, trackCell.transform.position);

                if (secondLink == null)
                {
                    GenerateSecondLink();
                }
                secondLink.GetComponent<ConnectionFadeAndTrack>().Assign(secondClosestCell, thirdClosestCell);

                if (thirdLink == null)
                {
                    GenerateThirdLink();
                }
                thirdLink.GetComponent<ConnectionFadeAndTrack>().Assign(thirdClosestCell, fourthClosestCell);


                break;
            default:
                line.SetPosition(0, originCell.transform.position);
                line.SetPosition(1, trackCell.transform.position);
                break;
        }

        line.positionCount = 2;

        if (alsoUsingTrails == true && cachedTrail == null)
        {
            TrailGeneration();
        }

    }

    private void GenerateSecondLink()
    {
        secondLink = new GameObject();
        secondLink.AddComponent<LineRenderer>();
        secondLink.transform.parent = parenter.transform;
        secondLink.name = "SecondLink";

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

    private void GenerateThirdLink()
    {
        thirdLink = new GameObject();
        thirdLink.transform.parent = parenter.transform;
        thirdLink.AddComponent<LineRenderer>();
        thirdLink.name = "ThirdLink";

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

    private void TrailGeneration()
    {
        GameObject trail = Instantiate(ConnectionGenerator.Instance.baseTrailObj, transform.position, Quaternion.identity);
        cachedTrail = trail;
        trail.transform.parent = parenter.transform;

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

    // you can now replace with Subcell.GetNearestNeighbours! srsly this cuts down all this shit below
    // good guy chris

    void FindClosest()
    {
        distances.Clear();

        foreach (Subcell c in allCells)
        {
            sortedCells = c.GetNearestNeighbours();
            //float d = Vector3.Distance(originCell.transform.position, c.transform.position);
            //if (c != originCell)
            //{
            //    distances.Add(d);
            //}
        }

        //float closest = 0;

        //distances.Min();

        //for (int i = 0; i < distances.Count; i++)
        //{
        //    if (i == 0)
        //    {
        //        closest = distances[i];
        //        trackCell = tempCells[i];

        //    }
        //    if (distances[i] < closest)
        //    {
        //        closest = distances[i];
        //        trackCell = tempCells[i];

        //    }
        //}

        trackCell = sortedCells[0];

        if (line.positionCount > 1 && allCells.Count > 2 ) // this check, not the best. // consider checking COUNT of AllCells instead?
        {
            FindSecondClosest();
        }
        else
        {
            FinalizeConnections();
        }
    }

    void FindSecondClosest()
    {
        secondClosestCell = sortedCells[1];
        //distances.Clear();
        //temps.Clear();

        //foreach (Subcell c in allCells)
        //{
        //    float d = Vector3.Distance(originCell.transform.position, c.transform.position);
        //    if (c != originCell && c != trackCell)
        //    {
        //        distances.Add(d);
        //        temps.Add(c);
        //    }
        //}

        //distances.Min();

        //float closest = 0;

        //for (int i = 0; i < distances.Count; i++)
        //{
        //    if (i == 0)
        //    {
        //        closest = distances[i];
        //        secondClosestCell = temps[i];
        //    }

        //    if (distances[i] < closest)
        //    {
        //        closest = distances[i];
        //        secondClosestCell = temps[i];
        //    }

        //}

        if (line.positionCount > 2 && allCells.Count > 3)
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
        thirdClosestCell = sortedCells[2];
        //distances.Clear();
        //temps.Clear();

        //foreach (Subcell c in allCells)
        //{
        //    float d = Vector3.Distance(originCell.transform.position, c.transform.position);
        //    if (c != originCell && c != trackCell && c != secondClosestCell)
        //    {
        //        distances.Add(d);
        //        temps.Add(c);
        //    }
        //}

        //distances.Min();

        //float closest = 0;

        //for (int i = 0; i < distances.Count; i++)
        //{
        //    if (i == 0)
        //    {
        //        closest = distances[i];
        //        thirdClosestCell = temps[i];
        //    }

        //    if (distances[i] < closest)
        //    {
        //        closest = distances[i];
        //        thirdClosestCell = temps[i];
        //    }
        //}

        if (line.positionCount > 3 && allCells.Count > 4)
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
        fourthClosestCell = sortedCells[3];
        //distances.Clear();
        //temps.Clear();

        //foreach (Subcell c in allCells)
        //{
        //    float d = Vector3.Distance(originCell.transform.position, c.transform.position);
        //    if (c != originCell && c != trackCell && c != secondClosestCell && c != thirdClosestCell)
        //    {
        //        distances.Add(d);
        //        temps.Add(c);
        //    }
        //}

        //distances.Min();

        //float closest = 0;

        //for (int i = 0; i < distances.Count; i++)
        //{
        //    if (i == 0)
        //    {
        //        closest = distances[i];
        //        fourthClosestCell = temps[i];
        //    }

        //    if (distances[i] < closest)
        //    {
        //        closest = distances[i];
        //        fourthClosestCell = temps[i];
        //    }
        //}

        FinalizeConnections();

    }

}
