using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GestureTest : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_demoCube;

	private Vector3 m_initialTouchPosition1;

	private void Update()
	{
		/*
		if (Input.touchCount == 2)
		{
			HandleTwoFingers();
			return;
		}
		*/

		if (Input.touchCount == 1)
		{
			if (Input.GetTouch (0).phase == TouchPhase.Began)
			{
				// i swear this is needed
				Vector2 output = Vector2.zero;
				Vector2 correction = Input.GetTouch (0).position;
				output.x = correction.y;
				output.y = correction.x;
				m_initialTouchPosition1 = output;
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
			//m_demoCube.transform.localRotation.eulerAngles = rot;

			/*
			Vector2 touchPositionDelta = axis - m_initialTouchPosition1;

			Vector3 origin = Vector3.zero;
			float angleVal = touchPositionDelta.x * Time.fixedDeltaTime;
			Camera.main.transform.RotateAround (origin, Vector3.up, angleVal);
			Camera.main.transform.LookAt (origin);
			*/


			//m_MainCamera.transform.Translate (origin * touchPositionDelta.y * Time.fixedDeltaTime);

			//m_MainCamera.transform.Translate(transform.forward * touchPositionDelta.y * Time.deltaTime);
			/*
			if (m_MainCamera.transform.position.z >= zAxisLimit)
			{
				m_MainCamera.transform.position = new Vector3(
					m_MainCamera.transform.position.x,
					m_MainCamera.transform.position.y,
					zAxisLimit);
			}
			*/


			/*
			if (touchPositionDelta.x > touchPositionDelta.y * 5 || touchPositionDelta.x < touchPositionDelta.y * -5)
			{
				Debug.Log ("HORIZONTAL DRAG!");
				if (touch.deltaPosition.x > 0f)
				{
					Vector3 origin = Vector3.zero;
					float angleVal = 10f * m_ZoomSpeed * Time.deltaTime;
					Camera.main.transform.RotateAround (origin, Vector3.up, angleVal);
					Camera.main.transform.LookAt (origin);
				}
				if (touch.deltaPosition.x < 0f)
				{
					Vector3 origin = Vector3.zero;
					float angleVal = -10f * m_ZoomSpeed * Time.deltaTime;
					Camera.main.transform.RotateAround (origin, Vector3.up, angleVal);
					Camera.main.transform.LookAt (origin);
				}
				return;
			}
			if (touchPositionDelta.y > touchPositionDelta.x * 5 || touchPositionDelta.y < touchPositionDelta.x * -5)
			{
				Debug.Log ("VERTICAL DRAG!");
				if (touch.deltaPosition.y > 0f)
				{
					m_MainCamera.transform.Translate(transform.forward * m_ZoomSpeed * Time.deltaTime);
				}
				if (touch.deltaPosition.y < 0f)
				{
					m_MainCamera.transform.Translate(-transform.forward * m_ZoomSpeed * Time.deltaTime);
				}


				if (m_MainCamera.transform.position.z >= zAxisLimit)
				{
					m_MainCamera.transform.position = new Vector3(
						m_MainCamera.transform.position.x,
						m_MainCamera.transform.position.y,
						zAxisLimit);
				}
			}
			*/


		}

	}

	/*
	void HandleTwoFingers()
	{
		Touch touchZero = Input.GetTouch(0);
		Touch touchOne = Input.GetTouch(1);

		Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
		Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

		float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

		float gentleDelta = (deltaMagnitudeDiff / 100);


		Debug.Log("OUR DELTA MAGNITUDE IS " + gentleDelta + " AND OUR SCALE IS " + m_selectedCell.transform.localScale );
		m_selectedCell.transform.localScale = new Vector3(m_selectedCell.transform.localScale.x + (gentleDelta * -1),
			m_selectedCell.transform.localScale.y + (gentleDelta * -1),
			m_selectedCell.transform.localScale.z + (gentleDelta * -1));

		if (m_selectedCell.transform.localScale.x >= 3)
		{
			m_selectedCell.transform.localScale = m_SafetyScaleVector;
		}
		if (m_selectedCell.transform.localScale.x <= 0)
		{
			m_selectedCell.transform.localScale = Vector3.zero;
		}


	}
	*/
}
