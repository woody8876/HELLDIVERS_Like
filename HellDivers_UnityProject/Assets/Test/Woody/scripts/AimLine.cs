using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    private Transform m_LineRenderTransform;
    private Transform m_Enitter;
    private LineRenderer m_LineRenderer;
    private GameObject m_LineRender;
    private int straightPosCount = 2;
    private int spinePosCount = 50;

    private void Start()
    {
        m_LineRender =  Resources.Load("LineRender") as GameObject;
        m_LineRender = Instantiate(m_LineRender, this.transform);
        m_LineRenderer = m_LineRender.GetComponent<LineRenderer>();
        
        SetAimLineInfo(true);
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
        else if (Input.GetKeyDown(KeyCode.X))
        {
            SetAimLineInfo(false);
        }
    }

    /// <summary>
    /// 設定直線或曲線
    /// </summary>
    /// <param name="straight"></param>
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
            m_LineRenderer.SetPosition(0, new Vector3(0,0,0));
            m_LineRenderer.SetPosition(1, (GetLastPosition()));
        }
        if (m_LineRenderer.positionCount == straightPosCount)
        {
            m_LineRenderer.SetPosition(0, m_Enitter.transform.localPosition);
            m_LineRenderer.SetPosition(1, GetLastPosition());

        }
        else if (m_LineRenderer.positionCount == spinePosCount)
        {
            Vector3 vPosition0 = m_Enitter.transform.localPosition;
            Vector3 vPosition1 = m_Enitter.transform.localPosition;
            Vector3 vPosition2 = (GetLastPosition() + m_Enitter.transform.localPosition) / 3;
            vPosition2.y = 10f;
            Vector3 vPosition3 = (GetLastPosition() + m_Enitter.transform.localPosition) / 3 * 2;
            vPosition3.y = 10f;
            Vector3 vPosition4 = GetLastPosition();
            float test = 50;

            for (int i = 0; i < spinePosCount; i++)
            {
                Vector3 firstPosLerp1 = Vector3.Lerp(vPosition0, vPosition1, i / test);
                Vector3 firstPosLerp2 = Vector3.Lerp(vPosition1, vPosition2, i / test);
                Vector3 firstPosLerp3 = Vector3.Lerp(vPosition2, vPosition3, i / test);
                Vector3 firstPosLerp4 = Vector3.Lerp(vPosition3, vPosition4, i / test);

                Vector3 secondPosLerp1 = Vector3.Lerp(firstPosLerp1, firstPosLerp2, i / test);
                Vector3 secondPosLerp2 = Vector3.Lerp(firstPosLerp2, firstPosLerp3, i / test);
                Vector3 secondPosLerp3 = Vector3.Lerp(firstPosLerp3, firstPosLerp4, i / test);

                Vector3 thirdPosLerp1 = Vector3.Lerp(secondPosLerp1, secondPosLerp2, i / test);
                Vector3 thirdPosLerp2 = Vector3.Lerp(secondPosLerp2, secondPosLerp3, i / test);

                Vector3 finalPos = Vector3.Lerp(thirdPosLerp1, thirdPosLerp2, i / test);

                m_LineRenderer.SetPosition(i, finalPos);
            }
        }
    }
    private void CloseAimLine()
    {
        m_LineRenderer.enabled = false;
    }

    private Vector3 GetLastPosition()
    {
        if (m_LineRenderer.positionCount == straightPosCount)
        {
            return new Vector3(0, 0, 50);
        }
        else if (m_LineRenderer.positionCount == spinePosCount)
        {
            return new Vector3(0, 0, 10);
        }
        return new Vector3(0, 0, 0);
    }
}

