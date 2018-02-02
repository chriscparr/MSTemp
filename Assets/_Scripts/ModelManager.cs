using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour {

	public static ModelManager Instance {get { return s_instance;}}
	private static ModelManager s_instance = null;

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
