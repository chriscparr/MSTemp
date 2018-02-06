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

		m_spinVector = new Vector3 (Random.Range (-1f, 1f) * m_spinSpeed, Random.Range (-1f, 1f) * m_spinSpeed, Random.Range (-1f, 1f) * m_spinSpeed);
		/*
		foreach (Mesh msh in m_cellMeshes)
		{
			m_meshDict.Add (msh.name, msh);
		}
		*/
	}

	private void Start()
	{
		m_rigidBody.AddRelativeForce (m_spinVector, ForceMode.Impulse);
	}

	public void InitAsType(ServiceType a_type)
	{
		MeshFilter m_filt = GetComponent<MeshFilter> ();
		switch (a_type) 
		{
			case ServiceType.AGILE:
				//m_filt.mesh = m_agileMesh;
				break;
				
			case ServiceType.CONTENT:
				
				break;
				
			case ServiceType.DATA:
				
				break;
				
			case ServiceType.FAST:
				
				break;
				
			case ServiceType.GROWTH:
				
				break;
				
			case ServiceType.LIFE:
				
				break;
				
			case ServiceType.LOOP:
				
				break;
				
			case ServiceType.SHOP:
				
				break;
				
			default:
				
				break;
		}
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
