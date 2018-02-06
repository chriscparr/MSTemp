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

	public void ShakeModel()
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

	public void ClearModel()
	{
		foreach (Subcell cell in m_subcells)
		{
			Destroy (cell.gameObject);
		}
		m_subcells.Clear ();
		Destroy (m_mainContainer);
	}
}
