using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour 
{

	public static AudioManager Instance {get { return s_instance;}}
	private static AudioManager s_instance = null;

    public VolumeSlider vol;
    public ReverbToggle rev;
    public Toggle gradual;
    public Toggle overlay;

    public DoubleAudioSource dSource;

	[HideInInspector]
    public AudioSource vidSauce;
    public AudioSource previousTrack;

    public float fadeOutSpeed = 0.5f;
    public float fadeInSpeed = 0.5f;
    

	private void Awake()
	{
		s_instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}

    public void Pause()
    {
        foreach (AudioSource asX in Camera.main.gameObject.GetComponents<AudioSource>())
        {
            if (asX.isPlaying)
            {  
                previousTrack = asX;
                StartCoroutine("FadeOut", asX);
            }
        }



        }

    public void Unpause()
    {
        StartCoroutine("FadeIn", previousTrack);
    }

    public IEnumerator FadeOut (AudioSource a)
    {
        while (a.volume > 0)
        {
            a.volume -= fadeOutSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeIn (AudioSource a)
    {
        while (a.volume < 1)
        {
            a.volume += fadeInSpeed * Time.deltaTime;
            yield return null;
        }
    }

	// Update is called once per frame
	public void AddVideoAudio()
    {
        if (vidSauce == null)
        {
            vidSauce = Camera.main.gameObject.AddComponent<AudioSource>();
        }
    }
}
