using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynapseMover : MonoBehaviour {
	bool allowMove;
	float val = 0;
	float velocity = 1;
	Vector3[] arr;

	public float maxTime = 30;

	// Use this for initialization
	public void Tweener(iTweenPath p) {
		arr = p.nodes.ToArray ();
		allowMove = true;
		// iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath(p.pathName), "time", (Speed), "orientToPath", true, "looktime", 2f, "oncomplete", "Reset", "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.easeInOutCubic));
	}

	void Awake() {
		iTween.Init (this.gameObject);
	}

	void Update() {
		if (allowMove)
			val = Mathf.SmoothStep(0, 1, Mathf.PingPong(Time.time/maxTime, 1));
        Debug.Log("SYNAPSE MOVING");
		iTween.PutOnPath (this.gameObject, arr, val);

		if (this.val >= 0.99f) {
			Debug.Log ("DISABLE THE MOVER");
			this.enabled = false;
		}

	
	}
}
