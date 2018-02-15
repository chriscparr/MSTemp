using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CaseStudyData : MonoBehaviour {

    public CaseStudyData()
    {
        ID = System.Guid.NewGuid().ToString();
    }

    public string ID;
    // public string[] VideoPaths;
    public VideoClip VideoPath;
    public TextMesh Label;

}
