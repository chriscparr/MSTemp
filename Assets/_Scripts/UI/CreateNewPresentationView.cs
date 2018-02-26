using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewPresentationView : MonoBehaviour 
{

	[SerializeField]
	private GameObject m_buttonScrollPrefab;
	[SerializeField]
	private GameObject m_msDiffBoxPrefab;
	[SerializeField]
	private MSAddStuffBox m_msAddIndustryBox;
	[SerializeField]
	private MSAddStuffBox m_msAddMarketsBox;
	[SerializeField]
	private MSAddStuffBox m_msAddNotesBox;


	private string[] m_buttonServiceLabels = new string[] {"FAST","SHOP","GROWTH","DATA","LOOP","CONTENT","AGILE","LIFE"};
	private string[] m_industryLabels = new string[] {"FASHION","CONSTRUCTION","FINANCE","ENTERTAINMENT","HEALTH","EDUCATION","LEGAL","ADVERTISING"};
	private string[] m_marketLabels = new string[] {"EUROPE","NORTH AMERICA","ASIA","MIDDLE EAST","AFRICA","AUSTRALIA","OCEANA","SOUTH AMERICA"};


	[SerializeField]
	private GameObject m_serviceButtonGrid;
	[SerializeField]
	private Button m_closeButton;

	private List<ServiceData> m_services = new List<ServiceData> ();

	private ScrollingButtonSelect m_scrollSelect;

	private PresentationData m_presentationData;

	public void SetupView(PresentationData a_pData = null)
	{
		m_services.Clear ();
		if (a_pData != null)
		{
			m_presentationData = a_pData;
		} 
		else
		{
			m_presentationData = new PresentationData ();
		}

		if (m_presentationData.Industries != null && m_presentationData.Industries.Length > 0)
		{
			m_msAddIndustryBox.IsToggled = true;
		}

	}

	private void Awake()
	{
		foreach (string label in m_buttonServiceLabels)
		{
			GameObject btnObj = Instantiate(m_msDiffBoxPrefab, m_serviceButtonGrid.transform) as GameObject;
			MindshareButton msButton = btnObj.GetComponent<MindshareButton> ();
			msButton.SetButtonValue (label);
			msButton.OnSelected += ServiceButtonSelectedEventHandler;
			msButton.OnUnselected += ServiceButtonUnselectedEventHandler;
		}
		m_closeButton.onClick.AddListener (CloseButtonEventHandler);
		m_msAddIndustryBox.OnPressed += OpenAddIndustryPanel;
		SetupView ();
	}

	private void CloseButtonEventHandler()
	{
		//UIManager.Instance.OpenSomeOtherView();
	}

	private void OpenAddIndustryPanel()
	{
		m_serviceButtonGrid.SetActive (false);
		GameObject scrollObj = Instantiate(m_buttonScrollPrefab, gameObject.transform) as GameObject;
		m_scrollSelect = scrollObj.GetComponent<ScrollingButtonSelect> ();
		m_scrollSelect.Initialise (m_industryLabels, m_presentationData.Industries);
		m_scrollSelect.OnCloseRequest += CloseAddIndustryPanel;
	}

	private void CloseAddIndustryPanel()
	{
		m_scrollSelect.OnCloseRequest -= CloseAddIndustryPanel;
		m_presentationData.Industries = m_scrollSelect.SelectedOptions;
		if (m_presentationData.Industries.Length > 0)
		{
			m_msAddIndustryBox.IsToggled = true;
		} 
		else
		{
			m_msAddIndustryBox.IsToggled = false;
		}
		Destroy (m_scrollSelect.gameObject);
		m_serviceButtonGrid.SetActive (true);
	}


	private void ServiceButtonSelectedEventHandler(string a_selectedOption)
	{
		Debug.Log ("You have selected " + a_selectedOption);
		//spawn service config panel or whatever

	}

	private void ServiceButtonUnselectedEventHandler(string a_unSelectedOption)
	{
		Debug.Log ("You have UNselected " + a_unSelectedOption);
		/*
		for (int i = 0; i < m_services.Count; i++)
		{
			if (m_services [i].ServiceName.ToUpper () == a_unSelectedOption.ToUpper ())
			{
				m_services.RemoveAt (i);
				break;
			}
		}
		*/
	}
}
