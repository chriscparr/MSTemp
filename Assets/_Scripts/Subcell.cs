﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Subcell : MonoBehaviour, IPointerClickHandler
{
	/*
	public enum ServiceType
	{
		FAST,
		SHOP,		//SHOP +
		GROWTH,
		DATA,		//DATA PLATFORMS AND SERVICES
		LOOP,
		CONTENT,	//CONTENT +
		AGILE,		//PLAN FOR AGILE
		LIFE		//LIFE +
	}
	private ServiceType m_serviceType;
	*/

	[SerializeField]
	private TextMesh m_labelText;
	[SerializeField]
	private GameObject m_caseCellPrefab;

	public Rigidbody RigidBody {get{ return m_rigidBody; }}
	private Rigidbody m_rigidBody;

	public ServiceData ServiceDat {get{ return m_serviceData; }}
	private ServiceData m_serviceData;

	public Vector3[] CellPositions {get{ return m_caseCellPositions; }}
	private Vector3[] m_caseCellPositions;


	private float m_doubleClickInterval = 0.25f;
	private int m_clickCount = 0;

	private CaseCell[] m_caseCells;
	private int numberOfStudiesInService = 3;	//debug value, should come from serviceData in future!

	private void Awake()
	{
		m_rigidBody = GetComponent<Rigidbody> ();
	}

	public void Initialise(ServiceData a_data)
	{
		m_serviceData = a_data;
		switch (m_serviceData.ServiceName)
		{
			case "AGILE":
				break;
			case "CONTENT":
				break;
			case "DATA":
				break;
			case "FAST":
				numberOfStudiesInService = VideoManager.Instance.m_FastVideos.Count+1;
				break;
			case "GROWTH":
				break;
			case "LIFE":
				break;
			case "LOOP":
				break;
			case "SHOP":
				break;
			default:
				break;
		}
		gameObject.transform.localScale = Vector3.one * m_serviceData.InitialScale;
		m_labelText.text = m_serviceData.ServiceName.ToLowerInvariant ();
		m_labelText.gameObject.SetActive (false);
		gameObject.AddComponent<RailMover>();
		CreateReversedMesh();
		GenerateCaseCells ();
	}

	public void ToggleLabelText(bool a_isActive)
	{
		m_labelText.gameObject.SetActive (a_isActive);
	}

    private void CreateReversedMesh()
    {
        GameObject clone = Instantiate(this.gameObject, transform.position, Quaternion.identity) as GameObject;        
		Destroy (clone.transform.GetChild(0).gameObject);
		clone.transform.parent = this.transform;
		Destroy(clone.GetComponent<Rigidbody>());
		Destroy(clone.GetComponent<SphereCollider>());
		Destroy(clone.GetComponent<Subcell>());
        clone.AddComponent<ReverseNormals>();
    }

	private void GenerateCaseCells()
	{
		m_caseCellPositions = new Vector3[m_serviceData.CaseStudies.Length];
		m_caseCells = new CaseCell[m_serviceData.CaseStudies.Length];
		for (int i = 0; i < m_caseCellPositions.Length; i++)
		{
			GameObject caseCellObj = Instantiate<GameObject>(m_caseCellPrefab, gameObject.transform);
			caseCellObj.transform.localPosition = Random.insideUnitSphere * 0.5f;
			m_caseCellPositions [i] = caseCellObj.transform.position;
			m_caseCells [i] = caseCellObj.GetComponent<CaseCell> ();
			m_caseCells [i].Initialise (this, m_serviceData.CaseStudies [i]);
		}
	}

    public void OnCollisionEnter(Collision collision)
    {
		if (CameraInputManager.Instance.m_CurrentPhase == CameraInputManager.Phase.MainCellPhase)
		{
			ModelManager.Instance.OnSubcellCollision(this.gameObject, collision.gameObject);
		}
    }

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (m_clickCount == 0)
		{
			StartCoroutine ("ProcessClicks");
		}
		m_clickCount++;
	}

	private IEnumerator ProcessClicks()
	{
		yield return new WaitForSecondsRealtime (m_doubleClickInterval);
		switch (m_clickCount)
		{
			case 1:
				Debug.Log(name + " Game Object Single Clicked!");
				ModelManager.Instance.HighlightSubcell (this);
				break;
			case 2:
				Debug.Log (name + " Game Object Double Clicked!");
				if (CameraInputManager.Instance.m_CurrentPhase == CameraInputManager.Phase.MainCellPhase)
				{
					CameraInputManager.Instance.FocusOnSubCell (this);
					CameraInputManager.Instance.SetPhase (CameraInputManager.Phase.FocusedSubCellPhase);
				}
				else
				if (CameraInputManager.Instance.m_CurrentPhase == CameraInputManager.Phase.FocusedSubCellPhase)
				{
						Debug.Log(ServiceDat.ServiceName);
						CameraInputManager.Instance.EnterSubCell(numberOfStudiesInService);
						CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.InsideSubCellPhase);
				}
				break;
			default:
				//do nothing...
				break;
		}
		m_clickCount = 0;
		yield return null;
	}

}
