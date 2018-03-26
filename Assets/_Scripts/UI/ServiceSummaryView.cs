using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServiceSummaryView : MonoBehaviour 
{
	[SerializeField]
	private Button m_exitButton;
	[SerializeField]
	private Text m_titleText;
	[SerializeField]
	private Text m_introText;

	public void SetupServiceView(ServiceData a_sData)
	{
		m_titleText.text = a_sData.ServiceName;
		m_introText.text = a_sData.ServiceIntroQuestion;
	}

	private void OnEnable()
	{
		m_exitButton.onClick.AddListener (OnFinishButtonPressed);
	}

	private void OnDisable()
	{
		m_exitButton.onClick.RemoveListener (OnFinishButtonPressed);
	}

	private void OnFinishButtonPressed()
	{
		UIManager.Instance.ShowPresentationView ();
	}
}
