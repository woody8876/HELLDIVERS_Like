using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTest : MonoBehaviour {

    public ParticleSystem m_ParticleSystem;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_ParticleSystem.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            m_ParticleSystem.gameObject.SetActive(false);
        }
    }
}
