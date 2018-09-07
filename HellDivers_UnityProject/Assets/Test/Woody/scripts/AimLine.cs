using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    private Transform m_LineRenderTransform;
    private Transform m_Enitter;
    private LineRenderer m_LineRenderer;

    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            SetAimLineInfo(false, 0.1f, 0.1f, 2);
            OpenAimLine();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            CloseAimLine();
        }
    }

    public void OpenAimLine()
    {
        m_LineRenderer.enabled = true;
        m_Enitter = transform.Find("Emitter");

        if (m_Enitter == null)
        {
            Debug.Log("Can't find the Emitter");
            m_LineRenderer.SetPosition(0, this.transform.position);
            m_LineRenderer.SetPosition(0, (SetLineLength()));
            Debug.Log("Line");
        }
        else
        {
            m_LineRenderer.SetPosition(0, m_Enitter.transform.localPosition);
            m_LineRenderer.SetPosition(1, (SetLineLength()));
        }
    }
    public void CloseAimLine()
    {
        m_LineRenderer.enabled = false;
    }

    public void SetAimLineInfo(bool useWorld, float startWidth, float endWidth, int positionCount)
    {
        m_LineRenderTransform = transform.Find("LineRender");
        if (m_LineRenderTransform == null)
        {
            Debug.Log("Can't find the LineRender");
        }
        else
        {
            m_LineRenderer = (LineRenderer)m_LineRenderTransform.GetComponent("LineRenderer");
        }
        m_LineRenderer.useWorldSpace = useWorld;
        m_LineRenderer.startWidth = startWidth;
        m_LineRenderer.endWidth = endWidth;
        m_LineRenderer.positionCount = positionCount;
    }
    public Vector3 SetLineLength()
    {
        if (m_LineRenderer.positionCount == 2)
        {
            //return new Vector3(0, 0, 50f);
            Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit MouseHit;

            if (Physics.Raycast(MouseRay, out MouseHit, 1000.0f, 1 << LayerMask.NameToLayer("Terrain")))
            {
                Vector3 Distance = MouseHit.point - m_Enitter.transform.position;
                Vector3 tPos = m_Enitter.transform.localPosition;
                tPos.z += Distance.magnitude;
                return tPos;
            }
        }
        else if (m_LineRenderer.positionCount == 4)
        {
        }


        return new Vector3(0, 0, 0);
    }
}

