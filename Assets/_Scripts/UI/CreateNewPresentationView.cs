﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewPresentationView : MonoBehaviour 
{

	[SerializeField]
	private GameObject m_buttonScrollPrefab;
	[SerializeField]
	private GameObject m_msToggleBoxPrefab;



	private PresentationData m_presentationData;

	public void SetupView(PresentationData a_pData = null)
	{
		if (a_pData != null)
		{
			m_presentationData = a_pData;
		} 
		else
		{
			m_presentationData = new PresentationData ();
		}

	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
