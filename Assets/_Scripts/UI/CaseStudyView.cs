using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class CaseStudyView : MonoBehaviour 
{
	[SerializeField]
	private Button m_nextButton;
	[SerializeField]
	private Button m_previousButton;
	[SerializeField]
	private Button m_homeButton;
	[SerializeField]
	private Button m_introNextButton;
	[SerializeField]
	private Button m_contentNextButton;
	[SerializeField]
	private GameObject m_introPanel;
	[SerializeField]
	private Text m_titleText;
	[SerializeField]
	private Text m_introQuestionText;
	[SerializeField]
	private GameObject m_contentPanel;
	[SerializeField]
	private Image m_bodyTextBG;
	[SerializeField]
	private Text m_bodyText;
	[SerializeField]
	private GameObject m_videoPlaceholder;
	[SerializeField]
	private Button m_videoBG;
	[SerializeField]
	private VideoPlayer m_videoPlayer;
	[SerializeField]
	private CanvasGroup m_pauseIconGroup;
	[SerializeField]
	private CanvasGroup m_playIconGroup;

	private OnRailsMovement m_ourMover;
	private CanvasGroup m_introCanvas;
	private CanvasGroup m_contentCanvas;

	private CaseStudyData m_csData;

	public void DisplayCaseStudy(CaseStudyData a_caseData)
	{
		m_csData = a_caseData;
		ClearCaseStudy ();
		m_titleText.text = m_csData.TitleText;
		m_introQuestionText.text = m_csData.IntroText;

		if (m_csData.CaseStudyType == "TEXT")
		{
			m_bodyTextBG.gameObject.SetActive (true);
			m_bodyText.gameObject.SetActive (true);
			m_bodyText.text = a_caseData.BodyText;
			m_videoBG.gameObject.SetActive (false);
			m_videoPlaceholder.SetActive (false);
		} 
		if (m_csData.CaseStudyType == "VIDEO")
		{
			m_bodyText.gameObject.SetActive (false);
			m_bodyTextBG.gameObject.SetActive (false);
			m_videoBG.gameObject.SetActive (true);
			m_videoPlaceholder.SetActive (true);
		}
		ShowIntroPanel ();
	}

	private void ShowIntroPanel()
	{
		m_contentPanel.SetActive (false);
		iTween.ValueTo (gameObject, iTween.Hash ("from", 0f, "to", 1f, "time", 1f,"delay",0f, "onupdate", "ApplyIntroPanelAlpha"));
	}

	private void ShowContentPanel()
	{
		m_contentPanel.SetActive (true);
		iTween.ValueTo (gameObject, iTween.Hash ("from", 1f, "to", 0f, "time", 1f,"delay",0f, "onupdate", "ApplyIntroPanelAlpha", "oncomplete", "AfterIntroPanelTween"));
		iTween.ValueTo (gameObject, iTween.Hash ("from", 0f, "to", 1f, "time", 1f,"delay",1f, "onupdate", "ApplyContentPanelAlpha", "oncomplete", "AfterMainPanelTween"));
	}

	private void AfterIntroPanelTween()
	{
		Debug.Log ("AfterIntroPanelTween()");
		m_introPanel.SetActive (false);
	}

	private void AfterMainPanelTween()
	{
		Debug.Log ("AfterMainPanelTween()");
		if (m_csData.CaseStudyType == "VIDEO")
		{
			m_videoPlayer.url = m_csData.VideoPath;
			m_videoPlayer.prepareCompleted += OnPrepareComplete;
			m_videoPlayer.Prepare ();
		}
	}

	private void OnPrepareComplete(VideoPlayer a_vPlayer)
	{
		m_videoPlayer.prepareCompleted -= OnPrepareComplete;
		Debug.Log ("Video Prepared!");
		//PlayVideo ();
	}

	private void FinishCaseStudy()
	{
		Debug.Log ("FinishCaseStudy()");
		ClearCaseStudy();
		GoToNextCaseStudy();
	}

	public void ClearCaseStudy()
	{
		m_contentPanel.SetActive (true);
		m_introPanel.SetActive (true);
		m_contentCanvas.alpha = 0f;
		m_introCanvas.alpha = 0f;
	}

	public void PlayVideo()
	{
		Debug.Log("Attempting to play video!");
		m_videoPlayer.source = VideoSource.Url;

		AudioManager.Instance.Pause();
		AudioManager.Instance.AddVideoAudio();

		m_videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

		AudioSource tempAud = AudioManager.Instance.vidSauce;
		m_videoPlayer.controlledAudioTrackCount = 1;
		m_videoPlayer.EnableAudioTrack(0, true);

		m_videoPlayer.SetTargetAudioSource(0, tempAud);
		tempAud.volume = 0;
		AudioManager.Instance.StartCoroutine("FadeIn", tempAud);
		m_videoPlayer.Play();
	}

	private void PlayPauseVideo()
	{
		if (m_videoPlayer.isPlaying)
		{
			m_videoPlayer.Pause ();
			iTween.ValueTo (gameObject, iTween.Hash ("from", 1f, "to", 0f, "time", 0.5f, "onupdate", "ApplyPauseIconAlpha"));
		}
		else
		{
			PlayVideo ();
			iTween.ValueTo (gameObject, iTween.Hash ("from", 1f, "to", 0f, "time", 0.5f, "onupdate", "ApplyPlayIconAlpha"));
		}
	}

	private void Start () 
	{
		m_ourMover = Camera.main.GetComponent<OnRailsMovement>();
		m_introCanvas = m_introPanel.GetComponent<CanvasGroup> ();
		m_contentCanvas = m_contentPanel.GetComponent<CanvasGroup> ();
	}
	
	private void OnEnable()
	{
		m_nextButton.onClick.AddListener (GoToNextCaseStudy);
		m_previousButton.onClick.AddListener (GoToPreviousCaseStudy);
		m_homeButton.onClick.AddListener (GoHome);
		m_introNextButton.onClick.AddListener (ShowContentPanel);
		m_contentNextButton.onClick.AddListener (GoToNextCaseStudy);
		m_videoBG.onClick.AddListener (PlayPauseVideo);
	}
	
	private void OnDisable()
	{
		m_nextButton.onClick.RemoveListener (GoToNextCaseStudy);
		m_previousButton.onClick.RemoveListener (GoToPreviousCaseStudy);
		m_homeButton.onClick.RemoveListener (GoHome);
		m_introNextButton.onClick.RemoveListener (ShowContentPanel);
		m_contentNextButton.onClick.RemoveListener (GoToNextCaseStudy);
		m_videoBG.onClick.RemoveListener (PlayPauseVideo);
	}
		
	public void GoToNextCaseStudy () 
	{
		ClearCaseStudy ();
		m_ourMover.GoToNextPoint();
	}

	private void GoToPreviousCaseStudy() 
	{
		ClearCaseStudy ();
        m_ourMover.GoToPreviousPoint();
    }

	private void GoHome() 
	{
		ClearCaseStudy ();
		m_ourMover.GoHome ();
        gameObject.SetActive(false);
    }

	private void ApplyIntroPanelAlpha(float _alpha)
	{
		m_introCanvas.alpha = _alpha;
	}

	private void ApplyContentPanelAlpha(float _alpha)
	{
		m_contentCanvas.alpha = _alpha;
	}

	private void ApplyPlayIconAlpha(float _alpha)
	{
		m_playIconGroup.alpha = _alpha;
	}

	private void ApplyPauseIconAlpha(float _alpha)
	{
		m_pauseIconGroup.alpha = _alpha;
	}
}
