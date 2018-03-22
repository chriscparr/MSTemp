using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class BiometricTouch : MonoBehaviour 
{
	public delegate void BiometricTouchAction(bool success, string message);
	public BiometricTouchAction OnTouchResult;

	[DllImport("__Internal")]
	private static extern void TouchID(string gameObjectName);

	public void RequestTouchAuth()
	{
		#if UNITY_EDITOR
		OnTouchAuthResponse("In the editor doughnut!");
		#elif UNITY_IOS
		TouchID(this.gameObject.name);
		#endif
	}

	public void OnTouchAuthResponse(string message)
	{
		Debug.Log ("OnTouchAuthResponse - " + message);
		if (OnTouchResult != null)
		{
			OnTouchResult (true, message);
		}
	}

}
