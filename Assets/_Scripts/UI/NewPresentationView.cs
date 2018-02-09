using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPresentationView : MonoBehaviour 
{
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

	private List<ServiceData> m_services = new List<ServiceData>();

	private void OnSavePresentationButtonPressed()
	{
		UIManager.Instance.ShowNewOrSavedView ();
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		m_savePresentationButton.onClick.AddListener (OnSavePresentationButtonPressed);
		m_industryInput.LabelText = "Industries";
		m_marketsInput.LabelText = "Markets";
		m_notesInput.LabelText = "Notes";
	}

	private void OnDisable()
	{
		m_savePresentationButton.onClick.RemoveListener (OnSavePresentationButtonPressed);
	}
}
