using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestManager : MonoBehaviour 
{
	[SerializeField]
	private Canvas m_uiCanvas;
	[SerializeField]
	private ScrollingButtonSelect m_scrollSelect;
	[SerializeField]
	private string[] m_testOptions;

	// Use this for initialization
	void Start () 
	{
		//m_scrollSelect.Initialise (m_testOptions);
	}

	private void LogSelectedOptions()
	{
		string selectedOpts = "Selected options: ";
		foreach (string sel in m_scrollSelect.SelectedOptions)
		{
			selectedOpts += sel + ", ";
		}
		Debug.Log ("<color=#0000ff>" + selectedOpts + "</color>");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Q))
		{
			LogSelectedOptions ();
		}
	}
}
