﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_mainContainerPrefab;
	[SerializeField]
	private GameObject m_subcellPrefab;
	[SerializeField]
	private GameObject m_modelRoot;

	public static ModelManager Instance {get { return s_instance;}}
	private static ModelManager s_instance = null;

	private GameObject m_mainContainer;
	private List<Subcell> m_subcells = new List<Subcell>();

	private Vector3[] m_placementVectors = new Vector3[] { 
		new Vector3(-1f, 1f, -1f),
		new Vector3(-1f, 1f, 1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(1f, 1f, 1f),
		new Vector3(-1f, -1f, -1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(1f, -1f, 1f)
	};



	private void Awake()
	{
		s_instance = this;
	}

	private void Start()
	{
		InitialiseModel (GenerateDebugPresentation ());
	}


	public void InitialiseModel(PresentationData a_pData)
	{
		m_mainContainer = Instantiate<GameObject> (m_mainContainerPrefab, m_modelRoot.transform);
		Vector3 minBounds = m_mainContainer.GetComponent<MeshCollider> ().bounds.min;
		minBounds.Scale (new Vector3 (0.3f, 0.3f, 0.3f));
		for (int i = 0; i < m_placementVectors.Length; i++)
		{
			GameObject sub = Instantiate<GameObject> (m_subcellPrefab, m_modelRoot.transform);
			m_subcells.Add (sub.GetComponent<Subcell> ());
			sub.transform.localPosition = Vector3.Scale (minBounds, m_placementVectors [i]);


		}
	}


	private void ClearModel()
	{
		foreach (Subcell cell in m_subcells)
		{
			Destroy (cell.gameObject);
		}
		m_subcells.Clear ();
		Destroy (m_mainContainer);
	}

	private PresentationData GenerateDebugPresentation()
	{
		string testData = "{\n\"PresenterName\":\"Mr. Tester\",\n\"PresenterPosition\":\"Test Manager\",\n\"ClientName\":\"IndustryLeader Inc.\",\n\"Industries\":[\"Finance\", \"Retail\", \"World Domination\"],\n\"Markets\":[\"Europe\", \"Asia\", \"America\"],\n\"Notes\":[\"Note 1\", \"Note 2\", \"Note 3\"],\n\"Services\":\n[\n{\"ServiceName\":\"FAST\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"SHOP\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"GROWTH\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"DATA\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"LOOP\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"CONTENT\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"AGILE\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"LIFE\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}\n]\n}";
		PresentationData pData = JsonUtility.FromJson<PresentationData> (testData);
		return pData;
	}


	private void Update()
	{
		if (Input.GetKeyDown (KeyCode.Q))
		{
			InitialiseModel (GenerateDebugPresentation ());
		}
		if (Input.GetKeyDown (KeyCode.W))
		{
			ClearModel ();
		}
	}
}
