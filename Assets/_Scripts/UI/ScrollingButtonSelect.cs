using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingButtonSelect : MonoBehaviour 
{
	public delegate void ScrollingButtonSelectAction();
	public ScrollingButtonSelectAction OnCloseRequest;

	[SerializeField]
	private GameObject m_msButtonPrefab;
	[SerializeField]
	private GameObject m_contentArea;
	[SerializeField]
	private Button m_exitScrollSelectClickArea;

	private List<MindshareButton> m_optionButtons;

	private List<string> m_selectedOptions = new List<string> ();
	public string[] SelectedOptions {get{ return m_selectedOptions.ToArray();}}

	public void Initialise(string[] a_options, string[] a_previouslySelectedOptions)
	{
		m_optionButtons = new List<MindshareButton> ();
		if (a_previouslySelectedOptions != null && a_previouslySelectedOptions.Length > 0)
		{
			List<string> prevSelected = new List<string> (a_previouslySelectedOptions);
			foreach (string opt in a_options)
			{
				GameObject btnObj = Instantiate<GameObject>(m_msButtonPrefab, m_contentArea.transform);
				MindshareButton msBtn = btnObj.GetComponent<MindshareButton> ();
				if (prevSelected.Contains (opt))
				{
					msBtn.SetButtonValue (opt, true);
				} 
				else
				{
					msBtn.SetButtonValue (opt,false);
				}
				msBtn.OnSelected += OnOptionSelected;
				msBtn.OnUnselected += OnOptionUnselected;
				m_optionButtons.Add (msBtn);
			}
		} 
		else
		{
			foreach (string opt in a_options)
			{
				GameObject btnObj = Instantiate<GameObject>(m_msButtonPrefab, m_contentArea.transform);
				MindshareButton msBtn = btnObj.GetComponent<MindshareButton> ();
				msBtn.SetButtonValue (opt);
				msBtn.OnSelected += OnOptionSelected;
				msBtn.OnUnselected += OnOptionUnselected;
				m_optionButtons.Add (msBtn);
			}
		}
		m_exitScrollSelectClickArea.onClick.AddListener (CloseRequest);
		iTween.MoveFrom (gameObject, iTween.Hash ("position",transform.position - new Vector3(0f, 200f, 0f),"time",0.5f,"easetype",iTween.EaseType.linear));
	}

	public void AnimateExit()
	{
		iTween.MoveTo (gameObject, iTween.Hash ("position",transform.position - new Vector3(0f, 200f, 0f),"time",0.5f,"easetype",iTween.EaseType.linear, "oncompletetarget", gameObject, "oncomplete", "DestroyAfterExit"));
	}

	private void DestroyAfterExit()
	{
		Destroy (gameObject);
	}

	private void OnOptionSelected(string a_selected)
	{
		m_selectedOptions.Add (a_selected);
	}

	private void OnOptionUnselected(string a_unSelected)
	{
		m_selectedOptions.Remove (a_unSelected);
	}

	private void CloseRequest()
	{
		//Need to request to close this panel so that the parent view can grab our selected options whilst this still exists.
		if (OnCloseRequest != null)
		{
			OnCloseRequest ();
		}
	}

	private void OnDestroy()
	{
		m_exitScrollSelectClickArea.onClick.RemoveListener (CloseRequest);
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
