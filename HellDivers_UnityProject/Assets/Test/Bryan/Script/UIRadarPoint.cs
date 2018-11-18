using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRadarPoint : MonoBehaviour
{
    private Vector3 m_Pos = new Vector3();
    private Vector3 m_Center = new Vector3();
    private Vector3 m_Dir = new Vector3();
    [SerializeField] private Image m_Image;
    private GameObject m_CurrentTarget;

    public void Init(GameObject target)
    {
        m_CurrentTarget = target;
        // m_RectHeight / m_RadarRadius...
    }

    // Use this for initialization
    void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {
        FindRadarCenter();

        m_Dir = m_CurrentTarget.transform.position - m_Center;
        m_Pos = this.transform.localPosition;
        m_Pos.x = m_Dir.x * (UIPanelRadar.Instance.RectWidth * 0.5f) / UIPanelRadar.Instance.RadarRadius;
        m_Pos.y = m_Dir.z * (UIPanelRadar.Instance.RectHeight * 0.5f) / UIPanelRadar.Instance.RadarRadius;
        this.transform.localPosition = m_Pos;
    }

    public void FindRadarCenter()
    {
        List<Player> pList = InGamePlayerManager.Instance.Players;
        if (pList == null) return;
        m_Center.Set(0, 0, 0);
        for (int i = 0; i < pList.Count; i++)
        {
            m_Center += pList[i].transform.position;
        }
        m_Center /= pList.Count;
    }
}
