using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDeletePanel : MonoBehaviour 
{
	[SerializeField]
	private Button m_confirmButton;
	[SerializeField]
	private Button m_cancelButton;
	[SerializeField]
	private Button m_closeButton;
	[SerializeField]
	private Text m_warningText;

	private PresentationData m_pData;

	public void Initialise(PresentationData a_pData)
	{
		m_pData = a_pData;
		m_warningText.text = "Are you sure you want to delete " + m_pData.ClientName +"'s presentation? This can't be undone!";
	}

	private void OnEnable()
	{
		m_confirmButton.onClick.AddListener (RemovePresentation);
		m_cancelButton.onClick.AddListener (ClosePanel);
		m_closeButton.onClick.AddListener (ClosePanel);
	}

	private void OnDisable()
	{
		m_confirmButton.onClick.RemoveListener (RemovePresentation);
		m_cancelButton.onClick.RemoveListener (ClosePanel);
		m_closeButton.onClick.RemoveListener (ClosePanel);
	}

	private void RemovePresentation()
	{
		PersistentDataHandler.DeleteFile (m_pData.ID);
		ClosePanel ();
	}

	private void ClosePanel()
	{
		transform.parent.GetComponent<SelectSavedPresentationView> ().RefreshList ();
		gameObject.SetActive (false);
		Destroy (gameObject);
	}
}
