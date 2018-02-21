using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMover : MonoBehaviour {

	public Vector3 desiredPosition;
	public float speed;
	public iTween.EaseType easeMethod;
    // you could probably cache the positions in Start, like you do in OnRailsMovement.cs
    // or just make a manager for it or something like that?

    // Use this for initialization
    public void TweenToPosition(Vector3 pos, float speed, bool orientToPath = true, iTween.EaseType easeMethod = iTween.EaseType.easeInOutSine)
    {
        Debug.Log("WE TWEEENIN BOIS" + this.gameObject.name + " AT TGHE SPEED OF " + speed);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", pos, "time", speed, "easetype", easeMethod, "orienttopath", orientToPath));
    }


	public void TweenToPosition(Vector3 pos, float a_time, GameObject a_callbackObject, string a_callbackMethodName)
	{
		bool orientToPath = true;
		iTween.EaseType easeMethod = iTween.EaseType.easeInOutSine;
		iTween.MoveTo(this.gameObject, iTween.Hash("position", pos, "time", a_time, "easetype", easeMethod, "orienttopath", orientToPath, "oncompletetarget", a_callbackObject, "oncomplete", a_callbackMethodName));
	}
}
