using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour 
{
	public static UIManager Instance {get { return s_instance;}}
	private static UIManager s_instance = null;

	private void Awake()
	{
		s_instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
