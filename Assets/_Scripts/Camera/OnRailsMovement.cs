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

	private float ourNodeCount = 1;
	int i = 0;
	// Use this for initialization
	void Awake() {
		iTween.Init (this.gameObject);
	}

	// IMPORTANT NOTE:
	// ONLY USE THIS SCRIPT IF THE CASE STUDY VIDEOS INSIDE THE SUBCELLS ARE GOING TO MOVE (THEY PROBABLY WONT)

	void Start () {
		thisPath = GetComponent<iTweenPath> ();
		// after we have created our path positions (aka, locations of the case studies)

		GameObject[] c = GameObject.FindGameObjectsWithTag ("CaseStudy"); // do this a different way later
		foreach (GameObject g in c) {
			caseStudyPositions.Add (g.transform.position);
		}

		arr = caseStudyPositions.ToArray();

		increment = 0;

		thisPath.nodeCount = caseStudyPositions.Count;
		thisPath.nodes = caseStudyPositions;

		distanceBetweenPositions = (float)(1f / (float)thisPath.nodeCount);
		marginForError = (float)(distanceBetweenPositions / (float)(thisPath.nodeCount * 2));
		destinationAsIncrement = (0 + distanceBetweenPositions);
		Debug.LogError (destinationAsIncrement);

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			RailMover rm = GetComponent<RailMover> ();
			rm.TweenToPosition (arr [i], 6);
			i++;
		}


		if (allowMove) {
			Debug.Log (increment);
			increment = Mathf.SmoothDamp (originAsIncrement, destinationAsIncrement, ref maxTime, speed);



			if (increment >= (destinationAsIncrement - marginForError) && increment > 0) {
				increment = (destinationAsIncrement - marginForError);
				AssignIncrements (increment, (increment + distanceBetweenPositions), true);
				allowMove = false;
			}
		}

		if (increment >= 1) {
			increment = 1;
			allowMove = false;
		}

	}



	public void AssignIncrements(float orig, float dest, bool wait)
	{

		Debug.LogError (orig + " , " + dest);
		allowMove = false;
		originAsIncrement = orig;
		destinationAsIncrement = dest;
		increment = orig;

		if (destinationAsIncrement >= 1) {
			destinationAsIncrement = 1;
		}
		if (wait) {
			StartCoroutine ("Wait");
		} else {
			allowMove = true;
			Debug.Log ("INCREMENT = " + increment);
		}
	}

	IEnumerator Wait() {
		yield return new WaitForSeconds (2);

	

		allowMove = true;
	}
}
