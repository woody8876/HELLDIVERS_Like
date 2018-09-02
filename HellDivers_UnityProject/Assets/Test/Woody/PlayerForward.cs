using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForward : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        Vector3 m_MousePos = Input.mousePosition;
        Ray r = Camera.main.ScreenPointToRay(m_MousePos);
        RaycastHit rh;

        if (Physics.Raycast(r, out rh,1000.0f))
        {
            Vector3 vFinal = rh.point;
            Vector3 v = vFinal - this.transform.position;
            v.y = 0.0f;

            v.Normalize();
            transform.forward = v;
        }
    }
    //public void OnDrawGizmos()
    //{
    //    if (Input.GetMouseButton(1))
    //    {
    //        Vector3 vPos = this.transform.position;
    //        Vector3 vFor = this.transform.forward;
            
    //        Vector3 target = vPos + vFor;

    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(this.transform.position, target);
    //    }
    //}
}
