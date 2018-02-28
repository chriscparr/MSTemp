[System.Serializable]
public class ServiceData
{
	public ServiceData()
	{
		InitialScale = 1.0f;
		EditedScale = 1.0f;
		Edited = false;
		ServiceIntroQuestion = "";
		CaseStudies = new CaseStudyData[] { };
	}

	public string ServiceName;
	public float InitialScale;
	public float EditedScale;
	public bool Edited;
	public string ServiceIntroQuestion;
	public CaseStudyData[] CaseStudies;
}