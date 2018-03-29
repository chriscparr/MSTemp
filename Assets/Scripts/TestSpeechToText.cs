using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TextSpeech;
using UnityEngine.SceneManagement;

public class TestSpeechToText : MonoBehaviour
{
    public Text wordOutput;

    void Start()
    {
        Setting("en-US");
        //loading.SetActive(false);
        SpeechToText.instance.onResultCallback = OnResultSpeech;
        AddLog("Unity Init Finish");
    }
    void AddLog(string log)
    {
        //txtLog.text += "\n" + log;
        Debug.Log(log);
    }
    public void StartRecording()
    {
#if UNITY_EDITOR
#else
        SpeechToText.instance.StartRecording("Speak any");
#endif
    }


    public void StopRecording()
    {
#if UNITY_EDITOR
        OnResultSpeech("Not support in editor.");
#else
        SpeechToText.instance.StopRecording();
#endif
    }
    void OnResultSpeech(string _data)
    {
        Debug.Log("NOW WEA RE OUTPUTTING WHATEVER WAS SAID, TO THE CONSOLE SO DEAL WITH IT");
        wordOutput.text = _data;
    }
    //public void OnClickSpeak()
    //{
    //    TextToSpeech.instance.StartSpeak(inputText.text);
    //}
    //public void  OnClickStopSpeak()
    //{
    //    TextToSpeech.instance.StopSpeak();
    //}
    public void Setting(string code)
    {
        // TextToSpeech.instance.Setting(code, pitch, rate);
        SpeechToText.instance.Setting(code);

    }
    public void OnClickApply()
    {
        // Setting(inputLocale.text);
    }

    void Update()
    {
        if (Input.touchCount == 3)
        {
            // Application.LoadLevel(1);
            SceneManager.LoadScene("SampleSpeechToText");
        }
    }
}
