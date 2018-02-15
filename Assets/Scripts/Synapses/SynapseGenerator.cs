using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	const string glyphs= "abcdefghijklmnopqrstuvwxyz";
	int charAmount;

    private void Awake()
    {
        s_instance = this;
    }

    // Use this for initialization
    public void GenerateSynapses (Vector3 position, GameObject bound) {

        charAmount = Random.Range(5,10); // get individual names for each paths (we have to do this)
        BoundingSphere = bound;

		for (int i = 0; i < NumberOfSynapsesToGenerate; i++) {
			GameObject currentSynapse = Instantiate (SynapsePrefab, position, Quaternion.identity) as GameObject;
			pathData = currentSynapse.GetComponent<iTweenPath> ();

			pathData.nodeCount = Random.Range (minimumNumberOfNodes, maximumNumberOfNodes);

			pathCollections.Add (pathData);
			}

		foreach (iTweenPath path in pathCollections)
		{
			AssignNodes (path);
			// Cleanup (path);
			StartPath(path);
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

	void AssignNodes (iTweenPath p )
	{
		for (int n = 0; n < p.nodeCount; n++) {

            Vector3 veryStartingPoint = BoundingSphere.GetComponent<SphereCollider>().bounds.center;

			p.nodes [n] = veryStartingPoint;

			if (n != 0) {
				
				Vector3 dist = (GetRandomDirection () * GetRandomDistance ());

				p.nodes [n] += dist;

				if (!BoundingSphere.GetComponent<Renderer>().bounds.Contains (p.nodes [n])) {
					// ???
					p.nodes [n] = veryStartingPoint;
					p.nodes [n] = Random.insideUnitSphere * GetRandomDistance ();
				}
			}

		}

		if (shouldIEndWhereIBegin == true) {
			p.nodes [(p.nodeCount-1)] = p.nodes [0];
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
}
