using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionBriefing : MonoBehaviour
{
    [SerializeField] private Transform m_PanelMap;
    [SerializeField] private UIMissionBriefingMap m_Map;
    [SerializeField] private UIMissionBriefingMapPoint m_MissionPoint;
    [SerializeField] private UIMissionBriefingMapPoint m_SpawnPoint;
    [SerializeField] private GameObject m_GOConcentric;

    public static UIMissionBriefing Instance { get; private set; }

    public UIMissionBriefingMap Map { get { return m_Map; } }
    public GameObject Concentric { get { return m_GOConcentric; } }
    public List<UIMissionBriefingMapPoint> PointList { get { return m_PointList; } }
    public UIMissionBriefingMapPoint HightLight { get { return m_HighLight; } set { m_HighLight = value; } }
    public UIMissionBriefingMapPoint Target { get { return m_Target; } set { m_Target = value; } }

    private List<UIMissionBriefingMapPoint> m_PointList;
    private UIMissionBriefingMapPoint m_HighLight;
    private UIMissionBriefingMapPoint m_Target;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        m_PointList = new List<UIMissionBriefingMapPoint>();
        m_Map = Instantiate(m_Map, m_PanelMap);
    }

    public void Init(MapInfo mapInfo)
    {
        for (int i = 0; i < mapInfo.SpawnPos.Count; i++)
        {
            AddPoint(mapInfo.SpawnPos[i].gameObject, eMapPointType.SPAWNPOINT);
            Debug.Log("Creata SpawnPoint");
        }
    }

    public void DrawUI()
    {
        this.transform.SetAsLastSibling();
    }

    public void AddPoint(GameObject target, eMapPointType type)
    {
        //UIMissionBriefingMapPoint mapPoint;
        //switch (type)
        //{
        //    case eMapPointType.MISSIONTOWER:
        //        mapPoint = Instantiate(m_MissionPoint, m_Map.MapRoot);
        //        mapPoint.Init(target, type);
        //        m_PointList.Add(mapPoint);
        //        break;
        //    case eMapPointType.SPAWNPOINT:
        //        mapPoint = Instantiate(m_SpawnPoint, m_Map.MapRoot);
        //        mapPoint.Init(target, type);
        //        m_PointList.Add(mapPoint);
        //        break;
        //}
    }

    public bool ComfirmSpawnPosition()
    {
        if (m_Target == null) return false;
        GameMain.Instance.GameStart(m_Target.CurrentTarget.transform);
        return true;
    }
}
