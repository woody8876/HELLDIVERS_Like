using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    private Transform m_LineRenderTransform;
    private Transform m_Enitter;
    private LineRenderer m_LineRenderer;
    private int straightPosCount = 2;
    private int spinePosCount = 50;

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
            OpenAimLine();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            CloseAimLine();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetAimLineInfo(true);
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            SetAimLineInfo(false);
        }
    }

    public void SetAimLineInfo(bool straight)
    {
        m_LineRenderer.positionCount = straight ? straightPosCount : spinePosCount;
    }

    private void OpenAimLine()
    {
        m_LineRenderer.enabled = true;
        m_Enitter = transform.Find("Emitter");

        if (m_Enitter == null)
        {
            Debug.Log("Can't find the Emitter");
            m_LineRenderer.SetPosition(0, this.transform.position);
            m_LineRenderer.SetPosition(1, (SetLineLength()));
        }
        else if(m_LineRenderer.positionCount == straightPosCount)
        {
            m_LineRenderer.SetPosition(0, m_Enitter.transform.localPosition);
            m_LineRenderer.SetPosition(1, (SetLineLength()));
        }
        else if (m_LineRenderer.positionCount == spinePosCount)
        {
            Vector3 vPosition0;
            Vector3 vPosition1;
            Vector3 vPosition2;
            Vector3 vPosition3;
            Vector3 vPosition4;

            vPosition0 = m_Enitter.transform.localPosition - m_Enitter.transform.forward;
            vPosition1 = m_Enitter.transform.localPosition;
            vPosition2 = (SetLineLength() + m_Enitter.transform.localPosition) / 3;
            vPosition2.y = 10f;
            vPosition3 = (SetLineLength() + m_Enitter.transform.localPosition) / 3 * 2;
            vPosition3.y = 10f;
            vPosition4 = SetLineLength();

            float test=50f;

            for (int i = 0; i < spinePosCount; i++)
            {
                Vector3 pos1 = Vector3.Lerp(vPosition0, vPosition1, i / test);
                Vector3 pos2 = Vector3.Lerp(vPosition1, vPosition2, i / test);
                Vector3 pos3 = Vector3.Lerp(vPosition2, vPosition3, i / test);
                Vector3 pos4 = Vector3.Lerp(vPosition3, vPosition4, i / test);

                var pos1_0 = Vector3.Lerp(pos1, pos2, i / test);
                var pos1_1 = Vector3.Lerp(pos2, pos3, i / test);
                var pos1_2 = Vector3.Lerp(pos3, pos4, i / test);

                var pos2_0 = Vector3.Lerp(pos1_0, pos1_1, i / test);
                var pos2_1 = Vector3.Lerp(pos1_1, pos1_2, i / test);

                Vector3 find = Vector3.Lerp(pos2_0, pos2_1, i / 25f);

                m_LineRenderer.SetPosition(i, find);
            }
            //m_LineRenderer.SetPosition(0, m_Enitter.transform.localPosition);
            //m_LineRenderer.SetPosition(1, vPosition1);
            //m_LineRenderer.SetPosition(2, vPosition2);
            //m_LineRenderer.SetPosition(3, (SetLineLength()));
        }
    }
    private void CloseAimLine()
    {
        m_LineRenderer.enabled = false;
    }

    private Vector3 SetLineLength()
    {
        if (m_LineRenderer.positionCount == straightPosCount)
        {
            return new Vector3(0, 0, 50f);

        }
        else if (m_LineRenderer.positionCount == spinePosCount)
        {
            Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit MouseHit;

            if (Physics.Raycast(MouseRay, out MouseHit, 1000.0f, 1 << LayerMask.NameToLayer("Terrain")))
            {
                Vector3 mPos = MouseHit.point;
                mPos.y = 0;
                Vector3 ePos = this.transform.position;
                ePos.y = 0;
                Vector3 Distance = mPos - ePos;

                return new Vector3(0, 0, Distance.magnitude);
            }
        }

        return new Vector3(0, 0, 0);
    }
}

