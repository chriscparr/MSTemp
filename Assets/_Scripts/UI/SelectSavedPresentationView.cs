using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSavedPresentationView : MonoBehaviour 
{
	[SerializeField]
	private Button m_presentationOneButton;


	private void OnPresentationOneButtonPressed()
	{
		//UIManager - display next screen
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		m_presentationOneButton.onClick.AddListener (OnPresentationOneButtonPressed);
	}

	private void OnDisable()
	{
		m_presentationOneButton.onClick.RemoveListener (OnPresentationOneButtonPressed);
	}
}
