using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public float Speed { get { return m_Speed; } set { m_Speed = value; } }

    [SerializeField] private float m_Speed = 5f;

    private void Start()
    {
        m_Controller = this.GetComponent<CharacterController>();

        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Move();
        }
        else if (m_Controller.isGrounded == false)
        {
            m_Controller.Move(Physics.gravity * Time.deltaTime);
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) FaceDirection();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (m_Cam != null)
        {
            m_CamFoward = Vector3.Scale(m_Cam.forward, Vector3.forward + Vector3.right);
            m_Direction = m_CamFoward * v + m_Cam.right * h;
        }
        else
        {
            m_Direction = Vector3.forward * v + Vector3.right * h;
        }

        m_Direction.Normalize();
        m_Move = m_Direction * m_Speed * Time.deltaTime;

        if (Input.GetButton("Run"))
        {
            m_Move *= 2.0f;
        }

        if (m_Controller.isGrounded == false)
        {
            m_Move += Physics.gravity * Time.deltaTime;
        }
        Debug.Log(h);
        Debug.Log(v);

        this.transform.forward = m_Direction;
        m_Controller.Move(m_Move);
    }


    private void FaceDirection()
    {
        m_MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(m_MouseRay, out m_MouseHit, 1000.0f, 1 << LayerMask.NameToLayer("Terrain")))
        {
            m_Direction = m_MouseHit.point - this.transform.position;
            m_Direction.y = 0.0f;

            if (m_Direction.magnitude < 0.1f) return;

            m_Controller.transform.forward = m_Direction;
        }
    }

    private CharacterController m_Controller;
    private Transform m_Cam;
    private Vector3 m_CamFoward;
    private Vector3 m_Direction;
    private Vector3 m_Move;
    private Ray m_MouseRay;
    private RaycastHit m_MouseHit;

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);
    }

#endif
}