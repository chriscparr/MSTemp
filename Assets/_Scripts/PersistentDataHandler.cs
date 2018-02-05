using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PersistentDataHandler : MonoBehaviour 
{

	//Replace this with code from previous work

	public void SaveJSON(string a_jsonStr, string a_fileName)
	{
		string path = Application.persistentDataPath + "/" + a_fileName + ".json";
		if (!File.Exists (path))
		{
			StreamWriter writer = new StreamWriter (path, false);
			writer.Write (a_jsonStr);
			writer.Close ();
		} 
		else
		{
			throw new FileLoadException ("File already exists!");
		}
	}
	/*
	public PresentationData[] LoadPresentationData()
	{
		List<PresentationData> presentations = new List<PresentationData> ();
		string path = Application.persistentDataPath;
		DirectoryInfo d = new DirectoryInfo (path);
		FileInfo[] files = d.GetFiles ();

		int jsonCount = 0;
		foreach (FileInfo finfo in files)
		{
			if (finfo.Extension.Contains ("json"))
			{
				jsonCount++;

			}
		}

	}
	*/

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
