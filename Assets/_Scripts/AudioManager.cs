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

    [Header("Leave me blank")]
    public AudioSource vidSauce;
    public AudioSource previousTrack;
    

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
                asX.Pause();
                previousTrack = asX;
            }
        }



        }

    public void Unpause()
    {
        previousTrack.UnPause();
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
