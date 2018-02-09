using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedPresentationButton : MonoBehaviour 
{
	public delegate void SavedPresentationSelectedEvent(PresentationData a_pData);
	public SavedPresentationSelectedEvent OnPresentationSelected;

	private Button m_mainButton;
	private Text m_buttonText;
	private PresentationData m_pData;
	public PresentationData Presentation {get { return m_pData; } set { m_pData = value; InitButton ();}}

	private void Awake()
	{
		m_mainButton = GetComponent<Button> ();
		m_buttonText = GetComponentInChildren<Text> ();
	}

	private void InitButton()
	{
		m_buttonText.text = m_pData.ClientName;
		m_mainButton.onClick.AddListener (OnButtonClicked);
	}

	private void OnButtonClicked()
	{
		if (OnPresentationSelected != null)
		{
			OnPresentationSelected (m_pData);
		}
	}

	private void OnDisable()
	{
		m_mainButton.onClick.RemoveListener (OnButtonClicked);
	}
}
