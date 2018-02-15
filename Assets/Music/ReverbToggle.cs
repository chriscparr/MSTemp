using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReverbToggle : MonoBehaviour {

    AudioReverbFilter filt;

    void Start()
    {
        filt = Camera.main.GetComponent<AudioReverbFilter>();
        if (GetComponent<Toggle>().isOn)
        {
            filt.enabled = true;
        }
    }

    public void OnValueChanged(bool b)
    {
        if (b)
        {
            filt.enabled = true;

        }
        else
        {
            filt.enabled = false;
        }
    }



}
