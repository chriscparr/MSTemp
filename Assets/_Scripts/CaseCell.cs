using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseCell : MonoBehaviour 
{
	[SerializeField]
	private TextMesh m_labelText;
	[SerializeField]
	private Renderer m_cellRend;
	[SerializeField]
	private Transform m_camPosPoint;

	private bool m_isInited = false;
	private CaseStudyData m_caseData;
	private Subcell m_parentSubcell;

	public Transform CameraPositioningPoint {get{ return m_camPosPoint;}}

	public void Initialise(Subcell a_parentSubcell, CaseStudyData a_caseData)
	{
		if (!m_isInited)
		{
			m_parentSubcell = a_parentSubcell;
			m_caseData = a_caseData;
			m_labelText.text = m_caseData.TitleText;
			m_cellRend.material.color = Random.ColorHSV();
			m_isInited = true;
		}
	}

	public void PlayCaseStudy()
	{
		//Call some method in uimanager that displays case study in a nice panel or whatever...
		if (m_isInited)
		{
			UIManager.Instance.CaseViewer.DisplayCaseStudy (m_caseData);
		}
	}

}
