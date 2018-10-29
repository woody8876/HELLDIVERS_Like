using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnMobs : MonoBehaviour
{

    public float m_SpawnTime = 3f;
    private GameObject[] m_SpawnPoints;
    public GameObject m_Go;
    // Use this for initialization
    void Start()
    {
        //m_Go = Resources.Load("FishAI") as GameObject;
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("FishAI"), 20, 3001);
        m_SpawnPoints = GameObject.FindGameObjectsWithTag("MobSpawnPoint");
        InvokeRepeating("SpawnEnemy", 0.0f, m_SpawnTime);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, m_SpawnPoints.Length);
        for (int i = 0; i < 3; i++)
        {
            m_Go = ObjectPool.m_Instance.LoadGameObjectFromPool(3001);
            if (m_Go == null) return;
            m_Go.SetActive(true);
            m_Go.transform.position = m_SpawnPoints[spawnIndex].transform.position;
        }
    }
}
