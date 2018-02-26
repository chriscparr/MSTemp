using UnityEngine;
using System.Collections;

public class LavaMoving : MonoBehaviour 
{
	public int materialIndex = 0;

	public bool AmIZZZ;

	public Material Z1;
	public Material Z2;
	public Material Z3;
	float Timer;
	float Begin;
	public Vector2 uvAnimationRate = new Vector2( 1.0f, 0.0f );
	public string textureName = "_MainTex";
	
	Vector2 uvOffset = Vector2.zero;


	void Start () {

		Begin = 0;
		Timer = 3;
	}
	void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
		}


		if (AmIZZZ == true)
		{
			Begin += 1 * Time.deltaTime;

			if (Begin > 0 && Begin < 1)
			{
				GetComponent<Renderer>().material = Z1;
			}
			if (Begin > 1 && Begin < 2)
			{
				GetComponent<Renderer>().material = Z2;
			}
			if (Begin > 2 && Begin < 3)
			{
				GetComponent<Renderer>().material = Z3;
			}
			if (Begin >= 3)
			{
				GetComponent<Renderer>().material = Z1;
				Begin = 0;
			}
		}

	}
}