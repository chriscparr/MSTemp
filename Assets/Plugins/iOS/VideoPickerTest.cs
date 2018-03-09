using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Video;
using System;
using System.IO;

//source: https://gist.github.com/naojitaniguchi/7d71267814ccd0ca719e

public class VideoPickerTest : MonoBehaviour 
{
	public delegate void VideoPickerInputAction(string a_videoPath);
	public VideoPickerInputAction OnVideoSelected;

	[DllImport("__Internal")]
	private static extern void OpenVideoPicker(string game_object_name, string function_name);


	public Texture2D shareButtonImage; 

	void OnGUI ()
	{
		if (GUILayout.Button (shareButtonImage, GUIStyle.none, GUILayout.Width (128), GUILayout.Height (128))) {
			OpenVideoPicker( this.gameObject.name, "VideoPicked" );
		}	
	}

	void VideoPicked( string path )
	{

		Debug.Log ("---->VideoPicked");
		Debug.Log( path );

		StartCoroutine ("CacheVideo", path);
		// Ali's changes 6/2/18
		//Playback (path);
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

	IEnumerator CacheVideo(string a_filePath)
	{
		WWW wwwVid = new WWW (a_filePath);
		yield return wwwVid;
		Debug.Log ("Download from " + a_filePath + " has completed");
		string destinationPath = Path.Combine(Application.persistentDataPath, a_filePath.Substring (a_filePath.LastIndexOf ("/") + 1));
		File.WriteAllBytes (destinationPath, wwwVid.bytes);
		Debug.Log ("Data saved to: " + destinationPath);
		yield return new WaitForSeconds (2f);
		StartCoroutine ("Playback", destinationPath);
	}
}
