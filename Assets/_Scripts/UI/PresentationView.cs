using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentationView : MonoBehaviour 
{
	[SerializeField]
	private Button m_finishedButton;

    [SerializeField]
    private Button m_saveButton;

	private void OnFinishedButtonPressed()
	{
		ModelManager.Instance.ClearModel ();
		UIManager.Instance.ShowEndPresentationView ();
        CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.SetupPhase);

        if (ConnectionGenerator.Instance != null)
        {
            ConnectionGenerator.Instance.DestroyAll();
        }
        if (PDFManager.Instance != null)
        {
            PDFManager.Instance.DestroyAll();
        }
		gameObject.SetActive(false);
	}

    private void OnSaveButtonPressed()
    {
        UIManager.Instance.ShowForwardSummaryView();
        gameObject.SetActive(false);
    }

	private void OnEnable()
	{
		m_finishedButton.onClick.AddListener (OnFinishedButtonPressed);
        m_saveButton.onClick.AddListener(OnSaveButtonPressed);
        // ModelManager.Instance.ShakeModel ();
	}

	private void OnDisable()
	{
		m_finishedButton.onClick.RemoveListener (OnFinishedButtonPressed);
        m_saveButton.onClick.RemoveListener(OnSaveButtonPressed);
	}
}
