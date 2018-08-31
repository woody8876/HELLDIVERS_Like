using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    private float m_Speed=3f;
    private float m_MoveHorizontal;
    private float m_MoveVertical;
    private Vector3 m_CharcterMove;
    private CharacterController m_CharacterController;

    private float m_MouseX;
    private float m_MouseY;

    // Use this for initialization
    void Start () {
        m_CharacterController = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {

        m_MoveHorizontal =Input.GetAxis("Horizontal");
        m_MoveVertical=Input.GetAxis("Vertical");

        m_CharcterMove = (this.transform.forward * m_MoveVertical + this.transform.right * m_MoveHorizontal) * m_Speed * Time.deltaTime;
        m_CharacterController.Move(m_CharcterMove);

        
        
    }
}
