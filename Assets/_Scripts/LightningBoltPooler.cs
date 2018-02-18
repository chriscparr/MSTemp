using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBoltPooler : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_lightningBoltPrefab;

	private float m_lb_MinDuration = 0.5f;
	private float m_lb_MaxDuration = 1f;
	private float m_lb_MinChaos = 0.15f;
	private float m_lb_MaxChaos = 0.5f;
	private int m_lb_MinGenerations = 4;
	private int m_lb_MaxGenerations = 6;

	private int m_poolSize;

	private List<LightningBoltScript> m_pool = new List<LightningBoltScript> ();

	private GameObject m_poolArea;

	private bool m_isInitialised = false;

	public void InitialisePool(int a_poolSize)
	{
		if (!m_isInitialised)
		{
			m_poolArea = new GameObject ("poolArea");
			m_poolArea.transform.position = new Vector3 (0f, -100f, 0f);
			m_isInitialised = true;
			m_poolSize = a_poolSize;
			for (int i = 0; i < m_poolSize; i++)
			{
				ReturnToPool (GenerateNewBolt ());
			}
		}
	}

	public void UseBoltFromPool(GameObject startObj, GameObject endObj, bool a_autoReturn = true)
	{
		if (m_isInitialised)
		{
			if (m_pool.Count < 1)
			{
				ReturnToPool (GenerateNewBolt ());
			}
			float duration = Random.Range (m_lb_MinDuration, m_lb_MaxDuration);
			LightningBoltScript lBolt = m_pool [0];
			m_pool.RemoveAt (0);
			lBolt.gameObject.SetActive (true);
			lBolt.StartObject = startObj;
			lBolt.EndObject = endObj;
			lBolt.ChaosFactor = Random.Range(m_lb_MinChaos, m_lb_MaxChaos);
			lBolt.Generations = Random.Range(m_lb_MinGenerations, m_lb_MaxGenerations);
			lBolt.Duration = 0.05f;
			if (a_autoReturn)
			{
				StartCoroutine(ReturnAfterTimeout(lBolt, duration));
			}
		}
	}

	public void ReturnToPool(LightningBoltScript a_bolt)
	{
		if (m_poolArea != null)
		{
			a_bolt.transform.position = m_poolArea.transform.position;
			m_pool.Add (a_bolt);
			a_bolt.gameObject.SetActive (false);
		}
	}

	private IEnumerator ReturnAfterTimeout(LightningBoltScript a_bolt, float a_timeDuration)
	{
		yield return new WaitForSeconds (a_timeDuration);
		ReturnToPool (a_bolt);
		yield return null;
	}

	private LightningBoltScript GenerateNewBolt()
	{
		GameObject boltObj;
		if (m_poolArea != null)
		{
			boltObj = Instantiate(m_lightningBoltPrefab, m_poolArea.transform) as GameObject;
		}
		else
		{
			boltObj = Instantiate(m_lightningBoltPrefab) as GameObject;
		}
		LightningBoltScript lScript = boltObj.GetComponent<LightningBoltScript>();
		return lScript;
	}

}
