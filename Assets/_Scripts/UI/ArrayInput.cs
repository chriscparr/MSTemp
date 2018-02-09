using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrayInput : MonoBehaviour 
{
	[SerializeField]
	private Button m_addElementButton;
	[SerializeField]
	private Button m_removeElementButton;
	[SerializeField]
	private Button m_clearAllElementsButton;
	[SerializeField]
	private Text m_listDisplay;
	[SerializeField]
	private Text m_titleText;
	[SerializeField]
	private InputField m_inputElement;

	private const string DisplayPrefix = "Current Values: [";
	private const string DisplaySuffix = "]";

	private List<string> m_elements = new List<string>();
	public string[] InputElements {get { return m_elements.ToArray (); }}
	public string LabelText {get { return m_titleText.text; } set { m_titleText.text = value; }}

	private void OnEnable()
	{
		m_addElementButton.onClick.AddListener (AddElement);
		m_removeElementButton.onClick.AddListener (RemoveElement);
		m_clearAllElementsButton.onClick.AddListener (ClearAllElements);
		m_elements.Clear ();
		RefreshListDisplay ();
		m_inputElement.text = "";
	}

	private void OnDisable()
	{
		m_addElementButton.onClick.RemoveListener (AddElement);
		m_removeElementButton.onClick.RemoveListener (RemoveElement);
		m_clearAllElementsButton.onClick.RemoveListener (ClearAllElements);
	}

	private void AddElement()
	{
		m_elements.Add (m_inputElement.text);
		m_inputElement.text = "";
		RefreshListDisplay ();
	}

	private void RemoveElement()
	{
		m_elements.RemoveAt (m_elements.Count-1);
		RefreshListDisplay ();
	}

	private void ClearAllElements()
	{
		m_elements.Clear ();
		RefreshListDisplay ();
	}

	private void RefreshListDisplay()
	{
		StringBuilder stringList = new StringBuilder();
		stringList.Append (DisplayPrefix);
		for (int i = 0; i < m_elements.Count; i++)
		{
			stringList.Append (m_elements [i]);
			if (i != m_elements.Count - 1)
			{
				stringList.Append (",");
			}
		}
		stringList.Append (DisplaySuffix);
		m_listDisplay.text = stringList.ToString ();

		/*
		 * 
		if (m_elements.Count > 0)
		{
			StringBuilder stringList = new StringBuilder();
			stringList.Append (DisplayPrefix);
			for (int i = 0; i < m_elements.Count; i++)
			{
				stringList.Append (m_elements [i]);
				if (i != m_elements.Count - 1)
				{
					stringList.Append (",");
				}
			}
			stringList.Append (DisplaySuffix);
			m_listDisplay.text = stringList.ToString ();
		}
		else
		{
			m_listDisplay.text = DisplayPrefix + DisplaySuffix;
		}
		
		*/
	}
}
