[System.Serializable]
public class CaseStudyData
{
	public CaseStudyData()
	{
		CaseStudyType = "";
		TitleText = "";
		IntroText = "";
		BodyText = "";
		VideoPath = "";
	}

	public string CaseStudyType;
	public string TitleText;
	public string IntroText;
	public string BodyText;
	public string VideoPath;
}