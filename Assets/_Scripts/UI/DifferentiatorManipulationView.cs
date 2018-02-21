using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifferentiatorManipulationView : MonoBehaviour {

    public Text difName;
    public Toggle lockTog;

	// Use this for initialization
	void OnEnable () {
        if (CameraInputManager.Instance.m_selectedCell!=null)
        difName.text = CameraInputManager.Instance.m_selectedCell.ServiceDat.ServiceName;
        lockTog.isOn = false;
        LockSubcell(false);
	}
	
	// Update is called once per frame
	public void LockSubcell(bool val)
    {
        if (val != true)
        {
            CameraInputManager.Instance.m_selectedCell.allowScaling = true;
        }
        else
        {
            CameraInputManager.Instance.m_selectedCell.allowScaling = false; 
        }
       
    }

    public void GoHome()
    {
        LockSubcell(false);
        CameraInputManager.Instance.ReleaseSubCellFromFocus(true);
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        Debug.LogError("DISABLING ME");
    }

}
