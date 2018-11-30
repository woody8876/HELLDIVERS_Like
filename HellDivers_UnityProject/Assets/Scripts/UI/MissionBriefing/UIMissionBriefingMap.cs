using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionBriefingMap : MonoBehaviour
{
    public Transform MapPointRoot { get { return m_MapPointRoot; } }

    [SerializeField] private Transform m_MapPointRoot;
    [SerializeField] private UIMissionBriefingConcentric m_Concentric;
    [SerializeField] private UIMissionMapPoint m_SpawnPointPrefab;
    [SerializeField] private UIMissionMapPoint m_TowerPointPrefab;

    #region Variable
    private RectTransform m_RectTransform;
    private Vector3 m_WorldOrigin;
    private float m_RectWidth;
    private float m_RectHeight;
    private float m_MapWidth = 544.0f;
    private float m_MapHeight = 720.0f;

    private List<UIMissionMapPoint> m_PointList = new List<UIMissionMapPoint>();
    #endregion
    
    // Use this for initialization
    void Awake()
    {
        m_WorldOrigin = MapInfo.Instance.MapOrigin.position;
        m_RectTransform = this.GetComponent<RectTransform>();
        m_RectWidth = m_RectTransform.sizeDelta.x;
        m_RectHeight = m_RectTransform.sizeDelta.y;
    }

    public void AddPointPrefab(GameObject target, eMapPointType type)
    {
        UIMissionMapPoint mapPoint = null;
        switch (type)
        {
            case eMapPointType.MISSIONTOWER:
                mapPoint = Instantiate(m_TowerPointPrefab, m_MapPointRoot);
                m_PointList.Add(mapPoint);
                break;
            case eMapPointType.SPAWNPOINT:
                mapPoint = Instantiate(m_SpawnPointPrefab, m_MapPointRoot);
                m_PointList.Add(mapPoint);
                break;
        }

        if (mapPoint != null) CalculatePosition(target.transform.position, ref mapPoint);
    }

    public bool Select()
    {
        if (UIMissionBriefing.Instance.HightLight == null) return false;

        UIMissionBriefing.Instance.Target = UIMissionBriefing.Instance.HightLight;
        UIMissionBriefing.Instance.HightLight = null;
        return true;
    }

    public void SetHightLight(UIMissionBriefingMapPoint mapPoint)
    {
        UIMissionBriefing.Instance.HightLight = mapPoint;
    }
    public void DeleteHighLight(UIMissionBriefingMapPoint mapPoint)
    {
        if (UIMissionBriefing.Instance.HightLight != mapPoint) return;

        UIMissionBriefing.Instance.HightLight = null;
    }
    

    private void CalculatePosition(Vector3 target,ref UIMissionMapPoint mapPoint)
    {
        Vector3 m_Dir = target - m_WorldOrigin;
        float mapWidth = m_MapPointRoot.GetComponent<RectTransform>().sizeDelta.x;
        float mapHeight = m_MapPointRoot.GetComponent<RectTransform>().sizeDelta.y;

        Vector3 m_Pos = mapPoint.transform.localPosition;
        m_Pos.x = m_Dir.x * mapWidth / m_MapWidth - mapWidth * 0.5f;
        m_Pos.y = m_Dir.z * mapHeight / m_MapHeight - mapHeight * 0.5f;
        mapPoint.transform.localPosition = m_Pos;
    }
}
