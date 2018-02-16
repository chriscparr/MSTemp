using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseStudyView : MonoBehaviour {

    OnRailsMovement ourMover;
	// Use this for initialization
	void Start () {
        ourMover = Camera.main.GetComponent<OnRailsMovement>();
	}
	
	// Update is called once per frame
	public void GoToNextCaseStudy () {
        if (ourMover.nodeI == 0)
        {
            ourMover.Initialise();
        }
        else
        {
            ourMover.GoToNextPoint();
        }
	}

    public void GoToPreviousCaseStudy() {
        ourMover.GoToPreviousPoint();
    }

    public void GoHome() {
        ourMover.GoHome();
        gameObject.SetActive(false);
    }
}
