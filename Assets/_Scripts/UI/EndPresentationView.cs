using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPresentationView : MonoBehaviour 
{
	[SerializeField]
	private Button m_startAgainButton;

	private void OnStartAgainButtonPressed()
	{
		UIManager.Instance.ShowNewOrSavedView ();
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		m_startAgainButton.onClick.AddListener (OnStartAgainButtonPressed);
	}

	private void OnDisable()
	{
		m_startAgainButton.onClick.RemoveListener (OnStartAgainButtonPressed);
	}
}
