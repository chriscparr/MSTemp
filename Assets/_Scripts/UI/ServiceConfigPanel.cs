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

	private ServiceData m_serviceData;

	private List<MSSavedCaseStudyBox> m_csBoxes = new List<MSSavedCaseStudyBox> ();

	private void Awake()
	{
		m_closeButton.onClick.AddListener (ClosePanel);
		m_saveButton.onClick.AddListener (SaveButtonPressed);
	}

	public void Initialise(string a_serviceName)
	{
		m_serviceData = new ServiceData ();
		List<string> acceptableNames = new List<string> (new string[] {"FAST","SHOP","GROWTH","DATA","LOOP","CONTENT","AGILE","LIFE"});
		if (acceptableNames.Contains (a_serviceName.ToUpper ()))
		{
			m_serviceData.ServiceName = a_serviceName.ToUpper ();
			m_title.text = m_serviceData.ServiceName;
		}
		else
		{
			throw new System.Exception (a_serviceName.ToUpper () + " is not an acceptable service name!");
		}
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
		for (int i = 0; i < m_serviceData.CaseStudies.Length; i++)
		{
			GameObject boxObj = Instantiate(m_savedCaseBoxPrefab, m_scrollContent.transform) as GameObject;
			MSSavedCaseStudyBox caseBox = boxObj.GetComponent<MSSavedCaseStudyBox> ();
			caseBox.Initialise (m_serviceData.CaseStudies [i]);
			caseBox.OnCaseEdit += EditCaseStudy;
			caseBox.OnCaseRemove += RemoveCaseStudy;
			m_csBoxes.Add (caseBox);
		}

	}

	private void EditCaseStudy(CaseStudyData a_csData)
	{
		//open case study edit panel
	}

	private void RemoveCaseStudy(CaseStudyData a_csData)
	{
		for (int i = 0; i < m_csBoxes.Count; i++)
		{
			if(m_csBoxes[i].CaseData == a_csData)
			{
				MSSavedCaseStudyBox box = m_csBoxes [i];
				box.OnCaseEdit -= EditCaseStudy;
				box.OnCaseRemove -= RemoveCaseStudy;
				m_csBoxes.RemoveAt (i);
				Destroy (box.gameObject);
				Debug.Log ("Successfully removed " + a_csData.TitleText + " from the list!");
				return;
			}
		}
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

	private void SaveButtonPressed()
	{
		InputToServiceData ();
		SubmitCompletedServiceData ();
		ClosePanel ();
	}

	private void InputToServiceData()
	{
		m_serviceData.ServiceIntroQuestion = m_introTextInput.text;
		m_serviceData.InitialScale = m_initScaleSlider.value;
		List<CaseStudyData> cases = new List<CaseStudyData> ();
		foreach (MSSavedCaseStudyBox box in m_csBoxes)
		{
			cases.Add (box.CaseData);
		}
		m_serviceData.CaseStudies = cases.ToArray ();
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
		ClearCaseStudyDisplay ();
		this.gameObject.SetActive (false);
		Destroy (this.gameObject);
	}

}
