using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAimLine : MonoBehaviour
{
    #region private variable
    private GameObject m_GoLineRender;
    public LineRenderer m_LineRender;
    private int straightPosCount = 4;
    private int spinePosCount = 50;
    #endregion

    private void Start()
    {
        m_GoLineRender = Resources.Load("Mobs/MobLineRender") as GameObject;
        m_GoLineRender = Instantiate(m_GoLineRender, this.transform);

        m_LineRender = m_GoLineRender.GetComponent<LineRenderer>();
        m_LineRender.useWorldSpace = false;
        SetAimLineInfo(true);
    }
    private void Update()
    {

    }

    /// <summary>
    /// 設定直線或曲線
    /// </summary>
    /// <param name="straight"></param>
    public void SetAimLineInfo(bool straight)
    {
        m_LineRender.positionCount = straight ? straightPosCount : spinePosCount;
    }

    public void OpenAimLine()
    {

        m_LineRender.enabled = true;
        Vector3 vPosition0 = new Vector3(0, 0.5f, 0);
        Vector3 vPosition1 = (GetLastPosition() + vPosition0) * 0.2f;
        Vector3 vPosition2 = (GetLastPosition() + vPosition0) * 0.8f;
        Vector3 vPosition3 = GetLastPosition();

        m_LineRender.SetPosition(0, vPosition0);
        m_LineRender.SetPosition(1, vPosition1);
        m_LineRender.SetPosition(2, vPosition2);
        m_LineRender.SetPosition(3, vPosition3);
    }
    public void CloseAimLine()
    {
        if (m_LineRender == null) return;
        m_LineRender.enabled = false;
    }

    public Vector3 GetLastPosition()
    {

        //RaycastHit rh;
        //if (Physics.Raycast(this.transform.position, this.transform.forward, out rh, 50f, 1 << LayerMask.NameToLayer("Enemies")))
        //{
        //    float fDis = (rh.point - this.transform.position).magnitude;
        //    return new Vector3(0, m_Enitter.localPosition.y, fDis);
        //}
        return new Vector3(0 ,0.5f , 50);
    }
}
