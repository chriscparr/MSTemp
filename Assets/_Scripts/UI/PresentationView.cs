using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentationView : MonoBehaviour 
{
	[SerializeField]
	private Button m_finishedButton;

	private void OnFinishedButtonPressed()
	{
		ModelManager.Instance.ClearModel ();
		UIManager.Instance.ShowEndPresentationView ();
        CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.SetupPhase);
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		m_finishedButton.onClick.AddListener (OnFinishedButtonPressed);
		ModelManager.Instance.ShakeModel ();
	}

	private void OnDisable()
	{
		m_finishedButton.onClick.RemoveListener (OnFinishedButtonPressed);
	}
}
