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

	public CaseCell[] CaseCells {get{ return m_caseCells; }}

	private float m_doubleClickInterval = 0.25f;
	private int m_clickCount = 0;
	private bool m_isLabelActive = false;

	private CaseCell[] m_caseCells;

	public bool CanScale { get; set;}
    [HideInInspector]
    public Material myOnMaterial;

	private bool m_hasReversedMesh = false;

	private void Awake()
	{
		m_rigidBody = GetComponent<Rigidbody> ();
		m_labelText.gameObject.transform.localScale = Vector3.one * 0.5f;
	}

	public void Initialise(ServiceData a_data)
	{
		m_serviceData = a_data;

		gameObject.transform.localScale = Vector3.one * m_serviceData.InitialScale;
		m_labelText.text = m_serviceData.ServiceName.ToUpper ();
		m_labelText.gameObject.SetActive (false);
		gameObject.AddComponent<RailMover>();
		CreateReversedMesh ();
		GenerateCaseCells ();
	}

	public void ToggleLabelText(bool a_isActive)
	{
		m_isLabelActive = a_isActive;
		m_labelText.gameObject.SetActive (a_isActive);
	}

	public void CreateReversedMesh()
    {
		if (!m_hasReversedMesh)
		{
            m_hasReversedMesh = true;
			GameObject clone = Instantiate(this.gameObject, transform.position, Quaternion.identity) as GameObject;        
			Destroy (clone.transform.GetChild(0).gameObject);
			clone.transform.parent = this.transform;
			clone.GetComponent<Renderer> ().sharedMaterial = myOnMaterial;
			Destroy(clone.GetComponent<Rigidbody>());
			Destroy(clone.GetComponent<SphereCollider>());
			Destroy(clone.GetComponent<Subcell>());
            clone.layer = 8; // IGNORE D.O.F
			clone.AddComponent<ReverseNormals>();
		}
    }

	public void GenerateCaseCells()
	{
		m_caseCells = new CaseCell[m_serviceData.CaseStudies.Length];
		for (int i = 0; i < m_caseCells.Length; i++)
		{
			GameObject caseCellObj = Instantiate<GameObject>(m_caseCellPrefab, gameObject.transform);
			caseCellObj.transform.localPosition = Random.insideUnitSphere * 0.5f;
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
					Debug.Log("Entering " + ServiceDat.ServiceName);
					CameraInputManager.Instance.EnterSelectedSubCell (this);
				}
				break;
			default:
				//do nothing...
				break;
		}
		m_clickCount = 0;
		yield return null;
	}

	private void Update()
	{
		if (m_isLabelActive)
		{
			m_labelText.gameObject.transform.SetPositionAndRotation (gameObject.transform.position + new Vector3 (0f, -3f, 0f), Quaternion.Inverse (Camera.main.transform.rotation));
		}
	}

}
