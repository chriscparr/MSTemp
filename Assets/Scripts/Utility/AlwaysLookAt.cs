using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookAt : MonoBehaviour {

    public Transform mainCam;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag ("MainCamera").transform;
        transform.localPosition = Vector3.zero;
    }
    
    // Update is called once per frame
    void Update () {
        transform.LookAt (mainCam);
        transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y - 180, transform.eulerAngles.z);
    }
}
