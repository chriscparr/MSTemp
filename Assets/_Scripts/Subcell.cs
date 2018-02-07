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
		switch (a_data.ServiceName)
		{
			case "AGILE":
				m_serviceType = ServiceType.AGILE;
				break;

			case "CONTENT":
				m_serviceType = ServiceType.CONTENT;
				break;

			case "DATA":
				m_serviceType = ServiceType.DATA;
				break;

			case "FAST":
				m_serviceType = ServiceType.FAST;
				break;

			case "GROWTH":
				m_serviceType = ServiceType.GROWTH;
				break;

			case "LIFE":
				m_serviceType = ServiceType.LIFE;
				break;

			case "LOOP":
				m_serviceType = ServiceType.LOOP;
				break;

			case "SHOP":
				m_serviceType = ServiceType.SHOP;
				break;
		}
		gameObject.transform.localScale = Vector3.one * a_data.ServiceWeighting;
	}
	
	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if ((Time.fixedTime - m_lastClickTime) < m_doubleClickInterval) 
		{
			Debug.Log (name + " Game Object Double Clicked!");
			//MorphScale ();
		} 
		else 
		{
			Debug.Log(name + " Game Object Clicked!");
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
