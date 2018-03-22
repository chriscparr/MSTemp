using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour
{
    [SerializeField]
    private Button m_loginButton;
	[SerializeField]
	private BiometricTouch m_bioTouch;

	private void Start()
	{
		//m_bioTouch = gameObject.AddComponent<BiometricTouch> ();
	}

    private void OnLoginButtonPressed()
	{
		m_bioTouch.RequestTouchAuth ();
    }

	private void OnEnable()
	{
		m_bioTouch.OnTouchResult += BioTouchEventHandler;
		m_loginButton.onClick.AddListener (OnLoginButtonPressed);
	}

	private void OnDisable()
	{
		m_bioTouch.OnTouchResult -= BioTouchEventHandler;
		m_loginButton.onClick.RemoveListener (OnLoginButtonPressed);
	}

	private void BioTouchEventHandler(bool a_result, string a_message)
	{
		UIManager.Instance.ShowNewOrSavedView();
		gameObject.SetActive(false);
	}
}
