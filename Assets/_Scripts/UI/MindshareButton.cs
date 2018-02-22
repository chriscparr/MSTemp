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

	public void SetButtonValue(string a_value)
	{
		m_buttonValue = a_value;
		m_labelText.text = a_value.ToUpper ();
	}

	private void Start()
	{
		m_button.image.color = m_msWhite;
		m_labelText.color = m_msPurple;
		m_button.onClick.AddListener (InvertColors);
	}

	private void InvertColors()
	{
		if (m_isToggled)
		{
			m_button.image.color = m_msPurple;
			m_labelText.color = m_msWhite;
			if (OnSelected != null)
			{
				OnSelected (m_buttonValue);
			}
		}
		else
		{
			m_button.image.color = m_msWhite;
			m_labelText.color = m_msPurple;
			if (OnUnselected != null)
			{
				OnUnselected (m_buttonValue);
			}
		}
		m_isToggled = !m_isToggled;
	}
}
