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
        m_LineRenderTransform = transform.Find("LineRender");
        if (m_LineRenderTransform == null)
        {
            Debug.Log("Can't find the LineRender");
        }
        else
        {
            m_LineRenderer = (LineRenderer)m_LineRenderTransform.GetComponent("LineRenderer");
        }
    }
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            SetAimLineInfo(true);
            OpenAimLine();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            CloseAimLine();
        }
    }
    public void SetAimLineInfo(bool straight)
    {
        m_LineRenderer.positionCount = straight ? 2 : 4;
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
                Vector3 mPos = MouseHit.point;
                mPos.y = 0;
                Vector3 ePos = m_Enitter.transform.localPosition;
                ePos.y = 0;
                Vector3 Distance = mPos - ePos;

                return new Vector3(0, 0, Distance.magnitude);
            }
        }
        else if (m_LineRenderer.positionCount == 4)
        {
        }
        
        return new Vector3(0, 0, 0);
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
            Vector3 Emitter = m_Enitter.transform.localPosition;
            Emitter.y = 0;
            m_LineRenderer.SetPosition(0, Emitter);
            m_LineRenderer.SetPosition(1, (SetLineLength()));
        }
    }
    public void CloseAimLine()
    {
        m_LineRenderer.enabled = false;
    }

}

