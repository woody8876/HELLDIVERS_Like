using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float Speed { get { return m_Speed; } set { m_Speed = value; } }
    public CharacterController Controller { get; private set; }

    [SerializeField] private float m_Speed = 1.1f;

    // Use this for initialization
    private void Start()
    {
        Controller = this.GetComponent<CharacterController>();

        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Move();
        }
        else
        {
            if (Controller.isGrounded == false)
            {
                Controller.Move(Physics.gravity * Time.deltaTime);
            }
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

        if (m_Direction.magnitude > 1)
        {
            m_Direction.Normalize();
        }

        m_Move = m_Direction * m_Speed * Time.deltaTime;

        if (Input.GetButton("Run"))
        {
            m_Move *= 2.0f;
        }

        if (Controller.isGrounded == false)
        {
            m_Move += Physics.gravity * Time.deltaTime;
        }

        this.transform.forward = m_Direction;
        Controller.Move(m_Move);
    }

    private void FaceDirection()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out mouseHit, 1000.0f, 1 << LayerMask.NameToLayer("Terrain")))
        {
            m_Direction = mouseHit.point - this.transform.position;
            m_Direction.y = 0.0f;

            if (m_Direction.magnitude < 0.1f) return;

            Controller.transform.forward = m_Direction;
        }
    }

    private Transform m_Cam;
    private Vector3 m_CamFoward;
    private Vector3 m_Direction;
    private Vector3 m_Move;
    private Ray mouseRay;
    private RaycastHit mouseHit;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);
    }
}