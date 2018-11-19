using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour {

    public GameObject[] m_GOPlayers;
    public GameObject[] trackedObjects;
    public GameObject radarPrefab;
    List<GameObject> radarObjects;
    List<GameObject> borderObjects;
    public float SwicthDistance = 100f;
    public Vector3 Center;
    public Transform helpTransform;
    // Use this for initialization
    void Start () {
        //CreateRadarObjects();
        //radarObjects = new List<GameObject>();
        //borderObjects = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {

        List<Player> pList = InGamePlayerManager.Instance.Players;
        if (pList == null) return;
        Center.Set(0, 0, 0);
        for (int i = 0; i < pList.Count; i++)
        {
            Center += pList[i].transform.position;
        }
        Center /= pList.Count;
        this.transform.position = Center + Vector3.up * 10f;

        //for (int i = 0; i < radarObjects.Count; i++)
        //{
        //    if(Vector3.Distance(radarObjects[i].transform.position, Center) > SwicthDistance)
        //    {
        //        //helpTransform.LookAt(radarObjects[i].transform);
        //        //borderObjects[i].transform.position = this.transform.position + SwicthDistance * helpTransform.forward;
        //        radarObjects[i].layer = LayerMask.NameToLayer("Invisible");
        //        //borderObjects[i].layer = LayerMask.NameToLayer("Radar");
        //    }
        //    else
        //    {
        //        radarObjects[i].layer = LayerMask.NameToLayer("Radar");
        //        //borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
        //    }
        //}
	}

    //public void CreateRadarObjects( )
    //{
    //    radarObjects = new List<GameObject>();
    //    borderObjects = new List<GameObject>();
    //    foreach (GameObject o in trackedObjects)
    //    {
    //        GameObject goRadar = Instantiate(radarPrefab, o.transform.position, Quaternion.identity) as GameObject;
    //        radarObjects.Add(goRadar);
    //        //GameObject goBorder = Instantiate(radarPrefab, o.transform.position, Quaternion.identity) as GameObject;
    //        //borderObjects.Add(goBorder);
    //    }
    //}
}
