using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSNotesPanel : MonoBehaviour 
{
	public delegate void NotesPanelAction();
	public NotesPanelAction OnCloseRequest;

	[SerializeField]
	private Button m_closeButton;
	[SerializeField]
	private InputField m_inputField;

	public string InputText {get{ return m_inputField.text;}set{ m_inputField.text = value;}}

	private void Awake ()
	{
		m_closeButton.onClick.AddListener (ClosePanel);
	}

	private void ClosePanel()
	{
		if (OnCloseRequest != null)
		{
			OnCloseRequest ();
		}
	}
}
