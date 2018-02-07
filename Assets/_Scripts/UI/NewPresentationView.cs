using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPresentationView : MonoBehaviour 
{
	[SerializeField]
	private Button m_savePresentationButton;

	private void OnSavePresentationButtonPressed()
	{
		UIManager.Instance.ShowNewOrSavedView ();
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		m_savePresentationButton.onClick.AddListener (OnSavePresentationButtonPressed);
	}

	private void OnDisable()
	{
		m_savePresentationButton.onClick.RemoveListener (OnSavePresentationButtonPressed);
	}
}
