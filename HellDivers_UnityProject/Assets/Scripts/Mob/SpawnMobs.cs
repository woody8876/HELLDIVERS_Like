using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnMobs : MonoBehaviour
{
    static public SpawnMobs m_Instance;

    private float m_SpawnTime = 3f;
    private float m_SpawnStartTime = 0.0f;
    private GameObject[] m_SpawnPoints;
    private GameObject m_GoFish;
    private GameObject m_GoPatrol;
    private GameObject[] m_FishCount;

    public void Init()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }  
    }

    void Start()
    {
        m_GoFish = Resources.Load("Mobs/Fish/Fish") as GameObject;
        m_GoPatrol = Resources.Load("Mobs/Patrol/Patrol") as GameObject;

        ObjectPool.m_Instance.InitGameObjects(m_GoFish, 40, 3001);
        ObjectPool.m_Instance.InitGameObjects(m_GoPatrol, 10, 3002);

        m_SpawnPoints = GameObject.FindGameObjectsWithTag("MobSpawnPoint");
        //InvokeRepeating("SpawnFish", m_SpawnStartTime, m_SpawnTime);
        SpawnPatrol();
    }
    
    private void SpawnPatrol()
    {
        int spawnIndex;
        for (int i = 0; i < 10; i++)
        {
            spawnIndex = Random.Range(0, m_SpawnPoints.Length);
            m_GoPatrol = ObjectPool.m_Instance.LoadGameObjectFromPool(3002);
            if (m_GoPatrol == null) return;
            m_GoPatrol.SetActive(true);
            m_GoPatrol.transform.position = m_SpawnPoints[spawnIndex].transform.position;
        }
    }
    public void SpawnFish()
    {
        m_FishCount = GameObject.FindGameObjectsWithTag("Fish");
        if (m_FishCount.Length >= 20) return;

        int spawnIndex = Random.Range(0, m_SpawnPoints.Length);
        for (int i = 0; i < 5; i++)
        {
            m_GoFish = ObjectPool.m_Instance.LoadGameObjectFromPool(3001);
            if (m_GoFish == null) return;
            m_GoFish.SetActive(true);
            m_GoFish.transform.position = m_SpawnPoints[spawnIndex].transform.position;
        }
    }
}
