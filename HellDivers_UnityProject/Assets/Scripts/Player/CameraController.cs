using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Camera Height
    public float m_fPosY = 10.0f;
    //Camera Rotate
    public float m_fRotX = 60.0f;
    private float fRotateX;

    //Camera move ratio while walk
    public float m_fWalkCamMove;
    //Camera move ratio while Run or Aim
    public float m_fRunCamMove;
    //Camera lerp ratio
    public float m_CamLerp;

    public Transform m_Player;
    private Camera m_Cam;
    private Vector3 m_vVec;

    // Use this for initialization
    private void Start()
    {
        m_Cam = Camera.main.GetComponent<Camera>();
        m_Cam.transform.rotation = Quaternion.Euler(m_fRotX, 0, 0);
        fRotateX = Mathf.Tan((90 - m_fRotX) * Mathf.Deg2Rad) ;

        m_Cam.fieldOfView = 60f;
        if (m_Player == null) m_Player = this.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_Player == this.transform) { return; }
        FollowTarget(m_Player.position);
    }

    public Vector3 SetTarget(Transform m_player, int[] a)
    {
        return m_player.position;
    }

    private Vector3 CameraMove(Transform target)
    {
        if (!AimState(target))
        {
            float fH = Input.GetAxis("Horizontal");
            float fV = Input.GetAxis("Vertical");
            if (Input.GetButton("Run"))
            {
                fH *= m_fRunCamMove;
                fV *= m_fRunCamMove;
            }
            else
            {
                fH *= m_fWalkCamMove;
                fV *= m_fWalkCamMove;
            }
            m_vVec.x = fH;
            m_vVec.z = (fV > 0) ? fV : fV * 1.25f;
        }
        return m_vVec;
    }
    private bool AimState(Transform target)
    {
        if (Input.GetMouseButton(1))
        {
            m_vVec.x = target.forward.x * m_fRunCamMove;
            m_vVec.z = target.forward.z * m_fRunCamMove * 0.75f;
            return true;
        }
        return false;
    }

    private void FollowTarget(Vector3 Target)
    {
        Vector3 vCurCamPos = m_Cam.transform.position;
        float fzPos = -fRotateX * (m_fPosY-1f);
        vCurCamPos.x = Mathf.Lerp(vCurCamPos.x, Target.x + CameraMove(m_Player).x, m_CamLerp);
        vCurCamPos.y = Mathf.Lerp(vCurCamPos.y, Target.y + CameraMove(m_Player).y + m_fPosY, m_CamLerp);
        vCurCamPos.z = Mathf.Lerp(vCurCamPos.z, Target.z + CameraMove(m_Player).z + fzPos, m_CamLerp);
        m_Cam.transform.position = vCurCamPos;
    }

    //Use to turn off the obstacle's mesh if which is on the way
    //private IEnumerator CheckView()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    if (Physics.Raycast(this.transform.position, m_Player.position - this.transform.position, 1000f, 1 << LayerMask.NameToLayer("Water")))
    //    {
    //        Collider[] AroundWall = Physics.OverlapSphere(m_Player.position, 20f, 1 << LayerMask.NameToLayer("Water"));
    //        foreach (Collider c in AroundWall)
    //        {
    //            c.gameObject.GetComponent<Renderer>().enabled = false;
    //            Debug.Log(AroundWall.Length);
    //        }
    //    }
    //    else
    //    {
    //        Collider[] AroundWall = Physics.OverlapSphere(m_Player.position, 20f, 1 << LayerMask.NameToLayer("Water"));
    //        foreach (Collider c in AroundWall)
    //        {
    //            c.gameObject.GetComponent<Renderer>().enabled = true;
    //        }
    //    }
    //}
}