using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSavedPresentationView : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_savedButtonPrefab;
	[SerializeField]
	private GameObject m_contentArea;

	private List<SavedPresentationButton> m_scrollButtons = new List<SavedPresentationButton>();

	private void OnEnable()
	{
		SetupScrollView ();
	}

	private void OnDisable()
	{
		foreach (SavedPresentationButton btn in m_scrollButtons)
		{
			btn.OnPresentationSelected -= OnPresentationSelectedEventHandler;
			Destroy (btn.gameObject);
		}
		m_scrollButtons.Clear ();
	}

	private void SetupScrollView()
	{
		float buttonHeight = 120f;
		m_contentArea.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (buttonHeight * PersistentDataHandler.GetJsonFilenames().Length));
		foreach (PresentationData pData in PersistentDataHandler.GetAllSavedPresentations())
		{
			GameObject btnObj = Instantiate<GameObject> (m_savedButtonPrefab, m_contentArea.transform);
			SavedPresentationButton btn = btnObj.GetComponent<SavedPresentationButton> ();
			m_scrollButtons.Add (btn);
			btn.Presentation = pData;
			btn.OnPresentationSelected += OnPresentationSelectedEventHandler;
		}
	}

	private void OnPresentationSelectedEventHandler(PresentationData a_pData)
	{
		GameManager.Instance.SetupModel (a_pData);
		UIManager.Instance.ShowPresentationView ();
		gameObject.SetActive(false);
	}
}
