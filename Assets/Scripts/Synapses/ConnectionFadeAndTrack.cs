using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionFadeAndTrack : MonoBehaviour {

    public Subcell origin;
    public Subcell destination;

    public LineRenderer myLine;

    public float fadeOnDistance;
    public float disappearOnDistance;

    public float fadeDivider = 4;

    public Material mMat;
    Color fadeColor;
    public float alpha = 1;
    float distance;

    [HideInInspector]
    public bool amDoing;

    private bool amOverridingFade;

	// Use this for initialization
	void OnEnable () {
        myLine = GetComponent<LineRenderer>();
        mMat = GetComponent<LineRenderer>().material;
        fadeColor = mMat.GetColor("_TintColor");
        fadeColor = new Color(255, 255, 255);
	}

    public void Assign(Subcell orig, Subcell dest)
    {
        origin = orig;
        destination = dest;
        amDoing = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (origin != null && destination != null && amDoing == true)
        {
            myLine.SetPosition(0, origin.transform.position);
            myLine.SetPosition(1, destination.transform.position);
            distance = Vector3.Distance(origin.transform.position, destination.transform.position);

            if (distance < fadeOnDistance)
            {
                StartCoroutine("OverrideReappear");
            }

            if (distance > fadeOnDistance && distance < disappearOnDistance)
            {
                StopAllCoroutines();
                amOverridingFade = false;
                alpha = (fadeDivider / distance);

                fadeColor.a = alpha;

                myLine.startColor = fadeColor;
                myLine.endColor = fadeColor;
            }

            if (distance > disappearOnDistance && amOverridingFade != true)
            {
                StartCoroutine("OverrideDisappear");
            }
        }
	}

    IEnumerator OverrideDisappear()
    {
        amOverridingFade = true;

        while (alpha > ConnectionGenerator.Instance.MinimumLineAlpha)
        {
            alpha -= 0.1f * Time.deltaTime;
//            Debug.Log("OVERRIDIUNG");
            fadeColor.a = alpha;

            myLine.startColor = fadeColor;
            myLine.endColor = fadeColor;
            yield return null;
        }
        alpha = ConnectionGenerator.Instance.MinimumLineAlpha;
        fadeColor.a = alpha;

        myLine.startColor = fadeColor;
        myLine.endColor = fadeColor;
        amOverridingFade = false;
    }

    IEnumerator OverrideReappear()
    {
        

        while (alpha < 1)
        {

            alpha += 0.1f * Time.deltaTime;

            fadeColor.a = alpha;

            myLine.startColor = fadeColor;
            myLine.endColor = fadeColor;
            yield return null;
        }
        alpha = 1;
        fadeColor.a = alpha;

        myLine.startColor = fadeColor;
        myLine.endColor = fadeColor;

    }
}
