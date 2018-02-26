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

	private string[] m_buttonServiceLabels = new string[] {"FAST","SHOP","GROWTH","DATA","LOOP","CONTENT","AGILE","LIFE"};

	[SerializeField]
	private GameObject m_serviceButtonGrid;
	[SerializeField]
	private Button m_closeButton;

	private List<ServiceData> m_services = new List<ServiceData> ();


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
	}

	private void CloseButtonEventHandler()
	{
		//UIManager.Instance.OpenSomeOtherView();
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
