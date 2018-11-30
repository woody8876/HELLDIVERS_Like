using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionBriefing : MonoBehaviour
{
    [SerializeField] private Transform m_PanelMap;
    [SerializeField] private UIMissionBriefingMap m_Map;
    [SerializeField] private GameObject m_GOConcentric;

    public static UIMissionBriefing Instance { get; private set; }

    public UIMissionBriefingMap Map { get { return m_Map; } }
    public GameObject Concentric { get { return m_GOConcentric; } }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        
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
        m_Map.AddPointPrefab(target, type);
    }

    public bool ComfirmSpawnPosition()
    {
        if (m_Map.ComfirmSpawnPosition()) Destroy(this.gameObject);
        return true;
    }
}
