using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour
{
    [SerializeField]
	private Button m_TouchIDButton;
	[SerializeField]
	private GameObject m_bioTouchPrefab;

	[SerializeField]
	private GameObject m_pwPanel;
	[SerializeField]
	private Button m_pwPanelCloseBtn;
	[SerializeField]
	private Button m_pwPanelLoginBtn;
	[SerializeField]
	private InputField m_pwPanelEmailInput;
	[SerializeField]
	private InputField m_pwPanelPWInput;

	private BiometricTouch m_bioTouch;

    private void OnTouchIDButtonPressed()
	{
		if (m_bioTouch == null)
		{
			GameObject bioTouch = Instantiate<GameObject> (m_bioTouchPrefab, null);
			m_bioTouch = bioTouch.GetComponent<BiometricTouch> ();
			m_bioTouch.OnTouchResult += BioTouchEventHandler;
		}
		m_bioTouch.RequestTouchAuth ();
    }

	private void OnEnable()
	{
		m_TouchIDButton.onClick.AddListener (OnTouchIDButtonPressed);
		m_pwPanelCloseBtn.onClick.AddListener (OnPanelCloseButton);
		m_pwPanelLoginBtn.onClick.AddListener (OnPanelLoginButton);
	}

	private void OnDisable()
	{
		m_TouchIDButton.onClick.RemoveListener (OnTouchIDButtonPressed);
		m_pwPanelCloseBtn.onClick.RemoveListener (OnPanelCloseButton);
		m_pwPanelLoginBtn.onClick.RemoveListener (OnPanelLoginButton);
	}

	private void OpenEmailPWPanel()
	{
		m_pwPanelEmailInput.text = "";
		m_pwPanelPWInput.text = "";
		m_TouchIDButton.gameObject.SetActive (false);
		m_pwPanel.SetActive (true);
	}

	private void OnPanelCloseButton()
	{
		m_TouchIDButton.gameObject.SetActive (true);
		m_pwPanel.SetActive (false);
	}

	private void OnPanelLoginButton()
	{
		//TODO actually verify text inputs! Here are some very insecure checks in the meantime!
		string emailCheckText = m_pwPanelEmailInput.text;
		string passwordCheckText = m_pwPanelPWInput.text;
		if (m_pwPanelEmailInput.text == emailCheckText && m_pwPanelPWInput.text == passwordCheckText)
		{
			OnPanelCloseButton ();
			UIManager.Instance.ShowNewOrSavedView ();
		}
		else
		{
			OpenEmailPWPanel ();
		}
	}
		
	private void BioTouchEventHandler(BiometricTouch.TouchIDResult a_touchResult)
	{
		switch (a_touchResult)
		{
			case BiometricTouch.TouchIDResult.SUCCESS:
				Debug.Log ("Touch Result - User authenticated successfully");
				UIManager.Instance.ShowNewOrSavedView ();
				break;
			case BiometricTouch.TouchIDResult.AUTH_FAIL:
				Debug.Log ("Touch Result - Authentication Failed");
				OpenEmailPWPanel ();
				break;
			case BiometricTouch.TouchIDResult.CANCEL_PRESSED:
				Debug.Log ("Touch Result - User pressed the Cancel button");
				OpenEmailPWPanel ();
				break;
			case BiometricTouch.TouchIDResult.ENTER_PW_PRESSED:
				Debug.Log ("Touch Result - User pressed \"Enter Password\"");
				OpenEmailPWPanel ();
				break;
			case BiometricTouch.TouchIDResult.NOT_CONFIGURED:
				Debug.Log ("Touch Result - Touch ID is not configured");
				OpenEmailPWPanel ();
				break;
			case BiometricTouch.TouchIDResult.CANT_EVALUATE:
				Debug.Log ("Touch Result - Can not evaluate Touch ID");
				OpenEmailPWPanel ();
				break;
			case BiometricTouch.TouchIDResult.IN_EDITOR:
				Debug.Log ("Touch Result - In the editor, doughnut!");
				OpenEmailPWPanel ();
				break;
		}
	}
}
