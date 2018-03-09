using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Video;
using System;

//source: https://gist.github.com/naojitaniguchi/7d71267814ccd0ca719e

public class VideoPicker : MonoBehaviour 
{
	public delegate void VideoPickerInputAction(string a_videoPath);
	public VideoPickerInputAction OnVideoSelected;

	[DllImport("__Internal")]
	private static extern void OpenVideoPicker(string game_object_name, string function_name);

	public void ShowVideoPicker()
	{
		OpenVideoPicker( this.gameObject.name, "VideoSelected" );
	}

	private void VideoSelected(string a_selectedVideoPath)
	{
		Debug.Log ("Video Selected");
		if (OnVideoSelected != null)
		{
			OnVideoSelected (a_selectedVideoPath);
		}
	}
}
