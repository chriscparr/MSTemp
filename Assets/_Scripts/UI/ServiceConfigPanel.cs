using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServiceConfigPanel : MonoBehaviour 
{
	public delegate void ServiceConfigAction(ServiceData a_servData);
	public ServiceConfigAction OnSaveService;


	[SerializeField]
	private Text m_title;
	[SerializeField]
	private Button m_closeButton;
	[SerializeField]
	private Button m_saveButton;
	[SerializeField]
	private Button m_addCaseStudyButton;
	[SerializeField]
	private InputField m_introTextInput;
	[SerializeField]
	private Slider m_initScaleSlider;
	[SerializeField]
	private GameObject m_scrollContent;

	[SerializeField]
	private GameObject m_savedCaseBoxPrefab;
	[SerializeField]
	private GameObject m_caseStudyConfigPanelPrefab;

	private CaseStudyConfigPanel m_caseConfigPanel;

	private ServiceData m_serviceData;

	private List<MSSavedCaseStudyBox> m_csBoxes = new List<MSSavedCaseStudyBox> ();

	private int m_editedCaseIndex = -1;

	private void Awake()
	{
		m_closeButton.onClick.AddListener (ClosePanel);
		m_saveButton.onClick.AddListener (SaveButtonPressed);
		m_addCaseStudyButton.onClick.AddListener (AddCaseStudyButtonPressed);
	}

	public void Initialise(string a_serviceName)
	{
		m_serviceData = new ServiceData ();
		m_serviceData.ServiceName = a_serviceName.ToUpper ();
		m_title.text = m_serviceData.ServiceName;
		BuildCaseStudyDisplay ();
	}

	public void Initialise(ServiceData a_servData)
	{
		m_serviceData = a_servData;
		m_title.text = m_serviceData.ServiceName;
		m_introTextInput.text = m_serviceData.ServiceIntroQuestion;
		m_initScaleSlider.value = m_serviceData.InitialScale;
		BuildCaseStudyDisplay ();
	}

	private void BuildCaseStudyDisplay()
	{
		m_addCaseStudyButton.gameObject.transform.SetParent (this.transform);
		m_addCaseStudyButton.gameObject.SetActive (false);

		for (int i = 0; i < m_serviceData.CaseStudies.Length; i++)
		{
			GameObject boxObj = Instantiate(m_savedCaseBoxPrefab, m_scrollContent.transform) as GameObject;
			MSSavedCaseStudyBox caseBox = boxObj.GetComponent<MSSavedCaseStudyBox> ();
			caseBox.Initialise (m_serviceData.CaseStudies [i]);
			caseBox.OnCaseEdit += EditCaseStudy;
			caseBox.OnCaseRemove += RemoveCaseStudy;
			m_csBoxes.Add (caseBox);
		}

		m_addCaseStudyButton.gameObject.SetActive (true);
		m_addCaseStudyButton.gameObject.transform.SetParent (m_scrollContent.transform);
	}

	private void EditCaseStudy(CaseStudyData a_csData)
	{
		//open case study edit panel
		List<CaseStudyData> csList = new List<CaseStudyData>(m_serviceData.CaseStudies);
		m_editedCaseIndex = csList.IndexOf (a_csData);
		GameObject csPanelObj = Instantiate<GameObject>(m_caseStudyConfigPanelPrefab, gameObject.transform);
		m_caseConfigPanel = csPanelObj.GetComponent<CaseStudyConfigPanel> ();
		m_caseConfigPanel.OnSaveCaseStudy += CaseStudyConfigComplete;
		m_caseConfigPanel.OnCloseCaseStudyPanel += CloseCaseStudyConfigPanel;
		m_caseConfigPanel.Initialise (a_csData);
	}

	private void RemoveCaseStudy(CaseStudyData a_csData)
	{
		List<CaseStudyData> csList = new List<CaseStudyData>(m_serviceData.CaseStudies);
		csList.Remove (a_csData);
		m_serviceData.CaseStudies = csList.ToArray ();
		ClearCaseStudyDisplay ();
		BuildCaseStudyDisplay ();
	}

	private void ClearCaseStudyDisplay()
	{
		foreach (MSSavedCaseStudyBox box in m_csBoxes)
		{
			box.OnCaseEdit -= EditCaseStudy;
			box.OnCaseRemove -= RemoveCaseStudy;
			box.gameObject.SetActive (false);
			Destroy (box.gameObject);
		}
		m_csBoxes.Clear ();
	}

	private void AddCaseStudyButtonPressed()
	{
		m_editedCaseIndex = -1;
		GameObject csPanelObj = Instantiate<GameObject>(m_caseStudyConfigPanelPrefab, gameObject.transform);
		m_caseConfigPanel = csPanelObj.GetComponent<CaseStudyConfigPanel> ();
		m_caseConfigPanel.OnSaveCaseStudy += CaseStudyConfigComplete;
		m_caseConfigPanel.OnCloseCaseStudyPanel += CloseCaseStudyConfigPanel;
		m_caseConfigPanel.Initialise ();
	}

	private void CloseCaseStudyConfigPanel()
	{
		m_caseConfigPanel.OnCloseCaseStudyPanel -= CloseCaseStudyConfigPanel;
		m_caseConfigPanel.OnSaveCaseStudy -= CaseStudyConfigComplete;
		m_caseConfigPanel.gameObject.SetActive (false);
		Destroy (m_caseConfigPanel.gameObject);
	}

	private void CaseStudyConfigComplete(CaseStudyData a_csData)
	{
		if (m_editedCaseIndex >= 0)
		{
			m_serviceData.CaseStudies [m_editedCaseIndex] = a_csData;
			m_editedCaseIndex = -1;
		}
		else
		{
			List<CaseStudyData> csList = new List<CaseStudyData>(m_serviceData.CaseStudies);
			csList.Add (a_csData);
			m_serviceData.CaseStudies = csList.ToArray ();
		}
		CloseCaseStudyConfigPanel ();
		ClearCaseStudyDisplay ();
		BuildCaseStudyDisplay ();
	}

	private void SaveButtonPressed()
	{
		InputsToServiceData ();
		SubmitCompletedServiceData ();
		ClosePanel ();
	}

	private void InputsToServiceData()
	{
		m_serviceData.ServiceIntroQuestion = m_introTextInput.text;
		m_serviceData.InitialScale = m_initScaleSlider.value;
		//case studies should already be saved as we go
	}

	private void SubmitCompletedServiceData()
	{
		if (OnSaveService != null)
		{
			OnSaveService (m_serviceData);
		}
	}

	private void ClosePanel()
	{
		m_saveButton.onClick.RemoveListener (SaveButtonPressed);
		m_closeButton.onClick.RemoveListener (ClosePanel);
		m_addCaseStudyButton.onClick.RemoveListener (AddCaseStudyButtonPressed);
		ClearCaseStudyDisplay ();
		this.gameObject.SetActive (false);
		Destroy (this.gameObject);
	}

}
