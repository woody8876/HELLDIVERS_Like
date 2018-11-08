using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour {

    public GameObject m_Go;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if(m_Go == null)
        {
            m_Go = GameObject.FindGameObjectWithTag("Player");
        }
        this.transform.position = m_Go.transform.position + Vector3.up * 100f;
    }
}
