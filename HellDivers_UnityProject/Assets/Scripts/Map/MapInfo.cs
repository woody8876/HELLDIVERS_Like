using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public static MapInfo Instance { get; private set; }

    public List<Transform> SpawnPos { get { return m_SpawnPos; } }
    public List<Transform> TowerPos { get { return m_TowerPos; } }
    public List<Transform> MobPos { get { return m_MobPos; } }
    [SerializeField] private List<Transform> m_SpawnPos;
    [SerializeField] private List<Transform> m_TowerPos;
    [SerializeField] private List<Transform> m_MobPos;

    public Transform GetRandomSpawnPos()
    {
        if (m_SpawnPos != null || m_SpawnPos.Count > 0)
        {
            int rand = Random.Range(0, m_SpawnPos.Count - 1);
            return m_SpawnPos[rand];
        }
        return null;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    [ContextMenu("Auto Scan")]
    private void AutoScan()
    {
        AutoScanPositions("SpawnPosGroup", out m_SpawnPos);
        AutoScanPositions("TowerPosGroup", out m_TowerPos);
        AutoScanPositions("MobPosGroup", out m_MobPos);
    }

    private void AutoScanPositions(string rootName, out List<Transform> container)
    {
        container = new List<Transform>();
        GameObject positionGroupRoot = GameObject.Find(rootName);

        if (positionGroupRoot == null)
        {
            Debug.LogWarningFormat("{0} doesn't exist.", rootName);
            return;
        }

        Transform[] positions = positionGroupRoot.GetComponentsInChildren<Transform>();
        if (positions != null)
        {
            for (int i = 1; i < positions.Length; i++)
            {
                container.Add(positions[i]);
            }
        }
    }
}