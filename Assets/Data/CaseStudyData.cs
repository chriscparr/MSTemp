using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CaseStudyData : MonoBehaviour 
{
    public string ID;
    // public string[] VideoPaths;
    public VideoClip VideoPath;
    public TextMesh Label;
	
	private void Awake()
	{
		ID = System.Guid.NewGuid().ToString();
	}
}
