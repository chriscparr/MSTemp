using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using System.Linq;

public class VideoManager : MonoBehaviour {

    // THIS IS FOR VERSION 1 FOR MARCH 9TH - FOR VERSION 2, MAKE THIS SHIT MORE ADVANCED (PUT VIDEO URLS IN SQL DATABASE MAYB? OR XML)
    // TODO
    // this is probably all useless shit anyway?

    // UPDATE - THIS IS shit and doesnt work


    public static VideoManager Instance { get { return s_instance; } }
    private static VideoManager s_instance = null;

    List<string> m_CurrentFiles = new List<string>();

    public List<VideoClip> m_FastVideos = new List<VideoClip>();
    public List<string> m_ShopVideos = new List<string>();
    public List<string> m_ContentVideos = new List<string>();

    void Awake() {
        s_instance = this;
    }

    void Start()
    {

        var URLs = Resources.LoadAll("FAST");
        List<VideoClip> vids = new List<VideoClip>();
        foreach (var s in URLs)
        {
            m_FastVideos.Add(s as VideoClip);
            Debug.Log("ADDING VID");
        }


        foreach (VideoClip s in m_FastVideos)
        {
            print("FAST VID: " + s.name);
        }

        foreach (string s in m_ContentVideos)
        {
            print("CONTENT VID: " + s);
        }

        foreach (string s in m_ShopVideos)
        {
            print("SHOP VID " + s);
        }

        var videoPlayer = Camera.main.gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
    }

    public void PlayVideo(VideoClip vidClip)
    {
        var videoPlayer = Camera.main.gameObject.GetComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = vidClip;
        // videoPlayer.url = vidURL;
        AudioManager.Instance.Pause();
        AudioManager.Instance.AddVideoAudio();

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
  
        AudioSource tempAud = AudioManager.Instance.vidSauce;
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.EnableAudioTrack(0, true);

        videoPlayer.SetTargetAudioSource(0, tempAud);
        tempAud.volume = 0;
        AudioManager.Instance.StartCoroutine("FadeIn", tempAud);
        videoPlayer.Play();
    }

    public void StopVideo()
    {
        var videoPlayer = Camera.main.gameObject.GetComponent<VideoPlayer>();
        AudioManager.Instance.StartCoroutine("FadeOut", AudioManager.Instance.vidSauce);
        AudioManager.Instance.vidSauce = null;
        videoPlayer.Stop();
        AudioManager.Instance.Unpause();

    }
}
