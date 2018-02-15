using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PitchSlider : MonoBehaviour {
    AudioSource source;
    public Slider mySlider;
    //void Start()
    //{
    //    mySlider = GetComponent<Slider>();
    //    source = Camera.main.gameObject.GetComponent<AudioSource>();
    //}
    //public void OnValueChanged()
    //{
    //    source.pitch = mySlider.value;
    //}

    //public void ChangeState(CameraInputManager.Phase phase, float wantedValue)
    //{
    //    iTween.ValueTo(this.gameObject, iTween.Hash("from", mySlider.value, "to", wantedValue, "time", 3, "onupdate", "valueTween"));
    //}

    //void valueTween(float val)
    //{
    //    mySlider.value = val;
    //    source.pitch = val;
    //}

    //public void overrideClip(AudioSource csource)
    //{
    //    source = csource;
    //}
}
