﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour {

    public GameObject[] m_GOs;
    public Vector3 m_Center;
	// Use this for initialization
	void Start () {
        m_Center.Set(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (m_GOs == null)
        {
            m_GOs = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < m_GOs.Length; i++)
            {
                m_Center += m_GOs[i].transform.position;
            }
            m_Center /= m_GOs.Length;
        }
        this.transform.position = m_Center + Vector3.up * 100f;
        Debug.Log(this.transform.position);
    }
}
