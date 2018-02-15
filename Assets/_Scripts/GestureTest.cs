using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GestureTest : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_demoCube;

	private Vector3 m_initialTouchPosition1;

	private float m_pinchDistance;

	private void Update()
	{

		if (Input.touchCount == 2)
		{
			if (Input.GetTouch (0).phase == TouchPhase.Began || Input.GetTouch (1).phase == TouchPhase.Began)
			{
				Vector2 dist = Input.GetTouch (0).position - Input.GetTouch (1).position;
				m_pinchDistance = dist.magnitude;
			}
			HandleTwoFingers();
			return;
		}


		if (Input.touchCount == 1)
		{
			if (Input.GetTouch (0).phase == TouchPhase.Began)
			{

			}
			HandleOneFinger ();
		}
	}

	private void HandleOneFinger()
	{
		Touch touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Moved)
		{

			//m_demoCube.transform.localPosition += new Vector3 (touch.deltaPosition.x, touch.deltaPosition.y, 0f) * Time.deltaTime;

			m_demoCube.transform.localPosition += new Vector3 (0f, touch.deltaPosition.y, 0f) * Time.deltaTime;

			Vector3 rot = (new Vector3 (0f, touch.deltaPosition.x, 0f) * Time.deltaTime);

			m_demoCube.transform.Rotate (rot);
		}

	}


	void HandleTwoFingers()
	{
		Touch touchZero = Input.GetTouch(0);
		Touch touchOne = Input.GetTouch(1);
		float deltaPinchDistance;
		if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
		{
			Vector2 dist = touchZero.position - touchOne.position;
			deltaPinchDistance = dist.magnitude - m_pinchDistance;

			SetCubeScale (deltaPinchDistance * 0.1f);
			//m_demoCube.transform.localScale += new Vector3 (deltaPinchDistance, deltaPinchDistance, deltaPinchDistance) * 0.01f * Time.deltaTime;


			//Debug.Log ("Previous pinch distance: " + m_pinchDistance.ToString() + " new pinch distance: " + dist.magnitude.ToString());
			//m_pinchDistance = dist.magnitude;
		}


	}

	private void SetCubeScale(float a_newScale)
	{
		if (a_newScale > 0f && a_newScale < 5f)
		{
			Debug.Log ("setting cube scale to: " + a_newScale.ToString ());
			Vector3 cubeScale = Vector3.one * a_newScale;
			m_demoCube.transform.localScale = cubeScale;
		}

	}
}
