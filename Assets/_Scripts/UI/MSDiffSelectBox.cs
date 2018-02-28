using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSDiffSelectBox : MonoBehaviour 
{
	public delegate void DifferentiatorSelectionAction(string a_buttonValue);
	public DifferentiatorSelectionAction OnSelected;

	public ServiceData SavedServiceData {get{ return m_serviceData; }set{ SetServiceData (value); }}
	private ServiceData m_serviceData;
	public bool IsServiceDataReady {get{ return m_isDataReady; }}
	private bool m_isDataReady = false;

	[SerializeField]
	private Color m_msPurple;
	[SerializeField]
	private Color m_msWhite;
	[SerializeField]
	private Button m_button;
	[SerializeField]
	private Text m_labelText;

	private bool m_isToggled = false;
	private string m_buttonValue;

	public void SetButtonValue(string a_value, bool a_isPreToggled = false)
	{
		m_buttonValue = a_value;
		m_labelText.text = a_value.ToUpper ();
		m_isToggled = a_isPreToggled;
		UpdateColors ();
	}

	private void SetServiceData(ServiceData a_servData)
	{
		m_serviceData = a_servData;
		m_isDataReady = true;
		m_isToggled = true;
		UpdateColors ();
	}

	private void Start()
	{
		m_button.onClick.AddListener (OnButtonPressed);
		UpdateColors ();
	}

	private void OnButtonPressed()
	{
		if (OnSelected != null)
		{
			OnSelected (m_buttonValue);
		}
	}

	private void UpdateColors()
	{
		if (m_isToggled)
		{
			m_button.image.color = m_msPurple;
			m_labelText.color = m_msWhite;
		}
		else
		{
			m_button.image.color = m_msWhite;
			m_labelText.color = m_msPurple;
		}
	}
}
