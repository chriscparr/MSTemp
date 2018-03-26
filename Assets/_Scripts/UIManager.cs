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
	private ServiceSummaryView m_servSummaryView;
	private CreateNewPresentationView m_newPresView;

	private GameObject m_currentView;
	private GameObject m_previousView;
	private Vector3 m_viewStartPoint = new Vector3(0f, 800f, 0f);

	public void ShowLoginView()
	{
		if (m_currentView != null)
		{
			TransitionToNextView (m_currentView, m_loginView);
		}
		else
		{
			m_loginView.SetActive (true);
			m_loginView.transform.localPosition = Vector3.zero;
			m_currentView = m_loginView;
		}
	}
	public void ShowNewOrSavedView()
	{
		TransitionToNextView (m_currentView, m_newOrSavedView);
	}
	public void ShowNewPresentationView(PresentationData a_pData = null)
	{
		m_newPresView.SetupView (a_pData);
		TransitionToNextView (m_currentView, m_newPresentationView);
	}
	public void ShowSelectSavedView()
	{
		TransitionToNextView (m_currentView, m_selectSavedView);
	}
	public void ShowPresentationView()
	{
		TransitionToNextView (m_currentView, m_presentationView);
	}
	public void ShowEndPresentationView()
	{
		TransitionToNextView (m_currentView, m_endPresentationView);
	}
	public void ShowServiceSummaryView(ServiceData a_sData)
	{
		m_servSummaryView.SetupServiceView (a_sData);
		TransitionToNextView (m_currentView, m_serviceSummaryView);
	}
	public void ShowCaseStudyView()
	{
		TransitionToNextView (m_currentView, m_caseStudyView);
	}
	public void ShowManipulationView()
	{
		TransitionToNextView (m_currentView, m_DifferentiatorManipulationView);
	}
    public void ShowForwardSummaryView()
    {
		TransitionToNextView (m_currentView, m_ForwardSummaryView);
    }

	private void TransitionToNextView(GameObject a_currentView, GameObject a_nextView)
	{
		m_previousView = a_currentView;
		m_currentView = a_nextView;
		iTween.MoveTo (m_previousView, iTween.Hash ("position",m_viewStartPoint,"time",0.25f,"easetype",iTween.EaseType.easeInBack,"oncomplete","OnTransitionOneComplete","islocal",true));
	}

	private void OnTransitionOneComplete()
	{
		m_previousView.SetActive (false);
		m_currentView.SetActive (true);
		m_currentView.transform.localPosition = m_viewStartPoint;
		iTween.MoveTo (m_currentView, iTween.Hash ("position",Vector3.zero,"time",0.25f,"easetype",iTween.EaseType.easeInBack,"oncomplete","OnTransitionTwoComplete","islocal",true));
	}

	private void OnTransitionTwoComplete()
	{
		Debug.Log ("<color=#00ffff>Completed animated transition between views!</color>");
	}

	/*
	public void HideAllViews()
	{
		foreach (GameObject g in m_allViewObjects)
		{
			g.SetActive (false);
		}
	}
	*/

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
		m_servSummaryView = m_serviceSummaryView.GetComponent<ServiceSummaryView> ();
		m_newPresView = m_presentationView.GetComponent<CreateNewPresentationView> ();

		foreach (GameObject view in m_allViewObjects)
		{
			view.transform.localPosition = m_viewStartPoint;
			view.SetActive (false);
		}
	}
}
