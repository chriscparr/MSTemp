using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifferentiatorManipulationView : MonoBehaviour 
{

	[SerializeField]
	private Text m_serviceNameText;
	[SerializeField]
    public Toggle m_lockToggle;
	[SerializeField]
	private Button m_homeButton;


	// Use this for initialization
	void OnEnable () 
	{
		if (CameraInputManager.Instance.SelectedCell != null)
		{
			m_serviceNameText.text = CameraInputManager.Instance.SelectedCell.ServiceDat.ServiceName;
		}			
		m_lockToggle.onValueChanged.AddListener (LockSubcell);
		m_homeButton.onClick.AddListener (GoHome);
        m_lockToggle.isOn = false;
		LockSubcell(m_lockToggle.isOn);
	}
	

	private void LockSubcell(bool a_isLocked)
    {
		if (CameraInputManager.Instance.SelectedCell != null)
		{
			CameraInputManager.Instance.SelectedCell.CanScale = !a_isLocked;
		}
    }

	private void GoHome()
    {
        LockSubcell(false);
        CameraInputManager.Instance.ReleaseSubCellFromFocus(true);
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
		m_lockToggle.onValueChanged.RemoveListener (LockSubcell);
		m_homeButton.onClick.RemoveListener (GoHome);
    }

}
