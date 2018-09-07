using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine
{
    private GameObject m_LineRenderGameObject;
    //private GameObject m_Enitter;
    private LineRenderer m_LineRenderer;


    public void SetAimLine()
    {
        //Vector3 Local = m_Enitter.transform.localPosition;
        m_LineRenderer.enabled = true;
        //m_LineRenderer.SetPosition(0, Local);
        m_LineRenderer.SetPosition(0, Vector3.forward * 50f);
    }
    public void CloseAimLine()
    {
        m_LineRenderer.enabled = false;
    }

    public void Init()
    {
        m_LineRenderGameObject = GameObject.Find("LineRender");
        m_LineRenderer = (LineRenderer)m_LineRenderGameObject.GetComponent("LineRenderer");
        //m_Enitter = GameObject.Find("Emitter");
        m_LineRenderer.useWorldSpace = false;
        m_LineRenderer.startWidth = 0.1f;
        m_LineRenderer.endWidth = 0.1f;
        m_LineRenderer.positionCount = 2;
    }
}

