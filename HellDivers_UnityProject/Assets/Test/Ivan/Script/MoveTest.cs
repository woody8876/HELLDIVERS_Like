using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveTest : Character {

    public float m_Speed = 30;
    Vector3 m_vPos;
	
	// Update is called once per frame
	void Update () {
        m_vPos.x = Input.GetAxis("Horizontal") * Time.deltaTime * m_Speed;
        m_vPos.z = Input.GetAxis("Vertical") * Time.deltaTime * m_Speed;
        transform.Translate(m_vPos);
	}

}
