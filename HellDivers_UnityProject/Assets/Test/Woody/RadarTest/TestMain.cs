using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMain : MonoBehaviour {

    public MobData m_MobData;
	// Use this for initialization
	void Start () {
        m_MobData = new MobData();
        m_MobData.Init();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
