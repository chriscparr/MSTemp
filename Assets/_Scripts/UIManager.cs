using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour 
{
	public static UIManager Instance {get { return s_instance;}}
	private static UIManager s_instance = null;

	public CaseStudyView CaseViewer {get { return m_caseView;}}

	[SerializeField]
	private GameObject m_loginViewPrefab;
	[SerializeField]
	private GameObject m_newOrSavedViewPrefab;
	[SerializeField]
	private GameObject m_newPresentationViewPrefab;
	[SerializeField]
	private GameObject m_selectSavedViewPrefab;
	[SerializeField]
	private GameObject m_presentationViewPrefab;
	[SerializeField]
	private GameObject m_endPresentationViewPrefab;
	[SerializeField]
	private GameObject m_serviceSummaryViewPrefab;
    [SerializeField]
    private GameObject m_caseStudyViewPrefab;

	private GameObject m_loginView;
	private GameObject m_newOrSavedView;
	private GameObject m_newPresentationView;
	private GameObject m_selectSavedView;
	private GameObject m_presentationView;
	private GameObject m_endPresentationView;
	private GameObject m_serviceSummaryView;
    private GameObject m_caseStudyView;

	private CaseStudyView m_caseView;

	public void ShowLoginView()
	{
		m_loginView.SetActive (true);
	}
	public void ShowNewOrSavedView()
	{
		m_newOrSavedView.SetActive (true);
	}
	public void ShowNewPresentationView()
	{
		m_newPresentationView.SetActive (true);
	}
	public void ShowSelectSavedView()
	{
		m_selectSavedView.SetActive (true);
	}
	public void ShowPresentationView()
	{
		m_presentationView.SetActive (true);
		CameraInputManager.Instance.ResetPosition ();
	}
	public void ShowEndPresentationView()
	{
		m_endPresentationView.SetActive (true);
	}
	public void ShowServiceSummaryView(ServiceData a_sData)
	{
		m_serviceSummaryView.SetActive (true);
		m_serviceSummaryView.GetComponent<ServiceSummaryView> ().SetupServiceView (a_sData);
	}
	public void ShowCaseStudyView()
    {
        m_caseStudyView.SetActive(true);
    }


	private void Awake()
	{
		s_instance = this;

		m_loginView = Instantiate<GameObject> (m_loginViewPrefab, this.gameObject.transform);
		m_newOrSavedView = Instantiate<GameObject> (m_newOrSavedViewPrefab, this.gameObject.transform);
		m_newPresentationView = Instantiate<GameObject> (m_newPresentationViewPrefab, this.gameObject.transform);
		m_selectSavedView = Instantiate<GameObject> (m_selectSavedViewPrefab, this.gameObject.transform);
		m_presentationView = Instantiate<GameObject> (m_presentationViewPrefab, this.gameObject.transform);
		m_endPresentationView = Instantiate<GameObject> (m_endPresentationViewPrefab, this.gameObject.transform);
		m_serviceSummaryView = Instantiate<GameObject> (m_serviceSummaryViewPrefab, this.gameObject.transform);
        m_caseStudyView = Instantiate<GameObject>(m_caseStudyViewPrefab, this.gameObject.transform);

		m_caseView = m_caseStudyView.GetComponent<CaseStudyView> ();

		m_loginView.SetActive (false);
		m_newOrSavedView.SetActive (false);
		m_newPresentationView.SetActive (false);
		m_selectSavedView.SetActive (false);
		m_presentationView.SetActive (false);
		m_endPresentationView.SetActive (false);
		m_serviceSummaryView.SetActive (false);
        m_caseStudyView.SetActive(false);
	}

}
