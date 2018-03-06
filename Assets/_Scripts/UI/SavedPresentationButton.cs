using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedPresentationButton : MonoBehaviour 
{
	public delegate void SavedPresentationSelectedEvent(PresentationData a_pData);
	public SavedPresentationSelectedEvent OnPresentationSelected;
	public SavedPresentationSelectedEvent OnEditPresentation;
	public SavedPresentationSelectedEvent OnDeletePresentation;

	[SerializeField]
	private Button m_mainButton;
	[SerializeField]
	private Button m_editButton;
	[SerializeField]
	private Button m_deleteButton;
	[SerializeField]
	private Text m_buttonText;

	private PresentationData m_pData;
	public PresentationData Presentation {get { return m_pData; } set { m_pData = value; InitButton ();}}

	private void InitButton()
	{
		m_buttonText.text = m_pData.ClientName.ToUpper ();
	}

	private void OnButtonClicked()
	{
		if (OnPresentationSelected != null)
		{
			OnPresentationSelected (m_pData);
		}
	}

	private void OnEditButtonClicked()
	{
		if (OnEditPresentation != null)
		{
			OnEditPresentation (m_pData);
		}
	}

	private void OnDeleteButtonClicked()
	{
		if (OnDeletePresentation != null)
		{
			OnDeletePresentation (m_pData);
		}
	}

	private void OnEnable()
	{
		m_mainButton.onClick.AddListener (OnButtonClicked);
		m_editButton.onClick.AddListener (OnEditButtonClicked);
		m_deleteButton.onClick.AddListener (OnDeleteButtonClicked);
	}

	private void OnDisable()
	{
		m_mainButton.onClick.RemoveListener (OnButtonClicked);
		m_editButton.onClick.RemoveListener (OnEditButtonClicked);
		m_deleteButton.onClick.RemoveListener (OnDeleteButtonClicked);
	}
}
