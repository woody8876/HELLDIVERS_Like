using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PlayerForward : MonoBehaviour {

    Vector3 m_MousePos;

    Ray r;
    RaycastHit rh;

    private GameObject LineRenderGameObject;
    private LineRenderer lineRenderer;
    private int lineLength = 2;
    // Use this for initialization
    void Start()
    {
        
        LineRenderGameObject = GameObject.Find("Line");
        lineRenderer = (LineRenderer)LineRenderGameObject.GetComponent("LineRenderer");
    }

    // Update is called once per frame
    void Update () {

        m_MousePos = Input.mousePosition;
        r = Camera.main.ScreenPointToRay(m_MousePos);

        if (Physics.Raycast(r, out rh,1000.0f))
        {
            Vector3 vTarget = rh.point;
            Vector3 vForward = vTarget - this.transform.position;
            vForward.y = 0.0f;

            vForward.Normalize();
            transform.forward = vForward;
        }
        if (Input.GetMouseButton(1))
        {
            LineRenderInit();
            lineRenderer.SetPosition(0, Vector3.forward * 100f);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            lineRenderer.enabled = false;
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
        Vector3 target;
        target = this.transform.forward*10 + this.transform.position;
        Gizmos.DrawLine(this.transform.position, target);

        Gizmos.DrawSphere(transform.position, 1f);
    }
}
