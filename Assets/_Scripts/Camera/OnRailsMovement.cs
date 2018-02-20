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

	// Use this for initialization
	private void Awake() 
	{
		iTween.Init (this.gameObject);
	}
		
	public void Init(CaseCell[] a_allCells)
	{
		for (int i = 0; i < a_allCells.Length; i++)
		{
			m_allCases.Add (a_allCells [i]);
			m_pathPoints.Add (a_allCells [i].transform.position);
		}
		m_thisPath = GetComponent<iTweenPath>();
		m_thisPath.nodeCount = m_pathPoints.Count;
		m_thisPath.nodes = m_pathPoints;
		m_pathPointIndex = 0;
	}

	private void MoveTo(int a_pathIndex)
	{
		m_thisPath.nodes [a_pathIndex] = m_allCases [a_pathIndex].transform.position + new Vector3 (0f, 0f, -2f);
		iTween.MoveTo(this.gameObject, iTween.Hash("position", m_thisPath.nodes[a_pathIndex], "time", 2f, "easetype", m_easeType, "orienttopath", true, "lookTime", 5, "oncomplete", "ReachedPathPoint"));
	}

	private void ReachedPathPoint() 
	{
		m_allCases [m_pathPointIndex - 1].PlayCaseStudy ();
		StartCoroutine("PlayAndWait");
	}

	public void GoToNextPoint()
	{
		//stop any currently playing videos in the ui
		Debug.Log("Heading to node " + m_pathPointIndex.ToString() + "/" + m_pathPoints.Count.ToString());
		if (m_pathPointIndex >= m_thisPath.nodeCount)
		{
			// get out, we are done.
			CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
			CameraInputManager.Instance.ResetPosition();
		}
		else
		{
			MoveTo(m_pathPointIndex);
			m_pathPointIndex++;
		}
	}

	public void GoToPreviousPoint()
	{
		//stop any currently playing videos in the ui
		m_pathPointIndex--;
		Debug.Log("Heading to node " + m_pathPointIndex.ToString() + "/" + m_pathPoints.Count.ToString());
		if (m_pathPointIndex <= 0)
		{
			// get out, we are done.
			CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
			CameraInputManager.Instance.ResetPosition(true);
			m_pathPointIndex = 0;
		}
		else
		{
			MoveTo(m_pathPointIndex);
		}
	}

	IEnumerator PlayAndWait() 
	{
		yield return new WaitForSeconds (2);
		if (m_pathPointIndex > m_thisPath.nodeCount)
		{
			CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
			CameraInputManager.Instance.ResetPosition (true);
			m_pathPointIndex = 0;
		}
		yield return null;
	}

	public void GoHome()
	{
		//VideoManager.Instance.StopVideo();
		m_pathPointIndex = 0;
		CameraInputManager.Instance.SetPhase(CameraInputManager.Phase.MainCellPhase);
		CameraInputManager.Instance.ResetPosition(true);
	}
}
