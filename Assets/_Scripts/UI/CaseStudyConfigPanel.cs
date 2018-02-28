using System.Collections;
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

	private bool m_isVideoType = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
