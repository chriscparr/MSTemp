using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;

public class TestTextToSpeech : MonoBehaviour {

    public Text SiriLines;

	// Use this for initialization
	void Start () {


        // just ignore this line.
        TextToSpeech.instance.Setting("en-GB", 0.5f,0.5f);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDone()
    {
        SpeechToText.instance.StartRecording();
    }

    public void SpeakLine()
    {
        SpeechToText.instance.ManuallyStopRecording();
        string lines = SiriLines.text;
        TextToSpeech.instance.StartSpeak(lines);
    }
}
