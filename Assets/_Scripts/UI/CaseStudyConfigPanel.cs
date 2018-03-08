using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseStudyConfigPanel : MonoBehaviour 
{
	public delegate void CaseStudyConfigAction(CaseStudyData a_csData);
	public CaseStudyConfigAction OnSaveCaseStudy;
	public delegate void CaseStudyPanelAction();
	public CaseStudyPanelAction OnCloseCaseStudyPanel;

	[SerializeField]
	private Button m_closeButton;
	[SerializeField]
	private Button m_textTypeButton;
	[SerializeField]
	private Button m_videoTypeButton;
	[SerializeField]
	private Button m_addVideoButton;
	[SerializeField]
	private Button m_saveCaseButton;
	[SerializeField]
	private InputField m_csTitleInputText;
	[SerializeField]
	private InputField m_csIntroInputText;
	[SerializeField]
	private InputField m_csBodyInputText;
	[SerializeField]
	private Text m_videoPathText;
	[SerializeField]
	private Color m_msPurple;

	private bool m_isVideoType = false;
	private CaseStudyData m_caseData;
	private VideoPicker m_videoPicker;

	private void Start()
	{
		m_videoPicker = gameObject.GetComponent<VideoPicker> ();
		m_textTypeButton.onClick.AddListener (TextButtonPressed);
		m_videoTypeButton.onClick.AddListener (VideoButtonPressed);
		m_addVideoButton.onClick.AddListener (AddVideoButtonPressed);
		m_saveCaseButton.onClick.AddListener (SubmitCaseStudyData);
		m_closeButton.onClick.AddListener (ClosePanel);
	}

	public void Initialise(CaseStudyData a_csData = null)
	{
		if (a_csData != null)
		{
			m_caseData = a_csData;
			m_csTitleInputText.text = m_caseData.TitleText;
			m_csIntroInputText.text = m_caseData.IntroText;
			m_csBodyInputText.text = m_caseData.BodyText;
			m_videoPathText.text = "Video location : " + m_caseData.VideoPath;
			if (m_caseData.CaseStudyType == "VIDEO")
			{
				VideoButtonPressed ();
			}
			else
			{
				TextButtonPressed ();
			}
		}
		else
		{
			m_caseData = new CaseStudyData ();
			VideoButtonPressed ();
		}
	}

	private void VideoButtonPressed()
	{

			m_videoTypeButton.image.color = Color.white;
			m_videoTypeButton.GetComponentInChildren<Text> ().color = m_msPurple;
			m_textTypeButton.image.color = m_msPurple;
			m_textTypeButton.GetComponentInChildren<Text> ().color = Color.white;
			m_csBodyInputText.gameObject.SetActive (false);
			m_videoPathText.gameObject.SetActive (true);
			m_addVideoButton.gameObject.SetActive (true);
			m_caseData.CaseStudyType = "VIDEO";
			m_isVideoType = true;
	}

	private void TextButtonPressed()
	{
			m_textTypeButton.image.color = Color.white;
			m_textTypeButton.GetComponentInChildren<Text> ().color = m_msPurple;
			m_videoTypeButton.image.color = m_msPurple;
			m_videoTypeButton.GetComponentInChildren<Text> ().color = Color.white;
			m_csBodyInputText.gameObject.SetActive (true);
			m_videoPathText.gameObject.SetActive (false);
			m_addVideoButton.gameObject.SetActive (false);
			m_caseData.CaseStudyType = "TEXT";
			m_isVideoType = false;
	}

	private void AddVideoButtonPressed()
	{
		#if UNITY_EDITOR
		return;
		#endif
		m_videoPicker.OnVideoSelected += VideoSelectedEventHandler;
		m_videoPicker.ShowVideoPicker();
	}

	private void VideoSelectedEventHandler(string a_videoPath)
	{
		m_videoPicker.OnVideoSelected -= VideoSelectedEventHandler;
		m_caseData.VideoPath = "file://" + PersistentDataHandler.SaveVideo (a_videoPath);
		m_videoPathText.text = m_caseData.VideoPath;
	}

	private void ClosePanel()
	{
		if (OnCloseCaseStudyPanel != null)
		{
			OnCloseCaseStudyPanel ();
		}
	}

	private void SubmitCaseStudyData()
	{
		m_caseData.TitleText = m_csTitleInputText.text;
		m_caseData.IntroText = m_csIntroInputText.text;
		m_caseData.BodyText = m_csBodyInputText.text;
		m_caseData.VideoPath = m_videoPathText.text;
		if (OnSaveCaseStudy != null)
		{
			OnSaveCaseStudy (m_caseData);
		}
	}

	private void OnDestroy()
	{
		m_textTypeButton.onClick.RemoveListener (TextButtonPressed);
		m_videoTypeButton.onClick.RemoveListener (VideoButtonPressed);
		m_addVideoButton.onClick.RemoveListener (AddVideoButtonPressed);
		m_saveCaseButton.onClick.RemoveListener (SubmitCaseStudyData);
		m_closeButton.onClick.RemoveListener (ClosePanel);
	}
}
