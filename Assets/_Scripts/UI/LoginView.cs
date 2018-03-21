using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class LoginView : MonoBehaviour
{
    [SerializeField]
    private Button m_loginButton;

    [DllImport("__Internal")]
    private static extern void TouchID();

    private void OnLoginButtonPressed()
    {
#if UNITY_EDITOR
        Success("");
#elif UNITY_IOS
        TouchID();
#endif

    }

    public void Success(string message)
    {
        UIManager.Instance.ShowNewOrSavedView();
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
