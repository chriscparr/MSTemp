using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSAddStuffBox : MonoBehaviour 
{
	public delegate void MSAddStuffBoxAction();
	public MSAddStuffBoxAction OnPressed;

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

	public bool IsToggled {get{ return m_isToggled;}set{ m_isToggled = value; UpdateColors ();}}
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
		m_button.onClick.AddListener (OnButtonPressed);
		UpdateColors ();
	}

	private void OnButtonPressed()
	{
		if (OnPressed != null)
		{
			OnPressed ();
		}
	}

	private void UpdateColors()
	{
		if (m_isToggled)
		{
			m_button.image.color = m_msPurple;
			m_labelText.color = m_msWhite;
			m_iconImage.sprite = m_removeIcon;
		}
		else
		{
			m_button.image.color = m_msWhite;
			m_labelText.color = m_msPurple;
			m_iconImage.sprite = m_addIcon;
		}
	}

}
