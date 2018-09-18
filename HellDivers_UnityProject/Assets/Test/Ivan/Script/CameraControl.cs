using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform m_Player;
    Camera m_Cam;
    Ray m_rCam2Target; 
// Use this for initialization
void Start () {
        m_Cam = GetComponent<Camera>();
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_Cam.transform.position = new Vector3( m_Player.position.x - 64.0f, m_Player.position.y + 88.0f, m_Player.position.z - 56.5f);
        m_Cam.transform.rotation = Quaternion.Euler(45, 48, 0);
        m_rCam2Target = new Ray(this.transform.position, this.transform.forward);

    }
	
	// Update is called once per frame
	void Update () {
        FollowTarget(m_Player);
        StartCoroutine(CheckView());
    }

    private void FollowTarget(Transform Target)
    {
        Vector3 vCurCamPos = m_Cam.transform.position;
        float fMovingDis = (vCurCamPos - Target.position).magnitude;
        if (Mathf.Abs(fMovingDis ) > 5f)
        {
            vCurCamPos.x = Mathf.Lerp(vCurCamPos.x, Target.position.x - 64.0f, 10f * Time.deltaTime);
            vCurCamPos.y = Mathf.Lerp(vCurCamPos.y, Target.position.y + 88.0f, 10f * Time.deltaTime);
            vCurCamPos.z = Mathf.Lerp(vCurCamPos.z, Target.position.z - 56.5f, 10f * Time.deltaTime);
            m_Cam.transform.position = vCurCamPos;
        }
    }

    IEnumerator CheckView()
    {
        yield return new WaitForSeconds(0.5f);
        if (Physics.Raycast(this.transform.position, this.transform.forward, (m_Player.position - this.transform.position).magnitude, 1 << LayerMask.NameToLayer("Water")))
        {
            Collider[] AroundWall = Physics.OverlapSphere(m_Player.position, 20f, 1 << LayerMask.NameToLayer("Water"));
            Vector3 vTarget2Cam = this.transform.position - m_Player.position;
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
                Debug.Log(AroundWall.Length);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_Player.position, 20f);
    }
}
