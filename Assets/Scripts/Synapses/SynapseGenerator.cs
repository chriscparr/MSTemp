using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SynapseGenerator : MonoBehaviour {

    //TODO A LOT OF THIS IS OUTDATED NOW, SO DO WELL TO IGNORE ME.

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
            int firstSubcellCount = 0;
            int secondSubcellCount = 1;

            foreach (Subcell s in allCells)
            {
                MakeASynapseThatPingPongsBetweenTwoSubCells(s.transform.position, s.gameObject, allCells[secondSubcellCount], s);

                firstSubcellCount++;
                secondSubcellCount++;
            }
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
                    Debug.Log("START BLOB = " + allCells[a].name + " / connected TO " + allCells[b].name);

            }

            for (int i = 0; i < allSynapses.Count(); i++)
            {
                allSynapses[i].GetComponent<SynapseMover>().startcell = allCells[Random.Range(0, allCells.Count)];
                allSynapses[i].GetComponent<SynapseMover>().endcell = allCells[Random.Range(0, allCells.Count)];

               
            }
        }
    }

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
            charAmount = Random.Range(5, 10);
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
                    StartPath(path);
                }
            }
        }
    }
	
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
