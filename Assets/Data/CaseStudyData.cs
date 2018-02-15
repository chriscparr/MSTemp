using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseStudyData : MonoBehaviour {

    public CaseStudyData()
    {
        ID = System.Guid.NewGuid().ToString();
    }

    public string ID;
    // public string[] VideoPaths;
    public string VideoPath;
    public TextMesh Label;

}
