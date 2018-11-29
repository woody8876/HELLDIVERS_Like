using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    [SerializeField] private GameObject m_GO;
    // Use this for initialization
    void Start () {
        m_GO = Instantiate(m_GO);
        m_GO.transform.position = new Vector3(273.794f, 54.6f, 510.713f);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIMissionBriefingMap.Instance.AddPointPrefab(m_GO, eMapPointType.MISSIONTOWER);
        }
    }
}
