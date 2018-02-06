using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Video;
using System;

public class VideoPicker : MonoBehaviour {

	public Texture2D shareButtonImage; // Use this for initialization

	[DllImport("__Internal")]
	private static extern void OpenVideoPicker(string game_object_name, string function_name);
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI ()
	{
		if (GUILayout.Button (shareButtonImage, GUIStyle.none, GUILayout.Width (128), GUILayout.Height (128))) {
			OpenVideoPicker( this.gameObject.name, "VideoPicked" );
		}	
	}

	void VideoPicked( string path ){
		
		Debug.Log ("---->VideoPicked");
		Debug.Log( path );

		// Ali's changes 6/2/18
		Playback (path);
	}

	void Playback(string path) {
		
		GameObject camera = GameObject.Find ("Main Camera");

		var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer> ();

		videoPlayer.playOnAwake = false;

		videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
		videoPlayer.url = path;

		videoPlayer.Play ();
		// End of Ali's changes 6/2/18
	}
		
}
