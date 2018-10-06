using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float m_fPosY = 10.0f;
    public float m_fRotX = 60.0f;
    private float fRotateX;

    public float K;
    public float L;

    public Transform m_Player;
    private Camera m_Cam;
    private Vector3 m_vVec;
    private Vector3 m_vCurPos;

    // Use this for initialization
    private void Start()
    {
        m_Cam = Camera.main.GetComponent<Camera>();
        m_Cam.transform.rotation = Quaternion.Euler(m_fRotX, 0, 0);
        fRotateX = Mathf.Tan((90 - m_fRotX) * Mathf.Deg2Rad) ;
        Debug.Log(fRotateX * m_fPosY);

        m_Cam.fieldOfView = 60f;
        if (m_Player == null) m_Player = this.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_Player == this.transform) { return; }
        m_vCurPos = m_Player.position;
    }
    private void LateUpdate()
    {
        if (m_Player == this.transform) { return; }
        FollowTarget(m_vCurPos); 
    }
    

    public Vector3 SetTarget(Transform m_player, int[] a)
    {
        return m_player.position;
    }

    private Vector3 CameraMove()
    {
        m_vVec.x = Input.GetAxis("Horizontal") * K;
        float z = Input.GetAxis("Vertical") * K * 0.75f;
        m_vVec.z = (z > 0) ? z : z * 1.25f;
        return m_vVec;
    }

    private void FollowTarget(Vector3 Target)
    {
        Vector3 vCurCamPos = m_Cam.transform.position;
        float fzPos = -fRotateX * (m_fPosY-1f);
        vCurCamPos.x = Mathf.Lerp(vCurCamPos.x, Target.x + CameraMove().x, 2f * Time.deltaTime);
        vCurCamPos.y = Mathf.Lerp(vCurCamPos.y, Target.y + CameraMove().y + m_fPosY, 2f * Time.deltaTime);
        vCurCamPos.z = Mathf.Lerp(vCurCamPos.z, Target.z + CameraMove().z + fzPos, 2f * Time.deltaTime);
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