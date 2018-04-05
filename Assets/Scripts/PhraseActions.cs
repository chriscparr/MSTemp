using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;

public class PhraseActions : MonoBehaviour {

    public static PhraseActions Instance { get { return s_instance; } }
    private static PhraseActions s_instance = null;

    public string fallBackString = "I can't do that. Let's try something else.";

	// Use this for initialization
	void Awake () {
        s_instance = this;
	}

    private void Start()
    {
        TextToSpeech.instance.onDoneCallback = OnActionComplete;
    }

    void OnActionComplete()
    {
        SpeechToText.instance.StartRecording();
    }

    // Update is called once per frame
    public void ActionToFollowPhrase (string _data) {
        
        // such a fucking bad way of dealing with this
        // but you can't do a switch statement using string.Contains so... ?

        if (_data.Contains("credits") || _data.Contains("who made you") || _data.Contains("who built you"))
        {
            SpeechToText.instance.ManuallyStopRecording();
            TextToSpeech.instance.StartSpeak("I was made by Ali Collins and Chris Parr of Mindshare");
            return;
        }

        if (_data.Contains("save PDF") || _data.Contains("export presentation") || _data.Contains("save presentation"))
        {
            if (CameraInputManager.Instance.m_CurrentPhase == CameraInputManager.Phase.MainCellPhase)
            {
                SpeechToText.instance.ManuallyStopRecording();
                TextToSpeech.instance.StartSpeak("Okay, let's save your presentation as a PDF");
                StartCoroutine("WaitAndPDF");
                return;
            }
            else
            {
                SpeechToText.instance.ManuallyStopRecording();
                TextToSpeech.instance.StartSpeak(fallBackString);
            }
        }

        if (_data.Contains("go to fast") || _data.Contains("take me to fast") || _data.Contains("show me fast"))
        {
            if (CameraInputManager.Instance.m_CurrentPhase == CameraInputManager.Phase.MainCellPhase)
            {
                SpeechToText.instance.ManuallyStopRecording();
                TextToSpeech.instance.StartSpeak("Fast service. Here, I will say the long descriptor for fast.");

                Subcell[] tempCells = ModelManager.Instance.GetAllSubCells();

                for (int i = 0; i < tempCells.Length; i++)
                {
                    // YOU CAN USE A SWITCH STATEMENT HERE THO
                    if (tempCells[i].ServiceDat.ServiceName == "FAST")
                    {
                        CameraInputManager.Instance.FocusOnSubCell(tempCells[i]);
                        break;
                    }
                }
                return;
            }
            else
            {
                SpeechToText.instance.ManuallyStopRecording();
                TextToSpeech.instance.StartSpeak(fallBackString);
                return;
            }
        }

        if (_data.Contains("go back") || _data.Contains("take me back") || _data.Contains("return to center"))
        {
            if (CameraInputManager.Instance.m_CurrentPhase == CameraInputManager.Phase.FocusedSubCellPhase)
            {
                SpeechToText.instance.ManuallyStopRecording();
                TextToSpeech.instance.StartSpeak("Okay, let's go back to the start.");
                CameraInputManager.Instance.ResetPosition(true);
                UIManager.Instance.ShowPresentationView();
                return;
            }
            else
            {
                SpeechToText.instance.ManuallyStopRecording();
                TextToSpeech.instance.StartSpeak(fallBackString);
                return;
            }
        }

	}

    IEnumerator WaitAndPDF()
    {
        yield return new WaitForSeconds(3);
        PDFManager.Instance.DoPDF();
    }
}
