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
	private GameObject m_msNotesPanelPrefab;
	[SerializeField]
	private GameObject m_serviceConfigPanelPrefab;
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
	[SerializeField]
	private Button m_submitButton;

	private List<ServiceData> m_services = new List<ServiceData> ();
	private PresentationData m_presentationData;

	private ScrollingButtonSelect m_scrollSelect;
	private MSNotesPanel m_notesPanel;
	private ServiceConfigPanel m_servicePanel;


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
		m_msAddMarketsBox.OnPressed += OpenAddMarketPanel;
		m_msAddNotesBox.OnPressed += OpenNotesPanel;
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

	private void OpenAddMarketPanel()
	{
		m_serviceButtonGrid.SetActive (false);
		GameObject scrollObj = Instantiate(m_buttonScrollPrefab, gameObject.transform) as GameObject;
		m_scrollSelect = scrollObj.GetComponent<ScrollingButtonSelect> ();
		m_scrollSelect.Initialise (m_marketLabels, m_presentationData.Markets);
		m_scrollSelect.OnCloseRequest += CloseAddMarketPanel;
	}

	private void CloseAddMarketPanel()
	{
		m_scrollSelect.OnCloseRequest -= CloseAddMarketPanel;
		m_presentationData.Markets = m_scrollSelect.SelectedOptions;
		if (m_presentationData.Markets.Length > 0)
		{
			m_msAddMarketsBox.IsToggled = true;
		} 
		else
		{
			m_msAddMarketsBox.IsToggled = false;
		}
		Destroy (m_scrollSelect.gameObject);
		m_serviceButtonGrid.SetActive (true);
	}

	private void OpenNotesPanel()
	{
		if (m_presentationData.Notes == null)
		{
			m_presentationData.Notes = new string[1];
		}
		GameObject notesObj = Instantiate(m_msNotesPanelPrefab, gameObject.transform) as GameObject;
		m_notesPanel = notesObj.GetComponent<MSNotesPanel> ();
		if (!string.IsNullOrEmpty (m_presentationData.Notes [0]))
		{
			m_notesPanel.InputText = m_presentationData.Notes[0];
		}
		m_notesPanel.OnCloseRequest += CloseNotesPanel;
	}

	private void CloseNotesPanel()
	{
		m_notesPanel.OnCloseRequest -= CloseNotesPanel;

		if (string.IsNullOrEmpty (m_notesPanel.InputText))
		{
			m_presentationData.Notes [0] = "";
		} 
		else
		{
			m_presentationData.Notes [0] = m_notesPanel.InputText;
		}
			
		if (!string.IsNullOrEmpty (m_presentationData.Notes[0]))
		{
			m_msAddNotesBox.IsToggled = true;
		} 
		else
		{
			m_msAddNotesBox.IsToggled = false;
		}

		Destroy (m_notesPanel.gameObject);
	}


	private void ServiceButtonSelectedEventHandler(string a_selectedOption)
	{
		Debug.Log ("You have selected " + a_selectedOption);
		GameObject servPanelObj = Instantiate(m_serviceConfigPanelPrefab, gameObject.transform) as GameObject;
		m_servicePanel = servPanelObj.GetComponent<ServiceConfigPanel> ();
		int indx = GetIndexByServiceName (a_selectedOption);
		if (indx > 0)
		{
			m_servicePanel.Initialise (m_services [indx]);
		}
		else
		{
			m_servicePanel.Initialise (a_selectedOption);
		}
		m_servicePanel.OnSaveService += SaveServiceDataEventHandler;
	}

	private int GetIndexByServiceName(string a_serviceName)
	{
		int servIndex = -1;
		for (int i = 0; i < m_services.Count; i++)
		{
			if (m_services [i].ServiceName == a_serviceName)
			{
				servIndex = i;
				break;
			}
		}
		return servIndex;
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

	private void SaveServiceDataEventHandler(ServiceData a_servData)
	{
		if (m_services.Contains (a_servData))
		{
			return;
		}

		int indx = GetIndexByServiceName (a_servData.ServiceName);
		if (indx > 0)
		{
			m_services [indx] = a_servData;
		}
		else
		{
			m_services.Add (a_servData);
		}
	}
}
