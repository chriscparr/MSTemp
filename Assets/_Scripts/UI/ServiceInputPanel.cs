using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServiceInputPanel : MonoBehaviour 
{
	public delegate void ServiceInputPanelStatus();
	public ServiceInputPanelStatus OnClosePanel;
	public delegate void ServiceInputAction(ServiceData a_sData);
	public ServiceInputAction OnSubmitService;

	[SerializeField]
	private Dropdown m_typeSelect;
	[SerializeField]
	private Slider m_weightingVal;
	[SerializeField]
	private InputField m_introText;
	[SerializeField]
	private Button m_submitBtn;
	[SerializeField]
	private Button m_closeBtn;

	private string[] m_serviceTypes = new string[] { 
		"FAST",
		"SHOP",
		"GROWTH",
		"DATA",
		"LOOP",
		"CONTENT",
		"AGILE",
		"LIFE"
	};

	private void OnEnable()
	{
		m_submitBtn.onClick.AddListener (OnSubmitButton);
		m_closeBtn.onClick.AddListener (OnCloseButton);
	}

	private void OnDisable()
	{
		m_submitBtn.onClick.RemoveListener (OnSubmitButton);
		m_closeBtn.onClick.RemoveListener (OnCloseButton);
	}


	private void Start () 
	{
		m_typeSelect.AddOptions (new List<string> (m_serviceTypes));
	}

	private void OnSubmitButton()
	{
		ServiceData servData = new ServiceData ();
		servData.ServiceName = m_serviceTypes [m_typeSelect.value];
		servData.ServiceWeighting = m_weightingVal.value;
		servData.ServiceIntroText = m_introText.text;
		servData.ServiceVideoPaths = new string[] {};

		if (OnSubmitService != null)
		{
			OnSubmitService (servData);
		}

		OnCloseButton ();
	}

	private void OnCloseButton()
	{
		gameObject.SetActive (false);
		Destroy (this.gameObject);
	}
}
