using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    private float m_MoveHorizontal;
    private float m_MoveVertical;
    private Vector3 m_CharcterMove;
    private Vector3 m_StandForward;
    private float m_Speed;
    private float m_SlowSpeed = 1.5f;
    private float m_WalkSpeed = 3f;
    private float m_RunSpeed = 6f;
    private CharacterController m_CharacterController;

    Vector3 m_MousePos;
    Ray r;
    RaycastHit rh;
    private GameObject m_Body;

    private GameObject LineRenderGameObject;
    private LineRenderer lineRenderer;
    private int lineLength = 2;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        LineRenderGameObject = GameObject.Find("Line");
        lineRenderer = (LineRenderer)LineRenderGameObject.GetComponent("LineRenderer");
        m_Body = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        m_MoveHorizontal = Input.GetAxis("Horizontal");
        m_MoveVertical = Input.GetAxis("Vertical");
        m_Speed = (Input.GetKey(KeyCode.LeftShift)) ? m_RunSpeed : m_WalkSpeed;

        m_MousePos = Input.mousePosition;
        r = Camera.main.ScreenPointToRay(m_MousePos);


        m_CharcterMove = (this.transform.forward * m_MoveVertical + this.transform.right * m_MoveHorizontal) * m_Speed * Time.deltaTime;
        m_CharcterMove += Physics.gravity * Time.deltaTime;
        m_CharacterController.Move(m_CharcterMove);


        //紅外線
        if (Input.GetMouseButton(1))
        {
            m_CharacterController.Move(m_CharcterMove*-0.5f);
            LineRenderInit();
            lineRenderer.SetPosition(0,Vector3.forward * 50f);
            FaceToRay(r,rh,1000);
        }
        else
        {
            lineRenderer.enabled = false;
        }

        //角色面相移動的方向
        if (Input.GetMouseButton(1) == false)
        {
            if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false)
            {

            }
            else
            {
                m_CharcterMove.y = 0.0f;
                m_Body.transform.forward = m_CharcterMove;
            }
        }
    }

    private void FaceToRay(Ray r,RaycastHit rh,int Distance)
    {
        if (Physics.Raycast(r, out rh, 1000.0f))
        {
            Vector3 vTarget = rh.point;
            Vector3 vForward = vTarget - m_Body.transform.position;
            vForward.y = 0.0f;

            vForward.Normalize();
            m_Body.transform.forward = vForward;
        }
    }
    private void LineRenderInit()
    {
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = lineLength;
    }
    private void OnDrawGizmos()
    {
        Vector3 target1;
        target1 = m_Body.transform.forward * 10 + this.transform.position;
        Gizmos.DrawLine(this.transform.position, target1);
    }
}

