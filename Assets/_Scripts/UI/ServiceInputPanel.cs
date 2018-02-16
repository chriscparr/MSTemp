using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServiceInputPanel : MonoBehaviour 
{
	public delegate void ServiceInputPanelStatus();
	public ServiceInputPanelStatus OnClosePanel;
	public delegate void ServiceInputAction(ServiceData a_sData);
	public ServiceInputAction OnSubmitService;

	[SerializeField]
	private Dropdown m_typeSelect;
	[SerializeField]
	private Slider m_weightingVal;
	[SerializeField]
	private InputField m_introText;
	[SerializeField]
	private Button m_submitBtn;
	[SerializeField]
	private Button m_closeBtn;
	[SerializeField]
	private Button m_addElementButton;
	[SerializeField]
	private Button m_removeElementButton;
	[SerializeField]
	private Text m_listDisplay;

	private const string DisplayPrefix = "Current Videos: [";
	private const string DisplaySuffix = "]";
	private List<string> m_videoPathsList = new List<string>();
	private VideoPicker m_videoPicker;

	private string[] m_serviceTypes = new string[] { 
		"FAST",
		"SHOP",
		"GROWTH",
		"DATA",
		"LOOP",
		"CONTENT",
		"AGILE",
		"LIFE"
	};

	private void Awake()
	{
		m_videoPicker = gameObject.AddComponent<VideoPicker> ();
	}

	private void Start () 
	{
		m_typeSelect.AddOptions (new List<string> (m_serviceTypes));
	}

	private void OnEnable()
	{
		m_submitBtn.onClick.AddListener (OnSubmitButton);
		m_closeBtn.onClick.AddListener (OnCloseButton);
		m_addElementButton.onClick.AddListener (AddVideoToList);
		m_removeElementButton.onClick.AddListener (RemoveVideoFromList);
		m_videoPicker.OnVideoSelected += VideoSelectedEventHandler;
	}

	private void OnDisable()
	{
		m_submitBtn.onClick.RemoveListener (OnSubmitButton);
		m_closeBtn.onClick.RemoveListener (OnCloseButton);
		m_addElementButton.onClick.RemoveListener (AddVideoToList);
		m_removeElementButton.onClick.RemoveListener (RemoveVideoFromList);
		m_videoPicker.OnVideoSelected -= VideoSelectedEventHandler;
	}

	private void VideoSelectedEventHandler(string a_videoPath)
	{
		m_videoPathsList.Add (a_videoPath);
		RefreshVideoListDisplay ();
	}

	private void AddVideoToList()
	{
		m_videoPicker.ShowVideoPicker ();
	}

	private void RemoveVideoFromList()
	{
		m_videoPathsList.RemoveAt (m_videoPathsList.Count-1);
		RefreshVideoListDisplay ();
	}

	private void RefreshVideoListDisplay()
	{
		StringBuilder stringList = new StringBuilder();
		stringList.Append (DisplayPrefix);
		for (int i = 0; i < m_videoPathsList.Count; i++)
		{
			stringList.Append (m_videoPathsList [i]);
			if (i != m_videoPathsList.Count - 1)
			{
				stringList.Append (",");
			}
		}
		stringList.Append (DisplaySuffix);
		m_listDisplay.text = stringList.ToString ();
	}

	private void OnSubmitButton()
	{
		ServiceData servData = new ServiceData ();
		servData.ServiceName = m_serviceTypes [m_typeSelect.value];
		servData.ServiceWeighting = m_weightingVal.value;
		servData.ServiceIntroText = m_introText.text;
		servData.ServiceVideoPaths = m_videoPathsList.ToArray ();

		if (OnSubmitService != null)
		{
			OnSubmitService (servData);
		}
		OnCloseButton ();
	}

	private void OnCloseButton()
	{
		gameObject.SetActive (false);
		Destroy (this.gameObject);
	}
}
