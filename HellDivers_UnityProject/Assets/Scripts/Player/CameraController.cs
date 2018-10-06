using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float m_fPosX = 0.0f;
    public float m_fPosY = 10.0f;
    public float m_fPosZ = -7.5f;
    public float m_fRotX = 60.0f;
    public float m_fRotY = 0.0f;
    public float m_fRotZ = 0.0f;

    public float K;
    public float L;

    public Transform m_Player;
    private Camera m_Cam;
    private Vector3 m_vPrePos;
    private Vector3 m_vCurPos;

    // Use this for initialization
    private void Start()
    {
        m_Cam = Camera.main.GetComponent<Camera>();
        m_Cam.transform.position = new Vector3(m_fPosX, m_fPosY, m_fPosZ);
        m_Cam.transform.rotation = Quaternion.Euler(m_fRotX, m_fRotY, m_fRotZ);
        m_Cam.fieldOfView = 60f;
        if (m_Player == null) m_Player = this.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_Player == this.transform) { return; }
        m_vPrePos = m_Player.position;
    }

    private void LateUpdate()
    {
        if (m_Player == this.transform) { return; }
        m_vCurPos = m_Player.position;
        if (!CameraMove()) { FollowTarget(m_vCurPos); }
    }

    public Vector3 SetTarget(Transform m_player)
    {
        return m_player.position;
    }

    private bool CameraMove()
    {
        if (m_vCurPos != m_vPrePos)
        {
            Vector3 vCurCamPos = m_Cam.transform.position;
            Vector3 Direc = m_vCurPos - m_vPrePos;
            Direc.Normalize();
            vCurCamPos.x = Mathf.Lerp(vCurCamPos.x, m_vCurPos.x + Direc.x * K + m_fPosX, L);
            vCurCamPos.z = Mathf.Lerp(vCurCamPos.z, m_vCurPos.z + Direc.z * K * 0.75f + m_fPosZ, L * 0.75f);
            m_Cam.transform.position = vCurCamPos;
            return true;
        }
        return false;
    }

    private void FollowTarget(Vector3 Target)
    {
        Vector3 vCurCamPos = m_Cam.transform.position;
        vCurCamPos.x = Mathf.Lerp(vCurCamPos.x, Target.x + m_fPosX, 2f * Time.deltaTime);
        vCurCamPos.y = Mathf.Lerp(vCurCamPos.y, Target.y + m_fPosY, 2f * Time.deltaTime);
        vCurCamPos.z = Mathf.Lerp(vCurCamPos.z, Target.z + m_fPosZ, 2f * Time.deltaTime);
        m_Cam.transform.position = vCurCamPos;
    }

    //Use to turn off the obstacle's mesh if which is on the way
    private IEnumerator CheckView()
    {
        yield return new WaitForSeconds(0.5f);
        if (Physics.Raycast(this.transform.position, m_Player.position - this.transform.position, 1000f, 1 << LayerMask.NameToLayer("Water")))
        {
            Collider[] AroundWall = Physics.OverlapSphere(m_Player.position, 20f, 1 << LayerMask.NameToLayer("Water"));
            foreach (Collider c in AroundWall)
            {
                c.gameObject.GetComponent<Renderer>().enabled = false;
                Debug.Log(AroundWall.Length);
            }
        }
        else
        {
            Collider[] AroundWall = Physics.OverlapSphere(m_Player.position, 20f, 1 << LayerMask.NameToLayer("Water"));
            foreach (Collider c in AroundWall)
            {
                c.gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(m_Player.position, 20f);
    //}
}