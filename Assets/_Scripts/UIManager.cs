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
	[SerializeField]
	private GameObject m_DifferentiatorManipulationPrefab;
    [SerializeField]
    private GameObject m_ForwardSummaryPrefab;

	private GameObject m_loginView;
	private GameObject m_newOrSavedView;
	private GameObject m_newPresentationView;
	private GameObject m_selectSavedView;
	private GameObject m_presentationView;
	private GameObject m_endPresentationView;
	private GameObject m_serviceSummaryView;
	private GameObject m_caseStudyView;
	private GameObject m_DifferentiatorManipulationView;
    private GameObject m_ForwardSummaryView;

	private GameObject[] m_allViewObjects;

	private CaseStudyView m_caseView;

	public void ShowLoginView()
	{
		HideAllViews ();
		m_loginView.SetActive (true);
	}
	public void ShowNewOrSavedView()
	{
		HideAllViews ();
		m_newOrSavedView.SetActive (true);
	}
	public void ShowNewPresentationView()
	{
		HideAllViews ();
		m_newPresentationView.SetActive (true);
	}
	public void ShowSelectSavedView()
	{
		HideAllViews ();
		m_selectSavedView.SetActive (true);
	}
	public void ShowPresentationView()
	{
		HideAllViews ();
		m_presentationView.SetActive (true);
	}
	public void ShowEndPresentationView()
	{
		HideAllViews ();
		m_endPresentationView.SetActive (true);
	}
	public void ShowServiceSummaryView(ServiceData a_sData)
	{
		HideAllViews ();
		m_serviceSummaryView.SetActive (true);
		m_serviceSummaryView.GetComponent<ServiceSummaryView> ().SetupServiceView (a_sData);
	}
	public void ShowCaseStudyView()
	{
		HideAllViews ();
		m_caseStudyView.SetActive(true);
	}
	public void ShowManipulationView()
	{
		HideAllViews ();
		m_DifferentiatorManipulationView.SetActive(true);
	}
    public void ShowForwardSummaryView()
    {
        HideAllViews();
        m_ForwardSummaryView.gameObject.SetActive(true);
    }
	public void HideAllViews()
	{
		foreach (GameObject g in m_allViewObjects)
		{
			g.SetActive (false);
		}
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
		m_DifferentiatorManipulationView = Instantiate <GameObject> (m_DifferentiatorManipulationPrefab, this.gameObject.transform);
        m_ForwardSummaryView = Instantiate<GameObject>(m_ForwardSummaryPrefab, this.gameObject.transform);

		m_allViewObjects = new GameObject[] {m_loginView, m_newOrSavedView, m_newPresentationView,
			m_selectSavedView, m_presentationView, m_endPresentationView, m_serviceSummaryView, m_caseStudyView, m_DifferentiatorManipulationView,
            m_ForwardSummaryView};



		m_caseView = m_caseStudyView.GetComponent<CaseStudyView> ();

		HideAllViews ();
	}
}
