using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSAddStuffBox : MonoBehaviour 
{
	public delegate void MSAddStuffBoxAction(string a_buttonValue);
	public MSAddStuffBoxAction OnSelected;
	public MSAddStuffBoxAction OnUnselected;

	[SerializeField]
	private Color m_msPurple;
	[SerializeField]
	private Color m_msWhite;
	[SerializeField]
	private Button m_button;
	[SerializeField]
	private Text m_labelText;
	[SerializeField]
	private Image m_iconImage;
	[SerializeField]
	private Sprite m_addIcon;
	[SerializeField]
	private Sprite m_removeIcon;

	private bool m_isToggled = false;
	private string m_buttonValue;

	public void SetButtonValue(string a_value)
	{
		m_buttonValue = a_value;
		m_labelText.text = a_value.ToUpper ();
	}

	private void Start()
	{
		InvertColors ();
		m_button.onClick.AddListener (InvertColors);
	}

	private void InvertColors()
	{
		if (m_isToggled)
		{
			m_button.image.color = m_msPurple;
			m_labelText.color = m_msWhite;
			m_iconImage.sprite = m_removeIcon;
			if (OnSelected != null)
			{
				OnSelected (m_buttonValue);
			}
		}
		else
		{
			m_button.image.color = m_msWhite;
			m_labelText.color = m_msPurple;
			m_iconImage.sprite = m_addIcon;
			if (OnUnselected != null)
			{
				OnUnselected (m_buttonValue);
			}
		}
		m_isToggled = !m_isToggled;
	}
}
