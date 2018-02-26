using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MindshareButton : MonoBehaviour 
{
	public delegate void MindshareButtonAction(string a_buttonValue);
	public MindshareButtonAction OnSelected;
	public MindshareButtonAction OnUnselected;

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

	private void Start()
	{
		m_button.onClick.AddListener (OnToggle);
		UpdateColors ();
	}

	private void OnToggle()
	{
		if (!m_isToggled)
		{
			if (OnSelected != null)
			{
				OnSelected (m_buttonValue);
			}
		}
		else
		{
			if (OnUnselected != null)
			{
				OnUnselected (m_buttonValue);
			}
		}
		m_isToggled = !m_isToggled;
		UpdateColors ();
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
