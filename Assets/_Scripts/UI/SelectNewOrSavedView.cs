using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectNewOrSavedView : MonoBehaviour 
{
	[SerializeField]
	private Button m_closeButton;
	[SerializeField]
	private Button m_useExistingButton;
	[SerializeField]
	private Button m_newPresentationButton;

	private void OnUseExistingButtonPressed()
	{
		UIManager.Instance.ShowSelectSavedView ();
		gameObject.SetActive(false);
	}

	private void OnNewPresentationButtonPressed()
	{
		UIManager.Instance.ShowNewPresentationView ();
		gameObject.SetActive(false);
	}

	private void OnCloseButtonPressed()
	{
		UIManager.Instance.ShowLoginView ();
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		m_useExistingButton.onClick.AddListener (OnUseExistingButtonPressed);
		m_newPresentationButton.onClick.AddListener (OnNewPresentationButtonPressed);
		m_closeButton.onClick.AddListener (OnCloseButtonPressed);
	}

	private void OnDisable()
	{
		m_useExistingButton.onClick.RemoveListener (OnUseExistingButtonPressed);
		m_newPresentationButton.onClick.RemoveListener (OnNewPresentationButtonPressed);
		m_closeButton.onClick.RemoveListener (OnCloseButtonPressed);
	}
}
