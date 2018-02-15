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
        //DirectoryInfo directory = new DirectoryInfo(Resources.Load);

        //DirectoryInfo[] subDirectories = directory.GetDirectories();

        //List<string> allPotentialSubdirectories = new List<string>();

        //foreach (DirectoryInfo dic in subDirectories)
        //{
        //    allPotentialSubdirectories.Add(dic.Name);
        //}

        //for (int i = 0; i < subDirectories.Length; i++)
        //{
        //    DirectoryInfo sInfo = subDirectories[i];

        //    FileInfo[] sFiles = sInfo.GetFiles();

        //    //for (int p = 0; p < sFiles.Length; p++)
        //    //{

        //        switch (subDirectories[i].Name)
        //        {
        //            case "AGILE":

        //                break;
        //            case "CONTENT":

        //            foreach (FileInfo content in sFiles)
        //            {
        //                if (!content.FullName.Contains("meta") && !content.FullName.Contains(".DS_Store"))
        //                m_ContentVideos.Add(content.FullName);
        //            }
        //                break;
        //            case "DATA":

        //                break;
        //            case "FAST":
        //            foreach (FileInfo content in sFiles)
        //            {
        //                if (!content.FullName.Contains("meta") && !content.FullName.Contains(".DS_Store"))
        //                m_FastVideos.Add(content.FullName);
        //            }
        //                break;
        //            case "GROWTH":

        //                break;
        //            case "LIFE":

        //                break;
        //            case "LOOP":

        //                break;
        //            case "SHOP":
        //            foreach (FileInfo content in sFiles)
        //            {
        //                if (!content.FullName.Contains("meta") && !content.FullName.Contains(".DS_Store"))
        //                m_ShopVideos.Add(content.FullName);
        //            }
        //                break;
        //        //}
        //    }
        //}

        //foreach (FileInfo file in subDirectories)
        //{
        //    if (!file.FullName.Contains(".meta"))
        //    {
        //        print(file.FullName);
        //    }
        //}

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

        //foreach (FileInfo file in vids)
        //{
        //    if (!file.FullName.Contains(".meta"))
        //    {
        //        videoFilePaths.Add(file.FullName);
        //    }
        //}

        //foreach (string s in videoFilePaths)
        //{
        //    print(s);
        //}
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
        videoPlayer.Play();
    }

    public void StopVideo()
    {
        var videoPlayer = Camera.main.gameObject.GetComponent<VideoPlayer>();
        AudioManager.Instance.vidSauce = null;
        videoPlayer.Stop();
        AudioManager.Instance.Unpause();

    }
}
