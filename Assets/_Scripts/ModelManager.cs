using System.Collections;
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

	public bool IsInitialised {get { return m_isInitialised; }}
	private bool m_isInitialised = false;

	private Subcell m_highlightedSubcell;
	private Light m_highlight;
	private bool m_highlightActive = false;

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
	}

	public void ScaleSubcell(Subcell a_subcell, float a_newScale)
	{
		a_subcell.transform.localScale = new Vector3 (a_newScale,a_newScale,a_newScale);
		RecalculateModelScale ();
	}

	private void RecalculateModelScale()
	{
		if (m_isInitialised)
		{
			float containerScale = 0f;
			foreach(Subcell subcell in m_subcells)
			{
				containerScale += subcell.transform.localScale.x;
			}
			containerScale = (2f * containerScale) + 2f;
			if (containerScale < 10f)
			{
				containerScale = 10f;
			}
			m_mainContainer.transform.localScale = new Vector3 (containerScale, containerScale, containerScale);
		}
	}

	public void InitialiseModel(PresentationData a_pData)
	{
		if (!m_isInitialised)
		{
			float containerScale = 0f;
			foreach (ServiceData serv in a_pData.Services)
			{
				containerScale += serv.ServiceWeighting;
			}
			containerScale = (2f * containerScale) + 2f;
			if (containerScale < 10f)
			{
				containerScale = 10f;
			}
			m_mainContainer = Instantiate<GameObject> (m_mainContainerPrefab, m_modelRoot.transform);
			m_mainContainer.transform.localScale = new Vector3 (containerScale, containerScale, containerScale);

			Vector3 minBounds = m_mainContainer.GetComponent<MeshCollider> ().bounds.min;
			minBounds.Scale (new Vector3 (0.3f, 0.3f, 0.3f));

			for (int i = 0; i < a_pData.Services.Length; i++)
			{
				GameObject sub = Instantiate<GameObject> (m_subcellPrefab, m_modelRoot.transform);
				m_subcells.Add (sub.GetComponent<Subcell> ());
				m_subcells [i].Initialise (a_pData.Services [i]);
				sub.transform.localPosition = Vector3.Scale (minBounds, m_placementVectors [i]);
			}
			m_isInitialised = true;

            // CameraInputManager.Instance.SetLookAtTarget(m_mainContainer.transform);
            CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
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

	public void ShakeModel()
	{
		if (m_isInitialised)
		{
			Vector3 origin = m_mainContainer.transform.position;
			Vector3 dir = new Vector3 ();
			foreach (Subcell cell in m_subcells)
			{
				dir = origin - cell.transform.position;
				dir.Scale(new Vector3(0.5f, 0.5f, 0.5f));
				cell.RigidBody.AddForce (dir, ForceMode.Impulse);
			}
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
        // here, you are focusing on a sub cell
        // so zoom into it, get up nice and close.
        // let the camera input manager deal with the movement
        acell.Label.text = acell.ServiceDat.ServiceName;
		m_highlight.enabled = true;
        // CameraInputManager.Instance.SetLookAtTarget(m_highlightedSubcell.transform);
        // CameraInputManager.Instance.FollowTarget();
        yield return new WaitForSeconds(3);
        acell.Label.text = "";
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
