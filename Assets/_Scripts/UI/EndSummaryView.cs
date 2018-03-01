using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSummaryView : MonoBehaviour {

    // actually use events and listeners here, use Chris' previous scripts if u wanna learn how

    public Text AddressTo;
    public Text AddressCC;
    public Text SubjectLine;

    [SerializeField]
    private Button SaveButton;
    [SerializeField]
    private Button SendButton;

    private void OnEnable()
    {
        SaveButton.onClick.AddListener(OnSaveButtonClick);
        SendButton.onClick.AddListener(OnSendButtonClick);
    }

    private void OnDisable()
    {
        SaveButton.onClick.RemoveListener(OnSaveButtonClick);
        SendButton.onClick.RemoveListener(OnSendButtonClick);
    }

    private void OnSaveButtonClick()
    {
        PDFManager.Instance.StartCoroutine("GenerateEntirePDF");
        // do the usual save pdf and open with stuff here
    }

    private void OnSendButtonClick()
    {
        if (!AddressTo.text.Contains("@"))
        {
            Debug.Log("Invalid email address! Make an actual UI popup for this error and escape");
            return;
        }
        string[] emailStrings = new string[3] { AddressTo.text, AddressCC.text, SubjectLine.text };
        PDFManager.Instance.emailStrings = emailStrings;
        PDFManager.Instance.StartCoroutine("GenerateEntirePDF");
        // new function - save the pdf, but deliberately open an email client with the subject and addresses pre-populated.
    }
}
