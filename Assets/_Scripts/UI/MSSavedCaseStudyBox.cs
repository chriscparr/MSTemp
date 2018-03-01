using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSSavedCaseStudyBox : MonoBehaviour 
{
	public delegate void CaseStudySetupAction(CaseStudyData a_csData);
	public CaseStudySetupAction OnCaseEdit;
	public CaseStudySetupAction OnCaseRemove;

	[SerializeField]
	private Text m_buttonLabelText;
	[SerializeField]
	private Button m_editButton;
	[SerializeField]
	private Button m_removeButton;

	public CaseStudyData CaseData {get{ return m_caseData;}}
	private CaseStudyData m_caseData;

	public void Initialise(CaseStudyData a_csData)
	{
		m_caseData = a_csData;
		m_buttonLabelText.text = m_caseData.TitleText;
		m_editButton.onClick.AddListener (EditCaseStudy);
		m_removeButton.onClick.AddListener (RemoveCaseStudy);
	}

	private void EditCaseStudy()
	{
		if (OnCaseEdit != null)
		{
			OnCaseEdit (m_caseData);
		}
	}

	private void RemoveCaseStudy()
	{
		if (OnCaseRemove != null)
		{
			OnCaseRemove (m_caseData);
		}
	}
		
}
