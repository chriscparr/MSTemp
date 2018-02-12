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

	private float m_doubleClickInterval = 0.25f;
	private int m_clickCount = 0;

	private void Awake()
	{
		m_rigidBody = GetComponent<Rigidbody> ();
        this.gameObject.AddComponent<RailMover>();
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
        transform.parent = null;

        CreateReversedMesh();
	}

    void CreateReversedMesh()
    {
        GameObject clone = Instantiate(this.gameObject, transform.position, Quaternion.identity) as GameObject;
        clone.transform.parent = this.transform;
        clone.AddComponent<ReverseNormals>();

        // messy
        Destroy(clone.GetComponent<Rigidbody>());
        Destroy(clone.GetComponent<SphereCollider>());
        Destroy(clone.GetComponent<Subcell>());
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
                CameraInputManager.Phase curPhase = CameraInputManager.Instance.m_CurrentPhase;
                switch (curPhase) {
                    case CameraInputManager.Phase.MainCellPhase:
                        CameraInputManager.Instance.FocusOnSubCell(this);
                        CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.FocusedSubCellPhase);

                        break;
                    case CameraInputManager.Phase.FocusedSubCellPhase:
                        
                        CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.InsideSubCellPhase);
                        break;
                    default:
                        break;
                }
				
				break;
			default:
				//do nothing...
				break;
		}
		m_clickCount = 0;
		yield return null;
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
