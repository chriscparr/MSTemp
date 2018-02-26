using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingButtonSelect : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_msButtonPrefab;
	[SerializeField]
	private GameObject m_contentArea;

	private List<MindshareButton> m_optionButtons;

	private List<string> m_selectedOptions = new List<string> ();
	public string[] SelectedOptions {get{ return m_selectedOptions.ToArray();}}

	public void Initialise(string[] a_selectOptions)
	{
		m_optionButtons = new List<MindshareButton> ();
		foreach (string opt in a_selectOptions)
		{
			GameObject btnObj = Instantiate<GameObject>(m_msButtonPrefab, m_contentArea.transform);
			MindshareButton msBtn = btnObj.GetComponent<MindshareButton> ();
			msBtn.SetButtonValue (opt);
			msBtn.OnSelected += OnOptionSelected;
			msBtn.OnUnselected += OnOptionUnselected;
			m_optionButtons.Add (msBtn);
		}
	}

	private void OnOptionSelected(string a_selected)
	{
		m_selectedOptions.Add (a_selected);
	}

	private void OnOptionUnselected(string a_unSelected)
	{
		m_selectedOptions.Remove (a_unSelected);
	}

	private void OnDestroy()
	{
		if (m_optionButtons.Count > 0)
		{
			foreach (MindshareButton msBtn in m_optionButtons)
			{
				msBtn.OnSelected -= OnOptionSelected;
				msBtn.OnUnselected -= OnOptionUnselected;
			}
			m_optionButtons.Clear ();
		}
	}
}
