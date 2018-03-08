using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

public class PersistentDataHandler 
{
	//----------------------------------------------------------------------------
	// Private Variables:
	//----------------------------------------------------------------------------

	private static string path = "";
	private const string fileExtension = ".json";
	private const string folder = "/Data/";

	/// <summary>
	/// Saves the json file to persistant datapath of the device
	/// </summary>
	/// <param name="fileName">File name.</param>
	/// <param name="jsonFile">Json file.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void SaveFile<T>(string fileName,T jsonFile) where T : class
	{
		ValidateFolderPath();

		string json = JsonUtility.ToJson(jsonFile,true);

		// Optimization
		// Stringbuilder uses the same reference when you append strings instead of creates a new copy each time
		StringBuilder filePath = new StringBuilder();
		filePath.Append(path);
		filePath.Append(fileName);
		filePath.Append(fileExtension);

		try
		{
			using (FileStream fs = new FileStream(filePath.ToString(), FileMode.Create))
			{
				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.Write(json);	
				}
			}
			Debug.Log("PersistentDataHandler: Save of " + fileName + " Completed");
		}
		catch(Exception e)
		{
			Debug.Log("PersistentDataHandler: Save Error: " + e.Message);
		}
	}

	public static string SaveVideo(string a_filePath)
	{
		ValidateFolderPath();
		string sourcePath = a_filePath.Replace ("file://","");	//convert from url to normal file path
		string destinationPath = path + a_filePath.Substring (a_filePath.LastIndexOf ("/") + 1);
		//Debug.Log ("PersistentDataHandler: Source file path: " + sourcePath + ", Destination file path: " + destinationPath);
		File.Copy (sourcePath, destinationPath);
		return destinationPath;
	}


	/// <summary>
	/// Saves the json file to persistant datapath of the device
	/// </summary>
	/// <param name="fileName">File name.</param>
	/// <param name="jsonString">Json String.</param>
	public static void SaveFile(string fileName, string jsonString)
	{
		ValidateFolderPath();
		StringBuilder filePath = new StringBuilder();
		filePath.Append(path);
		filePath.Append(fileName);
		filePath.Append(fileExtension);

		try
		{
			using (FileStream fs = new FileStream(filePath.ToString(), FileMode.Create))
			{
				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.Write(jsonString);	
				}
			}
			Debug.Log("PersistentDataHandler: Save of " + fileName + " Completed");
		}
		catch(Exception e)
		{
			Debug.Log("PersistentDataHandler: Save Error: " + e.Message);
		}
	}

	/// <summary>
	/// Loads the json file from the persistant datapath of the device
	/// </summary>
	/// <returns>The file.</returns>
	/// <param name="fileName">File name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T LoadFile<T>(string fileName) where T : class
	{
		ValidateFolderPath();
		T file = null;
		try
		{
			StringBuilder filePath = new StringBuilder();
			filePath.Append(path);
			filePath.Append(fileName);
			filePath.Append(fileExtension);

			using (StreamReader reader = new StreamReader(filePath.ToString()))
			{
				string json = "";
				json = reader.ReadToEnd();
				file = JsonUtility.FromJson<T>(json);
			}
			Debug.Log("PersistentDataHandler: Load of " + fileName + " Completed");
		}
		catch(Exception e)
		{
			Debug.Log("PersistentDataHandler: Load Error: " + e.Message);
		}
		return file;
	}

	/// <summary>
	/// Loads the json file from the persistant datapath of the device
	/// </summary>
	/// <returns>Json string.</returns>
	/// <param name="fileName">File name.</param>
	public static string LoadFile(string fileName)
	{
		ValidateFolderPath();
		string jsonString = null;
		try
		{
			StringBuilder filePath = new StringBuilder();
			filePath.Append(path);
			filePath.Append(fileName);
			filePath.Append(fileExtension);

			using (StreamReader reader = new StreamReader(filePath.ToString()))
			{
				string json = "";
				json = reader.ReadToEnd();
				jsonString = json;
			}
			Debug.Log("PersistentDataHandler: Load of " + fileName + " Completed");
		}
		catch(Exception e)
		{
			Debug.Log("PersistentDataHandler: Load Error: " + e.Message);
		}
		return jsonString;
	}

	public static PresentationData[] GetAllSavedPresentations()
	{
		List<PresentationData> savedPres = new List<PresentationData> ();
		foreach (string fName in GetJsonFilenames())
		{
			savedPres.Add (LoadFile<PresentationData>(fName));
		}
		return savedPres.ToArray ();
	}

	public static string[] GetJsonFilenames()
	{
		ValidateFolderPath ();
		List<string> fileNames = new List<string> ();
		DirectoryInfo d = new DirectoryInfo (path);
		FileInfo[] fInfos = d.GetFiles ();
		foreach (FileInfo f in fInfos)
		{
			if (f.Extension.Contains ("json"))
			{
				fileNames.Add (f.Name.Replace (".json", ""));
			}
		}
		return fileNames.ToArray ();
	}

	public static void DeleteFile(string fileName)
	{
		StringBuilder filePath = new StringBuilder();
		filePath.Append(path);
		filePath.Append(fileName);
		filePath.Append(fileExtension);

		if (File.Exists (filePath.ToString()))
		{
			File.Delete (filePath.ToString());
			Debug.Log("PersistentDataHandler: " + filePath.ToString() + " successfully deleted!");
		}
	}

	//----------------------------------------------------------------------------
	// Private Methods:
	//----------------------------------------------------------------------------

	private static void ValidateFolderPath()
	{
		if(string.IsNullOrEmpty(path))
		{
			path = Application.persistentDataPath + folder;

			if(!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
	}
}
