using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour
{
    [SerializeField]
    private Button m_loginButton;
	[SerializeField]
	private GameObject m_bioTouchPrefab;

	private BiometricTouch m_bioTouch;

    private void OnLoginButtonPressed()
	{
		if (m_bioTouch == null)
		{
			GameObject bioTouch = Instantiate<GameObject> (m_bioTouchPrefab, null);
			m_bioTouch = bioTouch.GetComponent<BiometricTouch> ();
			m_bioTouch.OnTouchResult += BioTouchEventHandler;
			m_bioTouch.RequestTouchAuth ();
		}
		else
		{
			m_bioTouch.RequestTouchAuth ();
		}
    }

	private void OnEnable()
	{
		m_loginButton.onClick.AddListener (OnLoginButtonPressed);
	}

	private void OnDisable()
	{
		m_loginButton.onClick.RemoveListener (OnLoginButtonPressed);
	}

	private void BioTouchEventHandler(bool a_result, int a_msgCode, string a_message)
	{
		//m_bioTouch.OnTouchResult -= BioTouchEventHandler;

		if (a_result)
		{
			UIManager.Instance.ShowNewOrSavedView ();
		}
		else
		{
			Debug.Log (a_message);
			//UIManager.Instance.ShowNewOrSavedView ();
		}
		if (a_msgCode == 0 || a_msgCode == 1)
		{
			m_bioTouch.OnTouchResult -= BioTouchEventHandler;
			Destroy (m_bioTouch.gameObject);
			m_bioTouch = null;
		}
	}
}
