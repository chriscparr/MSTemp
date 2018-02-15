using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance {get { return s_instance;}}
	private static GameManager s_instance = null;

	private float m_shakeForce = 10f;		//acceleration must exceed this limit to count as a shake
	private float m_shakeInterval = 1f;		//2 shakes must happen within this time (s)
	private float m_lastShakeTime = 0f;		//time of last detected shake
	private float m_nextShakeAfter = 0f;	//to avoid triggering more than once, wait until this time

	private void Awake()
	{
		s_instance = this;
	}

	public void SetupModel(PresentationData a_pData)
	{
		ModelManager.Instance.InitialiseModel (a_pData);
	}

	// Use this for initialization
	void Start () 
	{
		UIManager.Instance.ShowLoginView();
	}
	
	private void Update()
	{
		if ((Input.acceleration.sqrMagnitude >= m_shakeForce) && (Time.fixedTime >= m_nextShakeAfter))
		{
			//Debug.Log ("<color=#ff00ff>Single shake detected! Squared Magnitude: " + Input.acceleration.sqrMagnitude.ToString() + "</color>");
			if (m_lastShakeTime - Time.fixedTime <= m_shakeInterval)
			{
				//Debug.Log ("<color=#ff00ff>Double shake detected! Do stuff here!</color>");
				m_nextShakeAfter = Time.fixedTime + 5f;
				ModelManager.Instance.ShakeModel ();
			}
			m_lastShakeTime = Time.fixedTime;
		}


		if (Input.GetKeyDown (KeyCode.Q))
		{
			SaveDummyData(GenerateTestPresentation ());
		}
		if (Input.GetKeyDown (KeyCode.W))
		{
			Debug.Log ("<color=#ff0000>" + "Persistent data directory contains " + PersistentDataHandler.GetJsonFilenames().Length.ToString() + " json data files!</color>");
			/*
			foreach (string filePath in PersistentDataHandler.GetJsonFilenames())
			{
				PersistentDataHandler.DeleteFile (filePath);
			}
			*/
		}
		if (Input.GetKeyDown (KeyCode.S))
		{
			ModelManager.Instance.ShakeModel ();
		}
	}

	private PresentationData GenerateTestPresentation ()
	{
		string testData = "";			// "{\n\"ID\":\"" + System.Guid.NewGuid ().ToString () + "\",\n\"PresenterName\":\"Mr. Tester\",\n\"PresenterPosition\":\"Test Manager\",\n\"ClientName\":\"IndustryLeader Inc.\",\n\"Industries\":[\"Finance\", \"Retail\", \"World Domination\"],\n\"Markets\":[\"Europe\", \"Asia\", \"America\"],\n\"Notes\":[\"Note 1\", \"Note 2\", \"Note 3\"],\n\"Services\":\n[\n{\"ServiceName\":\"FAST\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"SHOP\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"GROWTH\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"DATA\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"LOOP\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"CONTENT\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"AGILE\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"LIFE\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}\n]\n}";
		string testData_pre = "{\n\"ID\":\"" + System.Guid.NewGuid ().ToString () + "\",\n\"PresenterName\":\"Mr. Tester\",\n\"PresenterPosition\":\"Test Manager\",\n\"ClientName\":\"IndustryLeader Inc.\",\n\"Industries\":[\"Finance\", \"Retail\", \"World Domination\"],\n\"Markets\":[\"Europe\", \"Asia\", \"America\"],\n\"Notes\":[\"Note 1\", \"Note 2\", \"Note 3\"],\n\"Services\":\n[\n";
		string testData1 = "{\"ServiceName\":\"SHOP\",\"ServiceWeighting\":1.5,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}";
		string testData2 = "{\"ServiceName\":\"GROWTH\",\"ServiceWeighting\":0.8,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}";
		string testData3 = "{\"ServiceName\":\"DATA\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}";
		string testData4 = "{\"ServiceName\":\"FAST\",\"ServiceWeighting\":0.5,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}";
		string testData5 = "{\"ServiceName\":\"LOOP\",\"ServiceWeighting\":0.7,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}";
		string testData6 = "{\"ServiceName\":\"CONTENT\",\"ServiceWeighting\":1.8,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}";
		string testData7 = "{\"ServiceName\":\"AGILE\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}";
		string testData8 = "{\"ServiceName\":\"LIFE\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}";
		string testData_post = "]\n}";

		testData = testData_pre + testData1 + ",\n" + testData2 + ",\n" + testData3 + ",\n" + testData4 + ",\n" + testData5 + ",\n" + testData6 + ",\n" + testData7 + ",\n" + testData8 + "\n" + testData_post;

		//testData = testData_pre + testData1 + ",\n" + testData2 + ",\n" + testData3 + "\n" + testData_post;


		return JsonUtility.FromJson<PresentationData> (testData);
	}

	private void SaveDummyData(PresentationData a_pData)
	{
		PersistentDataHandler.SaveFile<PresentationData> (a_pData.ID, a_pData);
		//Debug.Log ("<color=#ff0000>" + "Data file (id:" + data.ID + ") saved to persistent data directory!" + "</color>");
	}
}
