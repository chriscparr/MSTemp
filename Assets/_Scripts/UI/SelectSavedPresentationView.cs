using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSavedPresentationView : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_savedButtonPrefab;
	[SerializeField]
	private GameObject m_deleteConfirmPanelPrefab;
	[SerializeField]
	private GameObject m_contentArea;
	[SerializeField]
	private Button m_closeButton;

	private List<SavedPresentationButton> m_scrollButtons = new List<SavedPresentationButton>();

	public void RefreshList()
	{
		if (m_scrollButtons.Count > 0)
		{
			ClearButtonDisplay ();
		}
		GenerateButtonDisplay ();
	}

	private void OnEnable()
	{
		m_closeButton.onClick.AddListener (OnCloseButtonPressed);
		GenerateButtonDisplay ();
	}

	private void OnDisable()
	{
		ClearButtonDisplay ();
		m_closeButton.onClick.RemoveListener (OnCloseButtonPressed);
	}

	private void ClearButtonDisplay()
	{
		foreach (SavedPresentationButton btn in m_scrollButtons)
		{
			btn.OnPresentationSelected -= OnPresentationSelectedEventHandler;
			btn.OnEditPresentation -= OnPresentationEditEventHandler;
			btn.OnDeletePresentation -= OnPresentationDeleteEventHandler;
			Destroy (btn.gameObject);
		}
		m_scrollButtons.Clear ();
	}

	private void GenerateButtonDisplay()
	{
		foreach (PresentationData pData in PersistentDataHandler.GetAllSavedPresentations())
		{
			GameObject btnObj = Instantiate<GameObject> (m_savedButtonPrefab, m_contentArea.transform);
			SavedPresentationButton btn = btnObj.GetComponent<SavedPresentationButton> ();
			m_scrollButtons.Add (btn);
			btn.Presentation = pData;
			btn.OnPresentationSelected += OnPresentationSelectedEventHandler;
			btn.OnEditPresentation += OnPresentationEditEventHandler;
			btn.OnDeletePresentation += OnPresentationDeleteEventHandler;
		}
	}

	private void OnCloseButtonPressed()
	{
		UIManager.Instance.ShowNewOrSavedView ();
	}

	private void OnPresentationSelectedEventHandler(PresentationData a_pData)
	{
		GameManager.Instance.SetupModel (a_pData);
		UIManager.Instance.ShowPresentationView ();
	}

	private void OnPresentationEditEventHandler(PresentationData a_pData)
	{
		UIManager.Instance.ShowNewPresentationView(a_pData);
	}

	private void OnPresentationDeleteEventHandler(PresentationData a_pData)
	{
		GameObject delPanelObj = Instantiate<GameObject> (m_deleteConfirmPanelPrefab, gameObject.transform);
		delPanelObj.GetComponent<ConfirmDeletePanel> ().Initialise (a_pData);
	}
}
