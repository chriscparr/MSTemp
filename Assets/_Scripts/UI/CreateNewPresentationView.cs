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
	private InputField m_clientTextInput;
	[SerializeField]
	private InputField m_contactNameTextInput;
	[SerializeField]
	private InputField m_contactPositionTextInput;
	[SerializeField]
	private MSAddStuffBox m_msAddIndustryBox;
	[SerializeField]
	private MSAddStuffBox m_msAddMarketsBox;
	[SerializeField]
	private MSAddStuffBox m_msAddNotesBox;
	[SerializeField]
	private GameObject m_serviceButtonGrid;
	[SerializeField]
	private Button m_closeButton;
	[SerializeField]
	private Button m_submitButton;

	private string[] m_buttonServiceLabels = new string[] {"FAST","SHOP","GROWTH","DATA","LOOP","CONTENT","AGILE","LIFE"};
	private string[] m_industryLabels = new string[] {"FASHION","CONSTRUCTION","FINANCE","ENTERTAINMENT","HEALTH","EDUCATION","LEGAL","ADVERTISING"};
	private string[] m_marketLabels = new string[] {"EUROPE","NORTH AMERICA","ASIA","MIDDLE EAST","AFRICA","AUSTRALIA","OCEANA","SOUTH AMERICA"};

	private PresentationData m_presentationData;

	private ScrollingButtonSelect m_scrollSelect;
	private MSNotesPanel m_notesPanel;
	private ServiceConfigPanel m_servicePanel;

	private Dictionary<string, MSDiffSelectBox> m_diffBoxDict = new Dictionary<string, MSDiffSelectBox> ();

	public void SetupView(PresentationData a_pData = null)
	{
		if (a_pData != null)
		{
			m_presentationData = a_pData;
			//populate fields
			m_clientTextInput.text = m_presentationData.ClientName;
			m_contactNameTextInput.text = m_presentationData.PresenterName;
			m_contactPositionTextInput.text = m_presentationData.PresenterPosition;
			m_msAddIndustryBox.IsToggled = m_presentationData.Industries.Length > 0 ? true : false;
			m_msAddMarketsBox.IsToggled = m_presentationData.Markets.Length > 0 ? true : false;
			m_msAddNotesBox.IsToggled = m_presentationData.Notes[0].Length > 0 ? true : false;

			foreach (ServiceData sData in m_presentationData.Services)
			{
				m_diffBoxDict [sData.ServiceName].SavedServiceData = sData;
			}
		} 
		else
		{
			m_presentationData = new PresentationData ();
		}

	}

	private void Awake()
	{
		foreach (string label in m_buttonServiceLabels)
		{
			GameObject btnObj = Instantiate(m_msDiffBoxPrefab, m_serviceButtonGrid.transform) as GameObject;
			MSDiffSelectBox msDiffBox =  btnObj.GetComponent<MSDiffSelectBox> ();
			m_diffBoxDict.Add (label, msDiffBox);
			msDiffBox.SetButtonValue (label);
			msDiffBox.OnSelected += ServiceButtonSelectedEventHandler;
		}
		m_closeButton.onClick.AddListener (CloseButtonEventHandler);
		m_msAddIndustryBox.OnPressed += OpenAddIndustryPanel;
		m_msAddMarketsBox.OnPressed += OpenAddMarketPanel;
		m_msAddNotesBox.OnPressed += OpenNotesPanel;
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

		if (m_diffBoxDict [a_selectedOption].IsServiceDataReady)
		{
			m_servicePanel.Initialise (m_diffBoxDict [a_selectedOption].SavedServiceData);
		}
		else
		{
			m_servicePanel.Initialise (a_selectedOption);
		}
		m_servicePanel.OnSaveService += SaveServiceDataEventHandler;
	}

	private void SaveServiceDataEventHandler(ServiceData a_servData)
	{
		m_servicePanel.OnSaveService -= SaveServiceDataEventHandler;
		m_diffBoxDict [a_servData.ServiceName].SavedServiceData = a_servData;
	}

	private void SubmitPresentationData()
	{
		m_presentationData.ClientName = m_clientTextInput.text;
		m_presentationData.PresenterName = m_contactNameTextInput.text;
		m_presentationData.PresenterPosition = m_contactPositionTextInput.text;
		//Markets, Industries + notes should already be present

		List<ServiceData> serviceDats = new List<ServiceData> ();
		foreach (MSDiffSelectBox box in m_diffBoxDict.Values)
		{
			if (box.IsServiceDataReady)
			{
				serviceDats.Add (box.SavedServiceData);
			}
		}
		m_presentationData.Services = serviceDats.ToArray ();
		PersistentDataHandler.SaveFile<PresentationData> (m_presentationData.ID, m_presentationData);
		CloseButtonEventHandler ();
	}
}
