using UnityEngine;

[System.Serializable]
public class PresentationData
{
	public PresentationData()
	{
		ID = System.Guid.NewGuid ().ToString ();
		PresenterName = "";
		PresenterPosition = "";
		ClientName = "";
		Industries = new string[] { };
		Markets = new string[] { };
		Notes = new string[] { };
	}

	public string ID;
	public string PresenterName;
	public string PresenterPosition;
	public string ClientName;
	public string[] Industries;
	public string[] Markets;
	public string[] Notes;
	public ServiceData[] Services;
}