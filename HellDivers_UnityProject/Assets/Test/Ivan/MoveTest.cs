using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour {

    Vector3 m_vPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_vPos.x = Input.GetAxis("Horizontal");
        m_vPos.z = Input.GetAxis("Vertical");
        transform.position += m_vPos * Time.deltaTime * 10;
	}
}
