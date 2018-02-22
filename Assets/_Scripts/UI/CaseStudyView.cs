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
	private Button m_introNextButton;
	[SerializeField]
	private Button m_contentNextButton;
	[SerializeField]
	private GameObject m_introPanel;
	[SerializeField]
	private Text m_titleText;
	[SerializeField]
	private Text m_introQuestionText;
	[SerializeField]
	private GameObject m_contentPanel;
	[SerializeField]
	private Image m_bodyTextBG;
	[SerializeField]
	private Text m_bodyText;
	[SerializeField]
	private Image m_videoPlaceholder;

	private OnRailsMovement m_ourMover;
	private CanvasGroup m_introCanvas;
	private CanvasGroup m_contentCanvas;

	public void DisplayCaseStudy(CaseStudyData a_caseData)
	{
		ClearCaseStudy ();
		m_titleText.text = a_caseData.TitleText;
		m_introQuestionText.text = "Is this just an introductory question, or could it be the beginning of a new path for your business?";

		if (a_caseData.CaseStudyType == "TEXT")
		{
			m_bodyTextBG.gameObject.SetActive (true);
			m_bodyText.gameObject.SetActive (true);
			m_bodyText.text = a_caseData.BodyText;
			m_videoPlaceholder.gameObject.SetActive (false);
		} 
		if (a_caseData.CaseStudyType == "VIDEO")
		{
			//m_bodyText.text = a_caseData.VideoPath;
			m_bodyText.gameObject.SetActive (false);
			m_bodyTextBG.gameObject.SetActive (false);
			m_videoPlaceholder.gameObject.SetActive (true);
		}
		ShowIntroPanel ();
	}

	private void ShowIntroPanel()
	{
		//m_contentNextButton.onClick.RemoveListener (GoToNextCaseStudy);
		iTween.ValueTo (gameObject, iTween.Hash ("from", 0f, "to", 1f, "time", 1f,"delay",0f, "onupdate", "ApplyIntroPanelAlpha"));
	}

	private void ShowContentPanel()
	{
		m_introCanvas.alpha = 0f;
		m_contentCanvas.alpha = 1f;
		Debug.Log ("ShowContentPanel()");
		//m_introNextButton.onClick.RemoveListener (ShowContentPanel);
		//m_contentNextButton.onClick.AddListener (GoToNextCaseStudy);
		//iTween.ValueTo (gameObject, iTween.Hash ("from", 1f, "to", 0f, "time", 1f,"delay",0f, "onupdate", "ApplyIntroPanelAlpha"));
		//iTween.ValueTo (gameObject, iTween.Hash ("from", 0f, "to", 1f, "time", 1f,"delay",1f, "onupdate", "ApplyContentPanelAlpha"));
	}

	private void FinishCaseStudy()
	{
		Debug.Log ("FinishCaseStudy()");
		//iTween.ValueTo (gameObject, iTween.Hash ("from", 1f, "to", 0f, "time", 1f,"delay",0f, "onupdate", "ApplyContentPanelAlpha", "oncompletetarget", gameObject, "oncomplete", "GoToNextCaseStudy"));
		ClearCaseStudy();
		GoToNextCaseStudy();
	}

	public void ClearCaseStudy()
	{
		m_contentCanvas.alpha = 0f;
		m_introCanvas.alpha = 0f;
	}

	private void Start () 
	{
		m_ourMover = Camera.main.GetComponent<OnRailsMovement>();
		m_introCanvas = m_introPanel.GetComponent<CanvasGroup> ();
		m_contentCanvas = m_contentPanel.GetComponent<CanvasGroup> ();
		/*
		m_nextButton.gameObject.SetActive (false);
		m_previousButton.gameObject.SetActive (false);
		m_homeButton.gameObject.SetActive (false);
		*/
	}
	
	private void OnEnable()
	{
		/*
		m_nextButton.onClick.AddListener (GoToNextCaseStudy);
		m_previousButton.onClick.AddListener (GoToPreviousCaseStudy);
		m_homeButton.onClick.AddListener (GoHome);
		*/
		m_introNextButton.onClick.AddListener (ShowContentPanel);
		m_contentNextButton.onClick.AddListener (GoToNextCaseStudy);
	}
	
	private void OnDisable()
	{
		/*
		m_nextButton.onClick.RemoveListener (GoToNextCaseStudy);
		m_previousButton.onClick.RemoveListener (GoToPreviousCaseStudy);
		m_homeButton.onClick.RemoveListener (GoHome);
		*/
		m_introNextButton.onClick.RemoveListener (ShowContentPanel);
		m_contentNextButton.onClick.RemoveListener (GoToNextCaseStudy);
	}

	// Update is called once per frame
	public void GoToNextCaseStudy () 
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

	private void ApplyIntroPanelAlpha(float _alpha)
	{
		m_introCanvas.alpha = _alpha;
	}

	private void ApplyContentPanelAlpha(float _alpha)
	{
		m_contentCanvas.alpha = _alpha;
	}
}
