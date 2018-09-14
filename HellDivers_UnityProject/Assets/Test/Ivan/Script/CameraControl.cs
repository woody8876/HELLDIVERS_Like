using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform m_Player;
    Camera m_Cam;
    Collider m_currentHit;
    List<Collider> m_Wall;
// Use this for initialization
void Start () {
        m_Cam = GetComponent<Camera>();
        m_currentHit = GetComponent<Collider>();
        m_Wall = new List<Collider>();
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_Cam.transform.position = new Vector3( m_Player.position.x - 26, m_Player.position.y + 35, m_Player.position.z - 22);
        m_Cam.transform.rotation = Quaternion.Euler(45, 48, 0);

    }
	
	// Update is called once per frame
	void Update () {
        FollowTarget(m_Player);
        CheckView();

    }

    private void FollowTarget(Transform Target)
    {
        Vector3 vCurCamPos = m_Cam.transform.position;
        float fMovingDis = (vCurCamPos - Target.position).magnitude;
        if (Mathf.Abs(fMovingDis ) > 5f)
        {
            vCurCamPos.x = Mathf.Lerp(vCurCamPos.x, Target.position.x - 26, 10f * Time.deltaTime);
            vCurCamPos.y = Mathf.Lerp(vCurCamPos.y, Target.position.y + 35, 10f * Time.deltaTime);
            vCurCamPos.z = Mathf.Lerp(vCurCamPos.z, Target.position.z - 22, 10f * Time.deltaTime);
            m_Cam.transform.position = vCurCamPos;
        }
    }

    private void CheckView()
    {
        RaycastHit m_Hit1;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out m_Hit1, (m_Player.position - this.transform.position).magnitude, 1 << LayerMask.NameToLayer("Water")))
        {
            Collider[] AroundWall = Physics.OverlapSphere(m_Player.position, 10f);
            Vector3 vTarget2Cam = this.transform.position - m_Player.position;
            Vector3 vWall2Cam;
            foreach (Collider c in AroundWall)
            {
                vWall2Cam = this.transform.position - c.transform.position;
                Debug.DrawLine(this.transform.position, c.transform.position);
                float angle = Mathf.Acos(Vector3.Dot(vWall2Cam.normalized, vTarget2Cam.normalized)) * Mathf.Rad2Deg;


                if ( Mathf.Abs(angle) < 30.0f && vWall2Cam.magnitude < vTarget2Cam.magnitude)
                {
                    c.gameObject.GetComponent<Renderer>().enabled = false;
                    m_Wall.Add(c);
                }
            }
        }
        else if (Physics.Raycast(this.transform.position, this.transform.forward, out m_Hit1, (m_Player.position - this.transform.position).magnitude, 1 << LayerMask.NameToLayer("Terrian")))
        {
            Collider[] AroundWall = Physics.OverlapSphere(m_Player.position, 10f);
            Vector3 vTarget2Cam = this.transform.position - m_Player.position;
            Vector3 vWall2Cam;

            foreach (Collider c in AroundWall)
            {
                vWall2Cam = this.transform.position - c.transform.position;
                Debug.DrawLine(this.transform.position, c.transform.position);
                float angle = Vector3.Dot(vWall2Cam.normalized, vTarget2Cam.normalized);

                if (angle > 0.9f && vWall2Cam.magnitude < vTarget2Cam.magnitude)
                {
                    c.gameObject.GetComponent<Renderer>().enabled = false;
                    m_Wall.Add(c);
                }
            }
        }
        else
        {
            for (int i = 0; i < m_Wall.Count; i++)
            {
                m_Wall[i].gameObject.GetComponent<Renderer>().enabled = true;
            }
        }

        //if (Physics.Raycast(this.transform.position, this.transform.forward, out m_Hit1, (m_Player.position - this.transform.position).magnitude, 1 << LayerMask.NameToLayer("Water")))
        //{
        //    m_currentHit = m_Hit1.collider;
        //    m_Wall.Add(m_currentHit);
        //    m_Hit1.collider.gameObject.GetComponent<Renderer>().enabled = false;

        //    Debug.DrawRay(this.transform.position, m_Player.position - this.transform.position, Color.green);
        //}
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_Player.position, 10f);
    }
}
