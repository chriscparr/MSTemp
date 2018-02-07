using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour 
{
	[SerializeField]
	private Button m_loginButton;

	private void OnLoginButtonPressed()
	{
		//UIManager - display next screen
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		m_loginButton.onClick.AddListener (OnLoginButtonPressed);
	}

	private void OnDisable()
	{
		m_loginButton.onClick.RemoveListener (OnLoginButtonPressed);
	}
}
