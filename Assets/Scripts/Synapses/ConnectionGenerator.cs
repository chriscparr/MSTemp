using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConnectionGenerator : MonoBehaviour {


    public static ConnectionGenerator Instance { get { return s_instance; } }
    private static ConnectionGenerator s_instance = null;

    [Header("For Line Renderers")]
    public bool useLineRenderers = true;

    [Header("Between 2 and 4, must be like this")]
    [Range(2,4)]
    public int MaximumNumberOfConnections;
    [Header("Tinker with these values for testing")]
    [Header("Maximum should be at least double the minimum")]
    [Range(3,10)]
    public int MinimumDetectionDistance;
    [Range(7,25)]
    public int MaximumDetectionDistance;
    [Header("Lower values mean stronger fade")]
    [Range(2, 6)]
    public int FadeDivisionRate;

    [Header("Lower values = faster refresh but performance hits")]
    [Range(1,8)]
    public int ConnectionRefreshRate;

    [Header("Change me if you don't want lines to go invisible")]
    public float MinimumLineAlpha = 0;
    public float MaximumLineAlpha = 255;

    [Header("Line renderer settings")]
    public Material StartingMaterial;
    public float StartingWidth = 0.5f;
    public float EndingWidth = 0.5f;
    public bool ShouldWeDoBasicAnimation;
    public Vector2 BasicAnimationDirections = new Vector2(1, 0);

    [HideInInspector]
    public List<Subcell> allCells = new List<Subcell>();
    private GameObject par;

    [Header("For Trail Renderers")]
    public bool useTrails;
    public GameObject baseTrailObj;
    [Header("Material for base object")]
    public Material baseTrailMaterial;
    [Header("Material for trail itself")]
    public Material trailMat;
    [Header("Keep speed lower than duration")]
    public float trailSpeed = 0.3f;
    public float trailDuration = 0.5f;
    public float trailWidth = 2;
    [Header("Stretch, tile, repeat texture")]
    public LineTextureMode trailTexType;


    private void Awake()
    {
        s_instance = this;
    }

    // Use this for initialization
    public void ReceiveSubCells (List<Subcell> cells) {
        allCells = cells;

        StartCoroutine("Creation");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Creation() {

        GameObject trail = Instantiate(baseTrailObj, transform.position, Quaternion.identity);
        trail.name = "IM A DEBUG DELETE ME";
        trail.GetComponent<Renderer>().sharedMaterial = baseTrailMaterial;
        trail.AddComponent<TrailRenderer>();
        trail.GetComponent<TrailRenderer>().time = Instance.trailDuration;
        trail.GetComponent<TrailRenderer>().sharedMaterial = Instance.trailMat;
        trail.GetComponent<TrailRenderer>().widthMultiplier = Instance.trailWidth;
        trail.GetComponent<TrailRenderer>().textureMode = Instance.trailTexType;

        trail.AddComponent<ArcBetweenTwo>();
        trail.GetComponent<ArcBetweenTwo>().ReceivePoints(allCells[0], allCells[1]);

        par = new GameObject();
        par.name = "Connections";
        for (int i = 0; i < allCells.Count; i++)
        {
            int b = i;
            b++;

            //for (int w = 0; w < MaximumNumberOfConnections; w++)
            //{
                GameObject line = new GameObject();
            line.transform.parent = par.transform;
                line.AddComponent<LineRenderer>();
                line.transform.position = allCells[i].transform.position;

                line.GetComponent<LineRenderer>().material = StartingMaterial;
                line.GetComponent<LineRenderer>().startWidth = StartingWidth;
                line.GetComponent<LineRenderer>().endWidth = EndingWidth;
                line.GetComponent<LineRenderer>().positionCount = (MaximumNumberOfConnections-1);
                ConnectionTracker tracker = line.AddComponent<ConnectionTracker>();


                // here you should get the distance?

                // below is demo code only

                if (b >= allCells.Count)
                {
                    b = 0;
                }
                tracker.originCell = allCells[i];
                tracker.trackCell = allCells[i];
                //tracker.secondClosestCell = allCells[i];
                //tracker.thirdClosestCell = allCells[i];
                //tracker.fourthClosestCell = allCells[i];

                if (ShouldWeDoBasicAnimation)
                {
                    LavaMoving basicAnimator = line.gameObject.AddComponent<LavaMoving>();
                    basicAnimator.uvAnimationRate = BasicAnimationDirections;
                }
                yield return new WaitForSeconds(0.02f);
            // }
        }

  

    }


}
