using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnMobs
{
    static public SpawnMobs m_Instance;

    //private float m_SpawnTime = 3f;
    private float m_SpawnStartTime = 0.0f;
    private static int m_FishCount;
    private static int m_PatrolCount;
    private GameObject m_GoFish;
    private GameObject m_GoPatrol;

    public void Init()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }

        m_GoFish = Resources.Load("Mobs/Fish/Fish") as GameObject;
        m_GoPatrol = Resources.Load("Mobs/Patrol/Patrol") as GameObject;
        ObjectPool.m_Instance.InitGameObjects(m_GoFish, 40, 3001);
        ObjectPool.m_Instance.InitGameObjects(m_GoPatrol, 10, 3002);
    }

    //void Start()
    //{
    //    ObjectPool.m_Instance.InitGameObjects(m_GoFish, 40, 3001);
    //    ObjectPool.m_Instance.InitGameObjects(m_GoPatrol, 10, 3002);

    //    if (MapInfo.Instance != null)
    //    {
    //        m_SpawnPoints  = MapInfo.Instance.MobPos.ToArray();
    //    }
    //    //m_SpawnPoints = GameObject.FindGameObjectsWithTag("MobSpawnPoint");
    //    //InvokeRepeating("SpawnFish", m_SpawnStartTime, m_SpawnTime);
    //    SpawnPatrol();
    //}
    
    public void SpawnPatrol(int num)
    {
        if (MapInfo.Instance == null) return;
        if (MapInfo.Instance.MobPos.Count <= 0) return;

        int spawnIndex;
        for (int i = 0; i < num; i++)
        {
            spawnIndex = Random.Range(0, MapInfo.Instance.MobPos.Count - 1);
            m_GoPatrol = ObjectPool.m_Instance.LoadGameObjectFromPool(3002);
            if (m_GoPatrol == null) return;
            m_GoPatrol.SetActive(true);
            m_GoPatrol.transform.position = MapInfo.Instance.MobPos[spawnIndex].position;
        }
    }

    public void SpawnFish(int num)
    {
        if (MapInfo.Instance == null) return;
        if (MapInfo.Instance.MobPos.Count <= 0) return;

        if (m_FishCount > 20) return;
        //m_FishCount = GameObject.FindGameObjectsWithTag("Fish");
        //if (m_FishCount.Length >= 20) return;

        int spawnIndex = Random.Range(0, MapInfo.Instance.MobPos.Count);

        for (int i = 0; i < num; i++)
        {
            m_GoFish = ObjectPool.m_Instance.LoadGameObjectFromPool(3001);
            if (m_GoFish == null) return;
            m_GoFish.SetActive(true);
            m_GoFish.transform.position = MapInfo.Instance.MobPos[spawnIndex].position;
            m_FishCount++;
        }
    }
}
