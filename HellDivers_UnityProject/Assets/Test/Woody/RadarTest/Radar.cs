using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour {

    public GameObject[] trackedObjects;
    List<GameObject> radarObjects;
    public GameObject radarPrefab;
    List<GameObject> borderObjects;
    public float SwicthDistance = 10f;
    public Transform helpTransform;
    // Use this for initialization
    void Start () {
        CreateRadarObjects();
    }
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < radarObjects.Count; i++)
        {
            if(Vector3.Distance(radarObjects[i].transform.position, this.transform.position) > SwicthDistance)
            {
                helpTransform.LookAt(radarObjects[i].transform);
                borderObjects[i].transform.position = transform.position + SwicthDistance * helpTransform.forward;
                radarObjects[i].layer = LayerMask.NameToLayer("Invisible");
                borderObjects[i].layer = LayerMask.NameToLayer("Radar");
            }
            else
            {
                radarObjects[i].layer = LayerMask.NameToLayer("Radar");
                borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
            }
        }
	}

    void CreateRadarObjects()
    {
        radarObjects = new List<GameObject>();
        borderObjects = new List<GameObject>();
        foreach (GameObject o in trackedObjects)
        {
            GameObject goRadar = Instantiate(radarPrefab, o.transform.position, Quaternion.identity) as GameObject;
            radarObjects.Add(goRadar);
            GameObject goBorder = Instantiate(radarPrefab, o.transform.position, Quaternion.identity) as GameObject;
            borderObjects.Add(goBorder);
        }
    }
}
