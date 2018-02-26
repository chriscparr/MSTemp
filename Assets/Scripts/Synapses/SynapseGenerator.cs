using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SynapseGenerator : MonoBehaviour {


    public static SynapseGenerator Instance { get { return s_instance; } }
    private static SynapseGenerator s_instance = null;

	public GameObject SynapsePrefab;
	public int NumberOfSynapsesToGenerate = 3;
	public GameObject BoundingSphere; // this should be the clickable Subcell
	public float MinimumRadiusRandomisation = 2.5f;
	public float MaximumRadiusRandomisation = 5f;

	public float MinimumDistanceRandomisation = -10;
	public float MaximumDistanceRandomisation = 10;


	public bool shouldIEndWhereIBegin = true;

	public int minimumNumberOfNodes = 4;
	public int maximumNumberOfNodes = 10;

	iTweenPath pathData;
	List<iTweenPath> pathCollections = new List<iTweenPath> ();
    //
	const string glyphs= "abcdefghijklmnopqrstuvwxyz";
	int charAmount;

    public bool AreWeConnectingSubCells = true;
    public List<Subcell> allCells = new List<Subcell>();
    public List<GameObject> allSynapses = new List<GameObject>();

    public bool AreWeDoingLeoniesReferenceParticles = false;

    private void Awake()
    {
        s_instance = this;

 
    }

    public void ConnectSubCells(List<Subcell> cells)
    {
        allCells = cells;


        if (AreWeDoingLeoniesReferenceParticles == true)
            {
            // TODO TODO TODO TODO TODO TODO
            // simple shit mate, listen.
            // get 2 subcells (cycle through them so they all connect to another i.e. 1 to 2, 2 to 3, 8 to 0 etc.
            int firstSubcellCount = 0;
            int secondSubcellCount = 1;

            foreach (Subcell s in allCells)
            {
                MakeASynapseThatPingPongsBetweenTwoSubCells(s.transform.position, s.gameObject, allCells[secondSubcellCount], s);

                firstSubcellCount++;
                secondSubcellCount++;
            }

            // get a trail renderer, use the iTween path logic and update them at runtime depending on the start/end cell positions
            // uze the GetRandomDireciton and GetRandomDistance functions to create some bezier type curves
            // on Smoothstep if the val is achieved (aka 0 or 1, or 0.1 and 0.9), ping pong between the two (or reverse the array)
            // upon array reversal, re-randomize ALL POINTS except the last and the first (ALL POINTS except the start cell and end cell)
            // THIS ISNT TOO DIFFICULT, ETA 2-4 HOURS?  
            // USE A TRAIL RENDERER, AND OR A PARTICLE SYSTEM (THAT MIGHT BE NICER)... experiment long as it doesnt take too long please
            // TODO TODO TODO TODO TODO TODO
            // and for fuck sake get the early train in man come on, sleep is for the weak
        }



        if (AreWeConnectingSubCells == true)
        {
            for (int a = 0; a < allCells.Count; a++)
            {
                int b = a;
                b++;
                if (b >= allCells.Count)
                {
                    b = 0;
                }
                  
                    MakeOneSynapseThatConnectsToASubCell(allCells[a].transform.position, allCells[a].gameObject, allCells[b], allCells[a]);
                    // MakeOneSynapseThatConnectsToASubCell(allCells[a].transform.position, allCells[a].gameObject, allCells[b], allCells[a]);
                    Debug.Log("START BLOB = " + allCells[a].name + " / connected TO " + allCells[b].name);

                //else
                //{

                //    MakeOneSynapseThatConnectsToASubCell(allCells[a].transform.position, allCells[a].gameObject, allCells[a + 1], allCells[a]);
                //    // todo DECIDE WHETHER TO CALL THIS TWICE (FUCK PERFORMANCE FOR NOW)
                //    // TODO
                //    // I THINK ITS BETTER WITH AN EXTRA CALL BUT YOU KNOW, REASONS
                //    // TODO
                //    MakeOneSynapseThatConnectsToASubCell(allCells[a].transform.position, allCells[a].gameObject, allCells[a + 1], allCells[a]);
                //    Debug.Log("START BLOB = " + allCells[a].name + " / connected TO " + allCells[a+1].name);
                //}
                //GenerateSynapses(c.transform.position, c.gameObject, false);
            }

            for (int i = 0; i < allSynapses.Count(); i++)
            {
                allSynapses[i].GetComponent<SynapseMover>().startcell = allCells[Random.Range(0, allCells.Count)];
                allSynapses[i].GetComponent<SynapseMover>().endcell = allCells[Random.Range(0, allCells.Count)];

               
            }
        }
    }

    //public void MakeSynapse (Vector3 position, GameObject bound, bool overrideNormalFunctionality = false) {

    //    // charAmount = Random.Range(5,10); // get individual names for each paths (we have to do this)
    //    BoundingSphere = bound;

    //        GameObject currentSynapse = Instantiate (SynapsePrefab, position, Quaternion.identity) as GameObject;
    //        pathData = currentSynapse.GetComponent<iTweenPath> ();

    //        pathData.nodeCount = Random.Range (minimumNumberOfNodes, maximumNumberOfNodes);

    //        pathCollections.Add (pathData);
    //        allSynapses.Add(currentSynapse);
            

    //    foreach (iTweenPath path in pathCollections)
    //    {
    //        if (!overrideNormalFunctionality)
    //        {
    //            AssignNodes(path, true);
    //            // Cleanup (path);
    //            StartPath(path);
    //        }

  
    //    }
    //    Debug.LogError("ALL SYNAPSES GENERATED");
    //}

    public void MakeASynapseThatPingPongsBetweenTwoSubCells(Vector3 position, GameObject bound, Subcell endCell, Subcell startCell)
    {
        GameObject currentSynapse = Instantiate(SynapsePrefab, position, Quaternion.identity) as GameObject;
        pathData = currentSynapse.GetComponent<iTweenPath>();

        pathData.nodeCount = Random.Range(minimumNumberOfNodes, maximumNumberOfNodes);

        pathCollections.Add(pathData);
        allSynapses.Add(currentSynapse);
        BoundingSphere = bound;

        foreach (iTweenPath path in pathCollections)
        {

            AssignNodes(path, true, endCell);
            // Cleanup (path);
            StartPath(path);


        }

        foreach (GameObject path in allSynapses)
        {
            path.GetComponent<LineRenderer>().positionCount = path.GetComponent<iTweenPath>().nodeCount + 1;

            path.GetComponent<SynapseMover>().followEndCell = true;
            path.GetComponent<SynapseMover>().followStartCell = true;
        }


    }

    public void MakeOneSynapseThatConnectsToASubCell(Vector3 position, GameObject bound, Subcell endCell, Subcell startCell)
    {
        GameObject currentSynapse = Instantiate(SynapsePrefab, position, Quaternion.identity) as GameObject;
        pathData = currentSynapse.GetComponent<iTweenPath>();

        pathData.nodeCount = Random.Range(minimumNumberOfNodes, maximumNumberOfNodes);

        pathCollections.Add(pathData);
        allSynapses.Add(currentSynapse);
        BoundingSphere = bound;

        foreach (iTweenPath path in pathCollections)
        {
           
                AssignNodes(path, true, endCell);
                // Cleanup (path);
                StartPath(path);


        }

        foreach (GameObject path in allSynapses)
        {
            path.GetComponent<LineRenderer>().positionCount = path.GetComponent<iTweenPath>().nodeCount+1;
     
            path.GetComponent<SynapseMover>().followEndCell = true;
            path.GetComponent<SynapseMover>().followStartCell = true;

            //TODO COMMENT ME OUT TO REVERT BACK TO ORIGINAL
            path.GetComponent<SynapseMover>().trackPositionsRealTime = true;
        }

     
    }

    // Use this for initialization
    public void GenerateSynapses(Vector3 position, GameObject bound, bool overrideNormalFunctionality = false)
    {
        if (!overrideNormalFunctionality)
        {
            charAmount = Random.Range(5, 10); // get individual names for each paths (we have to do this)
            BoundingSphere = bound;

            for (int i = 0; i < NumberOfSynapsesToGenerate; i++)
            {
                GameObject currentSynapse = Instantiate(SynapsePrefab, position, Quaternion.identity) as GameObject;
                pathData = currentSynapse.GetComponent<iTweenPath>();

                pathData.nodeCount = Random.Range(minimumNumberOfNodes, maximumNumberOfNodes);

                pathCollections.Add(pathData);
                allSynapses.Add(currentSynapse);
            }

            foreach (iTweenPath path in pathCollections)
            {
                if (!overrideNormalFunctionality)
                {
                    AssignNodes(path, true);
                    // Cleanup (path);
                    StartPath(path);
                }
            }

            Debug.LogError("ALL SYNAPSES GENERATED");
        }
    }
	
	
	// Update is called once per frame
	Vector3 GetRandomDirection () {
		Vector3 direction = Random.insideUnitSphere * GetRandomDistance ();
		return direction;

	}

	float GetRandomDistance() {
		return Random.Range (MinimumDistanceRandomisation, MaximumDistanceRandomisation);
	}

    void AssignNodes(iTweenPath p, bool overRide = false, Subcell endCell = default(Subcell))
    {
        for (int c = 0; c < allCells.Count; c++)
        {
            for (int n = 0; n < p.nodeCount; n++)
            {

                Vector3 veryStartingPoint = BoundingSphere.transform.position;

                p.nodes[n] = veryStartingPoint;

                if (n != 0)
                {

                    Vector3 dist = (GetRandomDirection() * GetRandomDistance());

                    p.nodes[n] += dist;

                    if (!BoundingSphere.GetComponent<Renderer>().bounds.Contains(p.nodes[n]))
                    {
                        // ???
                        p.nodes[n] = veryStartingPoint;
                        p.nodes[n] = Random.insideUnitSphere * GetRandomDistance();
                    }
                }

            }

            if (shouldIEndWhereIBegin == true)
            {
                p.nodes[(p.nodeCount - 1)] = p.nodes[0];
            }

            if (overRide == true)
            {
                p.nodes[(p.nodeCount)] = endCell.transform.position;
            }
        }
    }
		

	void StartPath (iTweenPath p)
	{
		string s = "";

		for (int i = 0; i < charAmount; i++) {
			s += glyphs [Random.Range (0, glyphs.Length)]; 
		}
		p.pathName = s;
		Debug.Log (p.pathName);
		p.GetComponent<SynapseMover> ().Tweener (p);
	}

    public void DoReversal()
    {
        SynapseMover.reversed = true;

    }

    void Update()
    {
        if (AreWeConnectingSubCells == true && allCells != null)
        {
            //for (int n = 0; n < allSynapses.Count; n++)
            //{
            //    allSynapses[n].GetComponent<iTweenPath>().nodes[GetComponent<iTweenPath>().nodeCount] 
            //                  = allCells[Random.Range(0, allCells.Count)].transform.position;
            //}
        }
    }
}
