using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Subcell : MonoBehaviour, IPointerClickHandler
{
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

	public Rigidbody RigidBody {get{ return m_rigidBody; }}
	private Rigidbody m_rigidBody;

	public ServiceData ServiceDat {get{ return m_serviceData; }}
	private ServiceData m_serviceData;


	//[SerializeField]
	//private Mesh[] m_cellMeshes;
	//private Dictionary<string, Mesh> m_meshDict = new Dictionary<string, Mesh> ();

	//private Mesh m_sharedMsh;
	//private SkinnedMeshRenderer m_meshRend;

	private float m_doubleClickInterval = 1f;
	private float m_lastClickTime = 0f;

	//private float m_morphInterval = 0.5f;

	private Vector3 m_spinVector;
	private float m_spinSpeed = 2f;

	private void Awake()
	{
		m_rigidBody = GetComponent<Rigidbody> ();
	}

	public void Initialise(ServiceData a_data)
	{
		Color col = Color.white;
		m_serviceData = a_data;
		switch (m_serviceData.ServiceName)
		{
			case "AGILE":
				m_serviceType = ServiceType.AGILE;
				col = Color.magenta;
				break;

			case "CONTENT":
				m_serviceType = ServiceType.CONTENT;
				col = Color.yellow;
				break;

			case "DATA":
				m_serviceType = ServiceType.DATA;
				col = Color.grey;
				break;

			case "FAST":
				m_serviceType = ServiceType.FAST;
				col = Color.blue;
				break;

			case "GROWTH":
				m_serviceType = ServiceType.GROWTH;
				col = Color.green;
				break;

			case "LIFE":
				m_serviceType = ServiceType.LIFE;
				ColorUtility.TryParseHtmlString ("#800080", out col);
				break;

			case "LOOP":
				m_serviceType = ServiceType.LOOP;
				col = Color.cyan;
				break;

			case "SHOP":
				m_serviceType = ServiceType.SHOP;
				col = Color.red;
				break;
		}
		gameObject.transform.localScale = Vector3.one * m_serviceData.ServiceWeighting;
		GetComponent<MeshRenderer> ().material.color = col;
	}
	
	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if ((Time.fixedTime - m_lastClickTime) < m_doubleClickInterval) 
		{
			Debug.Log (name + " Game Object Double Clicked!");
            CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.FocusedSubCellPhase);
            CameraInputManager.Instance.FocusOnSubCell(this);
			//MorphScale ();
		} 
		else 
		{
			Debug.Log(name + " Game Object Clicked!");
			ModelManager.Instance.HighlightSubcell (this);
			//MorphScaleReset ();
		}
		m_lastClickTime = Time.fixedTime;
	}
	
	/*
	private void Start()
	{
		//StartCoroutine ("MorphLoop");



		m_meshRend = GetComponent<SkinnedMeshRenderer> ();
		m_sharedMsh = m_meshRend.sharedMesh;

		CombineInstance[] combine = new CombineInstance[m_cellMeshes.Length];
		for (int i = 0; i < m_cellMeshes.Length; i++)
		{
			combine [i].mesh = m_cellMeshes [i];
		}
		//GetComponent<MeshFilter> ().mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh.CombineMeshes (combine);


		Debug.Log ("<color=#00ff00>blendShapeCount = " + m_meshRend.sharedMesh.blendShapeCount.ToString() + "</color>");
		for (int i = 0; i < m_meshRend.sharedMesh.blendShapeCount; i++)
		{

		}
	}
	*/


	/*
	private IEnumerator MorphLoop()
	{
		while (true)
		{
			Vector3 nextScale = new Vector3 (Random.Range (0.8f, 1.3f), Random.Range (0.8f, 1.3f), Random.Range (0.8f, 1.3f));
			iTween.ScaleTo (gameObject, iTween.Hash ("scale",nextScale,"time",m_morphInterval,"easetype",iTween.EaseType.spring,"isLocal",true));
			yield return new WaitForSeconds (m_morphInterval);
			iTween.ScaleTo (gameObject, iTween.Hash ("scale",Vector3.one,"time",m_morphInterval,"easetype",iTween.EaseType.easeInOutQuad,"isLocal",true));
			yield return new WaitForSeconds (m_morphInterval);
		}
	}
	*/

	/*
	private void MorphScale()
	{
		Vector3 nextScale = new Vector3 (Random.Range (0.8f, 1.3f), Random.Range (0.8f, 1.3f), Random.Range (0.8f, 1.3f));
		iTween.ScaleTo (gameObject, iTween.Hash ("scale",nextScale,"time",2f,"isLocal",true));
	}

	private void MorphScaleReset()
	{
		iTween.ScaleTo (gameObject, iTween.Hash ("scale",Vector3.one,"time",2f,"isLocal",true));
	}
	*/



}
