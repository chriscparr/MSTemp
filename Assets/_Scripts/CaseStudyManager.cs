using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CaseStudyManager : MonoBehaviour {


    public static CaseStudyManager Instance { get { return s_instance; } }
    private static CaseStudyManager s_instance = null;

    [SerializeField]
    private GameObject m_caseCellPrefab;

    public List<CaseStudyData> allCases = new List<CaseStudyData>();

    private void Awake()
    {
        s_instance = this;
    }

    // Use this for initialization
    public void GenerateCaseStudies (GameObject spawnPos, int numberOfStudies, string serviceType) {

        for (int i = 0; i < (numberOfStudies); i++)
        {
            GameObject cas = Instantiate<GameObject>(m_caseCellPrefab, spawnPos.transform);
            if (i != 0)
            {
                cas.transform.localPosition = spawnPos.GetComponent<SphereCollider>().center;
                cas.transform.localPosition = Random.insideUnitSphere * 1; // example TODO i am an example
            }
            else
            {
                Vector3 firstVec = spawnPos.GetComponent<SphereCollider>().center;
                firstVec.z += 2f; // TODO I AM AN EXAMPLE
                cas.transform.localPosition = firstVec;
            }

            cas.GetComponent<Renderer>().material.color = Random.ColorHSV();

            HandleContentPopulation(cas, i, serviceType.ToString());


        }

        allCases = allCases.Distinct().ToList();
		
	}

    void HandleContentPopulation(GameObject currentCaseCell, int index, string serviceName)
    {
        CaseStudyData cdata = currentCaseCell.GetComponent<CaseStudyData>();

        if (index==0)
        {
            // THE FIRST CASE STUDY CELL WE CREATE WILL BE THE TEXT BLURB (CREATE SOME SORT OF UI FOR THIS MAYBE?)
            // CASE BEING, OUR FIRST CELL WILL NOT CONTAIN A VIDEO, RATHER IT WILL HAVE A BLURB ABOUT THE SERVICE
            // DEAL WITH THIS AT SOME POINT LATER? TODO
            cdata.VideoPath = null;
            cdata.Label.text = "Intro:"; // todo I AM AN EXAMPLE

        }
        else
        {
            int Cindex = (index - 1);
            // right, FOR NOW TODO just assume that each case study cell has ONLY ONE VIDEO, just for now!
            switch (serviceName)
            {
                
                case "AGILE":

                    break;
                case "CONTENT":
                    cdata.VideoPath = VideoManager.Instance.m_ContentVideos[Cindex];
                    break;
                case "DATA":

                    break;
                case "FAST":
                    cdata.VideoPath = VideoManager.Instance.m_FastVideos[Cindex];
                    break;
                case "GROWTH":

                    break;
                case "LIFE":

                    break;
                case "LOOP":

                    break;
                case "SHOP":
                    cdata.VideoPath = VideoManager.Instance.m_ShopVideos[Cindex];
                    break;
                    //}
            }
            cdata.Label.text = "Case Study: " + index.ToString();
            allCases.Add(cdata);
        }


        // either get the video URLs from JSON, or from VideoManager?
        // no, get them from VideoManager
        // but then, how will VideoManager get them? again, from JSON file?
        // keep in mind this is just for phase 1, so dont worry too much
    }
	
}
