using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance {get { return s_instance;}}
	private static GameManager s_instance = null;

	private void Awake()
	{
		s_instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		ModelManager.Instance.InitialiseModel (GenerateTestPresentation ());
	}
	
	private void Update()
	{
		if (Input.GetKeyDown (KeyCode.Q))
		{
			SaveDummyData(GenerateTestPresentation ());
		}
		if (Input.GetKeyDown (KeyCode.W))
		{
			//ClearModel ();
			Debug.Log ("<color=#ff0000>" + "Persistent data directory contains " + PersistentDataHandler.GetJsonFilenames().Length.ToString() + " json data files!</color>");
		}
		if (Input.GetKeyDown (KeyCode.S))
		{
			ModelManager.Instance.ShakeModel ();
		}
	}

	private PresentationData GenerateTestPresentation ()
	{
		string testData = "{\n\"ID\":\"" + System.Guid.NewGuid ().ToString () + "\",\n\"PresenterName\":\"Mr. Tester\",\n\"PresenterPosition\":\"Test Manager\",\n\"ClientName\":\"IndustryLeader Inc.\",\n\"Industries\":[\"Finance\", \"Retail\", \"World Domination\"],\n\"Markets\":[\"Europe\", \"Asia\", \"America\"],\n\"Notes\":[\"Note 1\", \"Note 2\", \"Note 3\"],\n\"Services\":\n[\n{\"ServiceName\":\"FAST\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"SHOP\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"GROWTH\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"DATA\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"LOOP\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"CONTENT\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"AGILE\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]},\n{\"ServiceName\":\"LIFE\",\"ServiceWeighting\":1.0,\"ServiceIntroText\":\"Introduction Text!\",\"ServiceVideoPaths\":[]}\n]\n}";
		return JsonUtility.FromJson<PresentationData> (testData);
	}

	private void SaveDummyData(PresentationData a_pData)
	{
		PersistentDataHandler.SaveFile<PresentationData> (a_pData.ID, a_pData);
		//Debug.Log ("<color=#ff0000>" + "Data file (id:" + data.ID + ") saved to persistent data directory!" + "</color>");
	}
}
