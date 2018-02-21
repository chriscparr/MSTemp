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
	private Text m_titleText;
	[SerializeField]
	private Text m_typeText;
	[SerializeField]
	private Text m_otherText;

	private OnRailsMovement m_ourMover;
	// Use this for initialization

	public void DisplayCaseStudy(CaseStudyData a_caseData)
	{
		ClearCaseStudy ();
		m_titleText.text = a_caseData.TitleText;
		m_typeText.text = a_caseData.CaseStudyType;
		if (a_caseData.CaseStudyType == "TEXT")
		{
			m_otherText.text = a_caseData.BodyText;
		} 
		if (a_caseData.CaseStudyType == "VIDEO")
		{
			m_otherText.text = a_caseData.VideoPath;
		}
	}

	public void ClearCaseStudy()
	{
		m_titleText.text = "";
		m_typeText.text = "";
		m_otherText.text = "";
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
