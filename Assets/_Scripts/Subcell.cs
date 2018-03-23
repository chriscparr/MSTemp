using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Subcell : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private TextMesh m_labelText;
	[SerializeField]
	private GameObject m_caseCellPrefab;
	[SerializeField]
	private Material m_offMaterial;
	[SerializeField]
	private Material m_onMaterial;

	public Rigidbody RigidBody {get{ return m_rigidBody; }}
	private Rigidbody m_rigidBody;

	public ServiceData ServiceDat {get{ return m_serviceData; }}
	private ServiceData m_serviceData;

	public CaseCell[] CaseCells {get{ return m_caseCells; }}
	private CaseCell[] m_caseCells;

	private float m_doubleClickInterval = 0.25f;
	private int m_clickCount = 0;
	private bool m_isLabelActive = false;

	public bool CanScale { get; set;}

	private bool m_hasReversedMesh = false;
	private bool m_isUsingOnMaterial = true;

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
		gameObject.GetComponent<Renderer> ().material = m_onMaterial;
		CreateReversedMesh ();
		GenerateCaseCells ();
	}

	public void ToggleMaterials(bool a_useOnMaterial)
	{
		m_isUsingOnMaterial = !a_useOnMaterial;	//prepare for switch
		ToggleMaterials ();
	}

	public void ToggleMaterials()
	{
		if (m_isUsingOnMaterial)
		{
			gameObject.GetComponent<Renderer> ().material = m_offMaterial;
		}
		else
		{
			gameObject.GetComponent<Renderer> ().material = m_onMaterial;
		}
		m_isUsingOnMaterial = !m_isUsingOnMaterial;
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
			Destroy(clone.GetComponent<Rigidbody>());
			Destroy(clone.GetComponent<SphereCollider>());
			Destroy(clone.GetComponent<Subcell>());
			clone.AddComponent<ReverseNormals>();
			clone.GetComponent<Renderer> ().material = m_onMaterial;
            clone.layer = 8; // IGNORE D.O.F
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

	public Subcell[] GetNearestNeighbours()
	{
		Dictionary<float, Subcell> cellDistances = new Dictionary<float, Subcell> ();
		List<Subcell> cells = new List<Subcell> (ModelManager.Instance.GetAllSubCells ());
		cells.Remove (this);
		foreach (Subcell c in cells)
		{
			float dist = Vector3.Distance (transform.position, c.transform.position);
			cellDistances.Add (dist, c);
			//Debug.Log ("Adding " + c.ServiceDat.ServiceName + ", distance = " + dist.ToString());
		}
		SortedList sorted = new SortedList (cellDistances);
		//Add sorted subcells back into the list so that we can return it .ToArray()
		cells.Clear ();
		for (int i = 0; i < sorted.Count; i++)
		{
			cells.Add ((Subcell)sorted.GetByIndex (i));
		}
		/*
		for (int i = 0; i < cells.Count; i++)
		{
			Debug.Log ("List index " + i.ToString() + ", subcell name: " + cells[i].ServiceDat.ServiceName);
		}
		*/
		return cells.ToArray ();
	}
}
