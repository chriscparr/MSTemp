using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

//source: https://gist.github.com/naojitaniguchi/7d71267814ccd0ca719e

public class VideoPicker : MonoBehaviour {

	public Texture2D shareButtonImage; // Use this for initialization

	[DllImport("__Internal")]
	private static extern void OpenVideoPicker(string game_object_name, string function_name);

	void OnGUI ()
	{
		if (GUILayout.Button (shareButtonImage, GUIStyle.none, GUILayout.Width (128), GUILayout.Height (128))) {
			OpenVideoPicker( "GameObject", "VideoPicked" );
		}
	}

	void VideoPicked( string path ){
		Debug.Log ("---->VideoPicked");
		Debug.Log( path );
	}
}
