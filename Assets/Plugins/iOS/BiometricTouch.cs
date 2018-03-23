using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class BiometricTouch : MonoBehaviour 
{
	public enum TouchIDResult
	{
		SUCCESS,
		AUTH_FAIL,
		CANCEL_PRESSED,
		ENTER_PW_PRESSED,
		NOT_CONFIGURED,
		CANT_EVALUATE,
		IN_EDITOR
	}

	public delegate void BiometricTouchAction(TouchIDResult a_result);
	public BiometricTouchAction OnTouchResult;

	[DllImport("__Internal")]
	private static extern void TouchID();

	public void RequestTouchAuth()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			TouchID ();
		}
		else
		{
			OnTouchAuthResponse("6");
		}
	}

	public void OnTouchAuthResponse(string a_messageIndex)
	{
		TouchIDResult outputResult;
		switch (a_messageIndex)
		{
			case "0":
				outputResult = TouchIDResult.SUCCESS;
				break;
			case "1":
				outputResult = TouchIDResult.AUTH_FAIL;
				break;
			case "2":
				outputResult = TouchIDResult.CANCEL_PRESSED;
				break;
			case "3":
				outputResult = TouchIDResult.ENTER_PW_PRESSED;
				break;
			case "4":
				outputResult = TouchIDResult.NOT_CONFIGURED;
				break;
			case "5":
				outputResult = TouchIDResult.CANT_EVALUATE;
				break;
			case "6":
				outputResult = TouchIDResult.IN_EDITOR;
				break;
			default:
				throw new System.Exception ("Error parsing message code \"" + a_messageIndex + "\"");
		}

		if (OnTouchResult != null)
		{
			OnTouchResult (outputResult);
		}
	}
}
