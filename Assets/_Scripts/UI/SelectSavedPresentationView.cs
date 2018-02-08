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

	private List<Button> m_scrollButtons = new List<Button>();

	private void OnPresentationOneButtonPressed()
	{
		GameManager.Instance.SetupModel ();
		UIManager.Instance.ShowPresentationView ();
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		if (PersistentDataHandler.GetJsonFilenames ().Length > 0)
		{
			SetupScrollView ();
		} 
		else
		{
			OnPresentationOneButtonPressed ();
		}
	}

	private void OnDisable()
	{
		foreach (Button btn in m_scrollButtons)
		{
			btn.onClick.RemoveAllListeners ();
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
			Button btn = btnObj.GetComponent<Button> ();
			m_scrollButtons.Add (btn);
			btn.GetComponentInChildren<Text> ().text = pData.ClientName;
			btn.onClick.AddListener (OnPresentationOneButtonPressed);
		}
	}
}
