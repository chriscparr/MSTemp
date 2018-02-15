using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NewPresentationView : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_serviceInputPanelPrefab;

	[SerializeField]
	private Button m_savePresentationButton;
	[SerializeField]
	private InputField m_presenterNameInput;
	[SerializeField]
	private InputField m_presenterPositionInput;
	[SerializeField]
	private InputField m_clientNameInput;
	[SerializeField]
	private ArrayInput m_industryInput;
	[SerializeField]
	private ArrayInput m_marketsInput;
	[SerializeField]
	private ArrayInput m_notesInput;
	[SerializeField]
	private Button m_addServiceButton;
	[SerializeField]
	private Button m_removeServiceButton;
	[SerializeField]
	private Button m_backButton;

	[SerializeField]
	private Text m_serviceDisplayText;

	private const string ServiceTextPrefix = "Services: [";
	private const string ServiceTextSuffix = "]";

	private List<ServiceData> m_services = new List<ServiceData>();

	private ServiceInputPanel m_servicePanel;

	private void OnSavePresentationButtonPressed()
	{
		PresentationData pData = new PresentationData ();
		pData.PresenterName = m_presenterNameInput.text;
		pData.PresenterPosition = m_presenterPositionInput.text;
		pData.ClientName = m_clientNameInput.text;
		pData.Industries = m_industryInput.InputElements;
		pData.Markets = m_marketsInput.InputElements;
		pData.Notes = m_notesInput.InputElements;
		pData.Services = m_services.ToArray ();

		PersistentDataHandler.SaveFile<PresentationData> (pData.ID, pData);

		OnBackButtonPressed ();
	}

	private void OnBackButtonPressed()
	{
		UIManager.Instance.ShowNewOrSavedView ();
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		m_savePresentationButton.onClick.AddListener (OnSavePresentationButtonPressed);
		m_addServiceButton.onClick.AddListener (AddNewService);
		m_removeServiceButton.onClick.AddListener (RemoveService);
		m_backButton.onClick.AddListener (OnBackButtonPressed);
		m_industryInput.LabelText = "Industries";
		m_marketsInput.LabelText = "Markets";
		m_notesInput.LabelText = "Notes";
	}

	private void OnDisable()
	{
		m_savePresentationButton.onClick.RemoveListener (OnSavePresentationButtonPressed);
		m_addServiceButton.onClick.RemoveListener (AddNewService);
		m_removeServiceButton.onClick.RemoveListener (RemoveService);
		m_backButton.onClick.RemoveListener (OnBackButtonPressed);
	}

	private void AddNewService()
	{
		GameObject servPanel = Instantiate<GameObject> (m_serviceInputPanelPrefab, this.gameObject.transform);
		m_servicePanel = servPanel.GetComponent<ServiceInputPanel> ();
		m_servicePanel.OnSubmitService += ServiceSubmitEventHandler;
		m_servicePanel.OnClosePanel += ServicePanelCloseEventHandler;
	}

	private void RemoveService()
	{
		if (m_services.Count > 0)
		{
			m_services.RemoveAt (m_services.Count - 1);
		}
		RefreshServicesDisplay ();
	}

	private void ServicePanelCloseEventHandler()
	{
		m_servicePanel.OnClosePanel -= ServicePanelCloseEventHandler;
		m_servicePanel.OnSubmitService -= ServiceSubmitEventHandler;
	}

	private void ServiceSubmitEventHandler(ServiceData a_sData)
	{
		m_services.Add (a_sData);
		RefreshServicesDisplay ();
	}

	private void RefreshServicesDisplay()
	{
		StringBuilder serviceNameList = new StringBuilder();
		serviceNameList.Append (ServiceTextPrefix);
		for (int i = 0; i < m_services.Count; i++)
		{
			serviceNameList.Append (m_services [i].ServiceName.ToLower());
			if (i != m_services.Count - 1)
			{
				serviceNameList.Append (",");
			}
		}
		serviceNameList.Append (ServiceTextSuffix);
		m_serviceDisplayText.text = serviceNameList.ToString ();
	}
}
