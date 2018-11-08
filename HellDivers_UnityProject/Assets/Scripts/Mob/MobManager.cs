using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MobManager
{
    static public MobManager m_Instance;

    private int m_FishCount;
    private int m_PatrolCount;
    private int m_TotalKill;
    private int m_TotalFishKill;
    private int m_TotalPatrolKill;
    public int TotalKill { get { return m_TotalKill; } private set { } }
    public int TotalFishKill { get { return m_TotalFishKill; } private set { } }
    public int TotalPatrolKill { get { return m_TotalPatrolKill; } private set { } }
    private GameObject m_GoPlayer;
    private GameObject m_GoFish;
    private GameObject m_GoPatrol;
    private GameObject m_GoWarning;
    private GameObject m_GoBullet;

    public void Init()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }

        m_GoFish = Resources.Load("Mobs/Fish/Fish") as GameObject;
        m_GoPatrol = Resources.Load("Mobs/Patrol/Patrol") as GameObject;
        m_GoBullet = Resources.Load("Mobs/MobBullet") as GameObject;
        m_GoWarning = Resources.Load("Mobs/EnemyAlert") as GameObject;
        ObjectPool.m_Instance.InitGameObjects(m_GoFish, 40, 3100);
        ObjectPool.m_Instance.InitGameObjects(m_GoPatrol, 40, 3200);
        ObjectPool.m_Instance.InitGameObjects(m_GoBullet, 20, 3201);
        ObjectPool.m_Instance.InitGameObjects(m_GoWarning, 5, 3210);
        m_TotalKill = 0;
        m_TotalFishKill = 0;
        m_TotalPatrolKill = 0;
    }

    public void DecreaseFishCount()
    {
        m_TotalKill++;
        m_TotalFishKill++;
        m_FishCount--;
    }
    public void DecreasePatrolCount()
    {
        m_TotalKill++;
        m_TotalPatrolKill++;
        m_PatrolCount--;
    }
    public void SpawnPatrol(int num)
    {
        if (MapInfo.Instance == null) return;
        if (MapInfo.Instance.MobPos.Count <= 0) return;

        int spawnIndex;
        for (int i = 0; i < num; i++)
        {
            spawnIndex = Random.Range(0, MapInfo.Instance.MobPos.Count);
            m_GoPatrol = ObjectPool.m_Instance.LoadGameObjectFromPool(3200);
            if (m_GoPatrol == null) return;
            m_GoPatrol.SetActive(true);
            m_GoPatrol.transform.position = MapInfo.Instance.MobPos[spawnIndex].position;
            m_PatrolCount++;
        }
    }

    public void SpawnFish(int num)
    {
        if (MapInfo.Instance == null) return;
        if (MapInfo.Instance.MobPos.Count <= 0) return;
        if (m_GoPlayer == null)
        {
            m_GoPlayer = GameObject.FindGameObjectWithTag("Player");
        }

        if (m_FishCount > 10) return;
        int BestSpawnIndex = 0;
        float BestDist = 0;

        for (int index = 0; index < MapInfo.Instance.MobPos.Count; index++)
        {
            if (index == 0)
            {
                BestDist = (MapInfo.Instance.MobPos[index].position - m_GoPlayer.transform.position).magnitude;
                BestSpawnIndex = index;
                continue;
            }
            if ((MapInfo.Instance.MobPos[index].position - m_GoPlayer.transform.position).magnitude < 20f)
            {
                continue;
            }
            if ((MapInfo.Instance.MobPos[index].position - m_GoPlayer.transform.position).magnitude < BestDist)
            {
                BestDist = (MapInfo.Instance.MobPos[index].position - m_GoPlayer.transform.position).magnitude;
                BestSpawnIndex = index;
            }
        }
        for (int i = 0; i < num; i++)
        {
            m_GoFish = ObjectPool.m_Instance.LoadGameObjectFromPool(3100);
            if (m_GoFish == null) return;
            m_GoFish.SetActive(true);
            m_GoFish.transform.position = MapInfo.Instance.MobPos[BestSpawnIndex].position;
            m_FishCount++;
        }
    }

    public void SpawnFish(int num, AIData data)
    {
        Vector3 spawnTarget = data.m_Go.transform.forward;
        NavMeshHit nHit;
        do
        {
            spawnTarget = Quaternion.AngleAxis(Random.Range(1f, 360f), Vector3.up) * spawnTarget;
            spawnTarget *= Random.Range(3f, 10f);
            spawnTarget += data.m_Go.transform.position;
        }while(NavMesh.Raycast(data.m_Go.transform.position, spawnTarget, out nHit, NavMesh.AllAreas));
       
        for (int i = 0; i < num; i++)
        {
            m_GoFish = ObjectPool.m_Instance.LoadGameObjectFromPool(3100);
            if (m_GoFish == null) return;
            m_GoFish.SetActive(true);
            m_GoFish.transform.position = spawnTarget;
            m_FishCount++;
        }
    }
}
