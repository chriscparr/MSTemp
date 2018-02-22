using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseStudyView : MonoBehaviour 
{
	[SerializeField]
	private Button m_nextButton;
	[SerializeField]
	private Button m_previousButton;
	[SerializeField]
	private Button m_homeButton;
	[SerializeField]
	private GameObject m_introPanel;
	[SerializeField]
	private Text m_titleText;
	[SerializeField]
	private Text m_introQuestionText;
	[SerializeField]
	private GameObject m_contentPanel;
	[SerializeField]
	private Text m_bodyText;

	[SerializeField]
	private GameObject m_videoPanel;

	[SerializeField]
	private Image m_videoPlaceholder;

	private OnRailsMovement m_ourMover;


	public void DisplayCaseStudy(CaseStudyData a_caseData)
	{
		ClearCaseStudy ();
		m_titleText.text = a_caseData.TitleText;
		m_introQuestionText.text = "Is this just an introductory question, or could it be the beginning of a new path for your business?";
		m_introPanel.SetActive(true);
		iTween.FadeFrom (m_introPanel, iTween.Hash ("alpha",0f,"time",2f,"easetype",iTween.EaseType.easeInExpo));


		if (a_caseData.CaseStudyType == "TEXT")
		{
			m_bodyText.text = a_caseData.BodyText;
		} 
		if (a_caseData.CaseStudyType == "VIDEO")
		{
			m_bodyText.text = a_caseData.VideoPath;
		}
	}

	public void ClearCaseStudy()
	{
		m_introPanel.SetActive (false);
		m_contentPanel.SetActive (false);
		m_titleText.text = "";
		m_introQuestionText.text = "";
		m_bodyText.text = "";
	}

	private void Start () 
	{
		m_ourMover = Camera.main.GetComponent<OnRailsMovement>();
	}
	
	private void OnEnable()
	{
		m_nextButton.onClick.AddListener (GoToNextCaseStudy);
		m_previousButton.onClick.AddListener (GoToPreviousCaseStudy);
		m_homeButton.onClick.AddListener (GoHome);
	}
	
	private void OnDisable()
	{
		m_nextButton.onClick.RemoveListener (GoToNextCaseStudy);
		m_previousButton.onClick.RemoveListener (GoToPreviousCaseStudy);
		m_homeButton.onClick.RemoveListener (GoHome);
	}

	// Update is called once per frame
	private void GoToNextCaseStudy () 
	{
		ClearCaseStudy ();
		m_ourMover.GoToNextPoint();
	}

	private void GoToPreviousCaseStudy() 
	{
		ClearCaseStudy ();
        m_ourMover.GoToPreviousPoint();
    }

	private void GoHome() 
	{
		ClearCaseStudy ();
		m_ourMover.GoHome ();
        gameObject.SetActive(false);
    }
}
