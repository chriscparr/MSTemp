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
	
	private string[] m_touchErrorCodes = 
	{
		"User authenticated successfully",
		"Authentication Failed",
		"User pressed the Cancel button",
		"User pressed \"Enter Password\"",
		"Touch ID is not configured",
		"Can not evaluate Touch ID",
		"In the editor doughnut!"
	};

	public void RequestTouchAuth()
	{
		#if UNITY_EDITOR
		OnTouchAuthResponse("6");
		#elif UNITY_IOS
		TouchID(this.gameObject.name);
		#endif
	}

	public void OnTouchAuthResponse(string a_messageIndex)
	{
		int indx;
		if (!int.TryParse (a_messageIndex, out indx))
		{
			if (OnTouchResult != null)
			{
				OnTouchResult (false, "Error parsing message code \"" + a_messageIndex + "\"");
			}
		}
		else
		{
			bool successful = indx == 0;			
			Debug.Log ("OnTouchAuthResponse - msg code: " + a_messageIndex.ToString() + " message: " + m_touchErrorCodes[indx]);			
			if (OnTouchResult != null)
			{
				OnTouchResult (successful, m_touchErrorCodes[indx]);
			}
		}
	}

}
