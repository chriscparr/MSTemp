using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour {

    public static VFXManager Instance { get { return s_instance; } }
    private static VFXManager s_instance = null;

    public GameObject LightningPrefab;
    public float minDuration = 0.5f;
    public float maxDuration = 1;

    public int minGenerations = 4;
    public int maxGenerations = 6;

    public float minChaos = 0.15f;
    public float maxChaos = 0.5f;

	// Use this for initialization
	void Awake () {
        s_instance = this;
	}
	
	// Update is called once per frame
	public void ElectricPulse (Vector3 origin, GameObject startObj, GameObject endObj) {
        GameObject lightning = Instantiate(LightningPrefab, origin, Quaternion.identity) as GameObject;
        LightningBoltScript lScript = lightning.GetComponent<LightningBoltScript>();

        lScript.StartObject = startObj;
        lScript.EndObject = endObj;
        lScript.ChaosFactor = Random.Range(minChaos, maxChaos);
        lScript.Generations = Random.Range(minGenerations, maxGenerations);
        lScript.enabled = true;
        lScript.GetComponent<SelfDestruct>().SelfDestroy(Random.Range(minDuration, maxDuration));
	}
}
