using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {
    AudioSource source;
    public Slider mySlider;
    void Start()
    {
        mySlider = GetComponent<Slider>();
        source = Camera.main.gameObject.GetComponent<AudioSource>();
        source.volume = mySlider.value;
    }
    public void OnValueChanged()
    {
        source.volume = mySlider.value;
    }

    public void ChangeState(CameraInputManager.Phase phase, float wantedValue)
    {
        if (AudioManager.Instance.gradual.isOn)
        {
            iTween.ValueTo(this.gameObject, iTween.Hash("from", mySlider.value, "to", wantedValue, "time", 5, "onupdate", "valueTween"));
        }
        else
        {
            source.volume = wantedValue;
            mySlider.value = wantedValue;
        }
    }

    void valueTween(float val)
    {
        mySlider.value = val;
        source.volume = val;
    }

    public void overrideClip(AudioSource csource)
    {
        source = csource;
    }
}
