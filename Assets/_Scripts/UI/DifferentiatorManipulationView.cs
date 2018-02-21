using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifferentiatorManipulationView : MonoBehaviour {

    public Text difName;
    public Toggle lockTog;

	// Use this for initialization
	void OnEnable () {
		if (CameraInputManager.Instance.SelectedCell!=null)
			difName.text = CameraInputManager.Instance.SelectedCell.ServiceDat.ServiceName;
        lockTog.isOn = false;
        LockSubcell(false);
	}
	
	// Update is called once per frame
	public void LockSubcell(bool val)
    {
        if (val != true)
        {
			CameraInputManager.Instance.SelectedCell.allowScaling = true;
        }
        else
        {
			CameraInputManager.Instance.SelectedCell.allowScaling = false; 
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
