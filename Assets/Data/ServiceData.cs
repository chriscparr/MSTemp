using UnityEngine;

[System.Serializable]
public class ServiceData
{
	public string ServiceName;
	public float InitialScale;
	public float EditedScale;
	public bool Edited;
	public string ServiceIntroQuestion;
	public CaseStudyData[] CaseStudies;
}