﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseStudyConfigPanel : MonoBehaviour 
{
	public delegate void CaseStudyConfigAction(CaseStudyData a_csData);
	public CaseStudyConfigAction OnSaveCaseStudy;

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

	private void Start()
	{
		m_textTypeButton.onClick.AddListener (TextButtonPressed);
		m_videoTypeButton.onClick.AddListener (VideoButtonPressed);
		m_addVideoButton.onClick.AddListener (AddVideoButtonPressed);
		Initialise ();
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
		if (!m_isVideoType)
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
	}

	private void TextButtonPressed()
	{
		if (m_isVideoType)
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
	}

	private void AddVideoButtonPressed()
	{

	}

	private void OnDestroy()
	{
		m_textTypeButton.onClick.RemoveListener (TextButtonPressed);
		m_videoTypeButton.onClick.RemoveListener (VideoButtonPressed);
		m_addVideoButton.onClick.RemoveListener (AddVideoButtonPressed);
	}
}
