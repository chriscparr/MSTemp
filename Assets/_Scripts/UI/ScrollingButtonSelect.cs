using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingButtonSelect : MonoBehaviour 
{
	[SerializeField]
	private MindshareButton m_msButtonPrefab;
	[SerializeField]
	private GameObject m_contentArea;

	private string[] m_selectOptions = new string[] { "Option1", "Option2", "Option3", "Option4", "Option5" };

	private float m_buttonWidth = 200f;


	// Use this for initialization
	void Start () 
	{
		m_contentArea.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, (m_selectOptions.Length * m_buttonWidth));
	}
	

}
