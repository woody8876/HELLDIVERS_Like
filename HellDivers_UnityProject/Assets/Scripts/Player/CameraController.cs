using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Transform m_Player;
    public float m_fPosX = 64.0f;
    public float m_fPosY = 88.0f;
    public float m_fPosZ = 56.5f;
    public float m_fRotX = 45.0f;
    public float m_fRotY = 48.0f;
    public float m_fRotZ = 0.0f;

    Camera m_Cam;
// Use this for initialization
void Start () {
        m_Cam = GetComponent<Camera>();
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_Cam.transform.rotation = Quaternion.Euler(m_fRotX, m_fRotY, m_fRotZ);
        m_Cam.fieldOfView = 15f;
    }
	
	// Update is called once per frame
	void Update () {
        FollowTarget(m_Player);
        StartCoroutine(CheckView());
    }

    private void FollowTarget(Transform Target)
    {
        Vector3 vCurCamPos = m_Cam.transform.position;
        vCurCamPos.x = Mathf.Lerp(vCurCamPos.x, Target.position.x - m_fPosX, 3f * Time.deltaTime);
        vCurCamPos.y = Mathf.Lerp(vCurCamPos.y, Target.position.y + m_fPosY, 3f * Time.deltaTime);
        vCurCamPos.z = Mathf.Lerp(vCurCamPos.z, Target.position.z - m_fPosZ, 3f * Time.deltaTime);
        m_Cam.transform.position = vCurCamPos;
    }

    IEnumerator CheckView()
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
            foreach(Collider c in AroundWall)
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
