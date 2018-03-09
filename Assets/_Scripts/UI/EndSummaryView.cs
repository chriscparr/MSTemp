using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSummaryView : MonoBehaviour {

    // actually use events and listeners here, use Chris' previous scripts if u wanna learn how

    public Text AddressTo;
    public Text AddressCC;
    public Text SubjectLine;

    public CanvasGroup thisCanvas;

    [SerializeField]
    private Button SaveButton;
    [SerializeField]
    private Button SendButton;
    [SerializeField]
    private Button HomeButton;

    // look at dis Chris look ma listeners
    // maybe i do maybe i dont
    private void OnEnable()
    {
        SaveButton.onClick.AddListener(OnSaveButtonClick);
        SendButton.onClick.AddListener(OnSendButtonClick);
        HomeButton.onClick.AddListener(OnHomeButtonPressed);
                  thisCanvas = GetComponent<CanvasGroup>();
        thisCanvas.alpha = 1;
    }

    private void OnDisable()
    {
        SaveButton.onClick.RemoveListener(OnSaveButtonClick);
        SendButton.onClick.RemoveListener(OnSendButtonClick);
        HomeButton.onClick.RemoveListener(OnHomeButtonPressed);
    }

    private void OnHomeButtonPressed()
    {
        ModelManager.Instance.ClearModel();
        UIManager.Instance.ShowEndPresentationView();
        CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.SetupPhase);

        if (ConnectionGenerator.Instance != null)
        {
            ConnectionGenerator.Instance.DestroyAll();
        }

        gameObject.SetActive(false);
    }

    private void OnSaveButtonClick()
    {
        thisCanvas.alpha = 0;
        PDFManager.Instance.StartCoroutine("GenerateEntirePDF");
        OnHomeButtonPressed();
        // do the usual save pdf and open with stuff here
    }

    private void OnSendButtonClick()
    {
        // lol crap check to make sure we've put in a proper email address
        if (!AddressTo.text.Contains("@"))
        {
            Debug.Log("Invalid email address! Make an actual UI popup for this error and escape");
            return;
        }
        thisCanvas.alpha = 0;
        string[] emailStrings = new string[3] { AddressTo.text, AddressCC.text, SubjectLine.text };
        PDFManager.Instance.emailStrings = emailStrings;
        PDFManager.Instance.StartCoroutine("GenerateEntirePDF");
        OnHomeButtonPressed();
        // new function - save the pdf, but deliberately open an email client with the subject and addresses pre-populated.
    }
}
