using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(iTweenPath))]
public class OnRailsMovement : MonoBehaviour 
{
	private float m_speed;
	private iTween.EaseType m_easeType = iTween.EaseType.easeInOutSine;
	private iTweenPath m_thisPath;
	private List<Vector3> m_pathPoints = new List<Vector3>();
	private List<CaseCell> m_allCases = new List<CaseCell>();
	private int m_pathPointIndex = 0;

	private bool m_isTracking = false;
	private CaseCell m_trackedCell;

	// Use this for initialization
	private void Awake() 
	{
		iTween.Init (this.gameObject);
	}
		
	public void Init(CaseCell[] a_allCells)
	{
		m_allCases.Clear ();
		for (int i = 0; i < a_allCells.Length; i++)
		{
			m_allCases.Add (a_allCells [i]);
		}
		m_pathPointIndex = 0;
		Debug.Log ("Case Cell list contains " + m_allCases.Count + " members!");
	}

	public void BeginCases()
	{
		MoveTo (m_allCases [0]);
	}

	private void MoveTo (CaseCell a_case)
	{
		m_trackedCell = a_case;
		StartCoroutine("MoveCameraToTrackedCaseCell");
	}

	private void ReachedPathPoint() 
	{
		StartCoroutine("PlayAndWait");
		m_trackedCell.PlayCaseStudy ();
	}

	public void GoToNextPoint()
	{
		m_isTracking = false;
		Debug.Log ("GoToNextPoint() index: " + m_pathPointIndex.ToString() + " m_allCases.Count: " + m_allCases.Count.ToString());
		if (m_pathPointIndex < m_allCases.Count -1)
		{
			//stop any currently playing videos in the ui
			m_pathPointIndex++;
			MoveTo (m_allCases [m_pathPointIndex]);
		} 
		else
		{
			GoHome ();
		}
	}

	public void GoToPreviousPoint()
	{
		m_isTracking = false;
		if (m_pathPointIndex > 0)
		{
			//stop any currently playing videos in the ui
			m_pathPointIndex--;
			MoveTo(m_allCases [m_pathPointIndex]);
		}
		else
		{
			GoHome ();
		}
	}

	IEnumerator PlayAndWait() 
	{
		yield return new WaitForSeconds (2);
		/*
		if (m_pathPointIndex > m_thisPath.nodeCount)
		{
			CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
			CameraInputManager.Instance.ResetPosition (true);
			m_pathPointIndex = 0;
		}
		*/
		yield return null;
	}

	private IEnumerator MoveCameraToTrackedCaseCell()
	{
		for (float percent = 0f; percent < 1f; percent += 0.01f)
		{
			gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, m_trackedCell.CameraPositioningPoint.position, percent);
			gameObject.transform.LookAt (m_trackedCell.transform);
			yield return new WaitForEndOfFrame ();
		}
		Debug.Log ("MoveCameraToTrackedCaseCell - Completed!");
		m_isTracking = true;
		ReachedPathPoint ();
		yield return null;

	}

	public void GoHome()
	{
		Debug.Log ("Going Home!");
		m_isTracking = false;
		m_allCases.Clear ();
		m_trackedCell = null;
		//VideoManager.Instance.StopVideo();
		UIManager.Instance.HideCaseStudyView();
		CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
		CameraInputManager.Instance.ResetPosition(true);
	}

	private void Update()
	{
		if (m_isTracking)
		{
			gameObject.transform.position = m_trackedCell.CameraPositioningPoint.position;
			gameObject.transform.rotation = m_trackedCell.CameraPositioningPoint.rotation;
		}
	}
}
