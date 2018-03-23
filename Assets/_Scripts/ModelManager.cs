using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour 
{	
	[SerializeField]
	private GameObject m_mainContainerPrefab;
	[SerializeField]
	private GameObject m_modelRoot;
	[SerializeField]
	private LightningBoltPooler m_boltPooler;
	[SerializeField]
	private	NamedPrefab[] m_modelPrefabDefs;

	private Dictionary<string, GameObject> m_modelPrefabs = new Dictionary<string, GameObject> ();

	public static ModelManager Instance {get { return s_instance;}}
	private static ModelManager s_instance = null;
    //
	private GameObject m_mainContainer;
	private List<Subcell> m_subcells = new List<Subcell>();

	public bool IsInitialised {get { return m_isInitialised; }}
	private bool m_isInitialised = false;

	public Subcell HighlightedSubcell {get { return m_highlightedSubcell; }}
	private Subcell m_highlightedSubcell;

	private Light m_highlight;
	private bool m_highlightActive = false;

	private Vector3 m_minSubcellScale = new Vector3 (0.1f, 0.1f, 0.1f);
	private Vector3 m_maxSubcellScale = new Vector3 (5f, 5f, 5f);

    [Header("On and off mats")]
    public Material OffStateMaterial;
    public Material[] OnStateMaterials;

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
		m_highlight = m_modelRoot.GetComponentInChildren<Light> ();
		m_highlight.enabled = false;
		foreach (NamedPrefab np in m_modelPrefabDefs)
		{
			m_modelPrefabs.Add (np.PrefabName, np.PrefabGameObject);
		}
		m_boltPooler.InitialisePool (20); 
	}

	public void ScaleSubcell(Subcell a_subcell, float a_newScale)
	{
		Vector3 scale = a_subcell.transform.localScale + new Vector3 (a_newScale, a_newScale, a_newScale);
		scale = Vector3.Max (m_minSubcellScale, scale);
		scale = Vector3.Min (m_maxSubcellScale, scale);
		a_subcell.transform.localScale = scale;
		RecalculateModelScale ();
	}

	private void RecalculateModelScale()
	{
		if (m_isInitialised)
		{
			float containerScale = 2f;
			foreach(Subcell subcell in m_subcells)
			{
				containerScale += subcell.transform.localScale.x;
			}
			containerScale = (1.25f * containerScale);
			m_mainContainer.transform.localScale = new Vector3 (containerScale, containerScale, containerScale);
		}
	}

	public void InitialiseModel(PresentationData a_pData)
	{
		if (!m_isInitialised)
		{
			float containerScale = 2f;
			//TODO - check for edited scale in future!
			foreach (ServiceData serv in a_pData.Services)
			{
				containerScale += serv.InitialScale;
			}
			containerScale = (1.25f * containerScale);

			m_mainContainer = Instantiate<GameObject> (m_mainContainerPrefab, m_modelRoot.transform);
			m_mainContainer.transform.localScale = new Vector3 (containerScale, containerScale, containerScale);

			Vector3 minBounds = m_mainContainer.GetComponent<MeshCollider> ().bounds.min;
			minBounds.Scale (new Vector3 (0.3f, 0.3f, 0.3f));

			for (int i = 0; i < a_pData.Services.Length; i++)
			{
				GameObject sub = Instantiate<GameObject> (m_modelPrefabs[a_pData.Services[i].ServiceName], m_modelRoot.transform);
				m_subcells.Add (sub.GetComponent<Subcell> ());
				m_subcells [i].Initialise (a_pData.Services [i]);
				sub.transform.localPosition = Vector3.Scale (minBounds, m_placementVectors [i]);
			}
			m_isInitialised = true;

            CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);

            if (ConnectionGenerator.Instance != null)
            {
                ConnectionGenerator.Instance.ReceiveSubCells(m_subcells);
            }
			PDFManager.Instance.PrePopulate (a_pData);
            ShakeModel();
		}
	}

	public void HighlightSubcell(Subcell a_subcell)
	{
		if (!m_highlightActive)
		{
			m_highlightedSubcell = a_subcell;
			m_highlightActive = true;
			StartCoroutine ("IlluminateHighlightedSubcell", a_subcell);
		}
	}

	public void OnSubcellCollision(GameObject a_objOne, GameObject a_objTwo)
	{
		m_boltPooler.UseBoltFromPool (a_objOne, a_objTwo);
	}

	public CaseCell[] GetAllCaseCells()
	{
		List<CaseCell> allCaseCells = new List<CaseCell> ();
		if (m_isInitialised)
		{
			foreach (Subcell cell in m_subcells)
			{
				allCaseCells.AddRange (cell.CaseCells);
			}
		}
		return allCaseCells.ToArray ();
	}

	public Subcell[] GetAllSubCells()
	{
		return m_subcells.ToArray ();
	}

	public void ShakeModel()
	{
		if (m_isInitialised)
		{
			foreach (Subcell cell in m_subcells)
			{
				cell.RigidBody.AddForce ((Random.onUnitSphere * 3f), ForceMode.Impulse);
			}
		}
	}

	public void HaltSubcells()
	{
		if (m_isInitialised)
		{
			foreach (Subcell cell in m_subcells)
			{
				//Halt subcells
				cell.RigidBody.AddForce ((cell.RigidBody.velocity * -1), ForceMode.VelocityChange);
				cell.RigidBody.AddTorque ((cell.RigidBody.angularVelocity * -1), ForceMode.VelocityChange);
			}
		}
	}

	public void HaltSubcell(Subcell a_cell)
	{
		if (m_isInitialised)
		{
			//Halt subcell
			a_cell.RigidBody.AddForce ((a_cell.RigidBody.velocity * -1), ForceMode.VelocityChange);
			a_cell.RigidBody.AddTorque ((a_cell.RigidBody.angularVelocity * -1), ForceMode.VelocityChange);
		}
	}

	public void ClearModel()
	{
		if (m_isInitialised)
		{
			foreach (Subcell cell in m_subcells)
			{
				Destroy (cell.gameObject);
			}
			m_subcells.Clear ();
			Destroy (m_mainContainer);
			m_isInitialised = false;
		}
	}

	private IEnumerator IlluminateHighlightedSubcell(Subcell acell)
	{
        if (CameraInputManager.Instance.m_CurrentPhase != CameraInputManager.Phase.FocusedSubCellPhase)
        {
            foreach (Subcell cell in m_subcells)
            {
                cell.ToggleLabelText(true);
            }
        }

		m_highlight.enabled = true;
        yield return new WaitForSeconds(3);
        foreach (Subcell cell in m_subcells)
        {
            cell.ToggleLabelText(false);
        }
		m_highlightActive = false;
		m_highlight.enabled = false;

		yield return null;
	}

	private void Update()
	{
		if (m_highlightActive)
		{
			m_highlight.transform.position = m_highlightedSubcell.transform.position + new Vector3 (0f, 0f, -8f);
		}
	}
}

[System.Serializable]
public struct NamedPrefab
{
	public string PrefabName;
	public GameObject PrefabGameObject;
}