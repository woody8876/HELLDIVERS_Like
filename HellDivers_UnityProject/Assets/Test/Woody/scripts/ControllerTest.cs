﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviour {

#region Private Variable
    private CharacterController m_Controller;
    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 m_Direction;
    private Vector3 m_Move;
    private Ray m_MouseRay;
    private RaycastHit m_MouseHit;
    private AniTest PAC;
#endregion Private Variable

    private void Start()
    {
        PAC = this.GetComponent<AniTest>();
        m_Controller = this.GetComponent<CharacterController>();

        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }

    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
        PAC.Move(m_Move);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);
    }

#endif
}
