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
    private GameObject m_GOPlayer;
    private GameObject m_GOFish;
    private GameObject m_GOPatrol;
    private GameObject m_GOWarning;
    private GameObject m_GOBullet;
    private GameObject m_GOSpwanEffect;

    public void Init()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }

        m_GOFish = Resources.Load("Mobs/Fish/Fish") as GameObject;
        m_GOPatrol = Resources.Load("Mobs/Patrol/Patrol") as GameObject;
        m_GOBullet = Resources.Load("Mobs/Patrol/PatrolBullet") as GameObject;
        m_GOWarning = Resources.Load("Mobs/Effect/EnemyAlert") as GameObject;
        m_GOSpwanEffect = Resources.Load("Mobs/Effect/SpawnEffect") as GameObject;
        ObjectPool.m_Instance.InitGameObjects(m_GOFish, 40, 3100);
        ObjectPool.m_Instance.InitGameObjects(m_GOPatrol, 40, 3200);
        ObjectPool.m_Instance.InitGameObjects(m_GOBullet, 20, 3201);
        ObjectPool.m_Instance.InitGameObjects(m_GOWarning, 5, 3210);
        ObjectPool.m_Instance.InitGameObjects(m_GOSpwanEffect, 10, 3001);
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
            m_GOPatrol = ObjectPool.m_Instance.LoadGameObjectFromPool(3200);
            if (m_GOPatrol == null) return;
            m_GOPatrol.SetActive(true);
            m_GOPatrol.transform.position = MapInfo.Instance.MobPos[spawnIndex].position;
            m_PatrolCount++;
        }
    }

    public void SpawnFish(int num)
    {
        //if (MapInfo.Instance == null) return;
        //if (MapInfo.Instance.MobPos.Count <= 0) return;


        //if (m_FishCount > 10) return;
        //int BestSpawnIndex = 0;
        //float BestDist = 0;

        //for (int index = 0; index < MapInfo.Instance.MobPos.Count; index++)
        //{
        //    if (index == 0)
        //    {
        //        BestDist = (MapInfo.Instance.MobPos[index].position - m_GoPlayer.transform.position).magnitude;
        //        BestSpawnIndex = index;
        //        continue;
        //    }
        //    if ((MapInfo.Instance.MobPos[index].position - m_GoPlayer.transform.position).magnitude < 20f)
        //    {
        //        continue;
        //    }
        //    if ((MapInfo.Instance.MobPos[index].position - m_GoPlayer.transform.position).magnitude < BestDist)
        //    {
        //        BestDist = (MapInfo.Instance.MobPos[index].position - m_GoPlayer.transform.position).magnitude;
        //        BestSpawnIndex = index;
        //    }
        //}
        //for (int i = 0; i < num; i++)
        //{
        //    m_GoFish = ObjectPool.m_Instance.LoadGameObjectFromPool(3100);
        //    if (m_GoFish == null) return;
        //    m_GoFish.SetActive(true);
        //    m_GoFish.transform.position = MapInfo.Instance.MobPos[BestSpawnIndex].position;
        //    m_FishCount++;
        //}

        if (m_FishCount > 5) return;

        m_GOPlayer = GameObject.FindGameObjectWithTag("Player");

        Vector3 spawnTarget = m_GOPlayer.transform.forward;
        NavMeshHit nHit;
        do
        {
            spawnTarget = m_GOPlayer.transform.forward;
            spawnTarget = Quaternion.AngleAxis(Random.Range(1f, 360f), Vector3.up) * spawnTarget;
            spawnTarget *= Random.Range(25f, 35f);
            spawnTarget += m_GOPlayer.transform.position;
        } while (NavMesh.Raycast(m_GOPlayer.transform.position, spawnTarget, out nHit, NavMesh.AllAreas));
      
        for (int i = 0; i < num; i++)
        {
            m_GOFish = ObjectPool.m_Instance.LoadGameObjectFromPool(3100);
            if (m_GOFish == null) return;
            m_GOFish.transform.position = spawnTarget;
            m_GOFish.SetActive(true);
            m_FishCount++;
        }
    }

    public void SpawnFish(int num, AIData data)
    {
        Vector3 spawnTarget = data.m_Go.transform.forward;
        NavMeshHit nHit;
        do
        {
            spawnTarget = data.m_Go.transform.forward;
            spawnTarget = Quaternion.AngleAxis(Random.Range(1f, 360f), Vector3.up) * spawnTarget;
            spawnTarget *= Random.Range(1f, 2f);
            spawnTarget += data.m_Go.transform.position;
        }while(NavMesh.Raycast(data.m_Go.transform.position, spawnTarget, out nHit, NavMesh.AllAreas));


        for (int i = 0; i < num; i++)
        {
            m_GOFish = ObjectPool.m_Instance.LoadGameObjectFromPool(3100);
            if (m_GOFish == null) return;
            m_GOFish.transform.position = spawnTarget;
            m_GOFish.SetActive(true);
            m_FishCount++;
        }
    }

    public void SpawnFish(int num, Transform center, float minRadius, float maxRadius)
    {
        Vector3 spawnTarget = center.forward;
        NavMeshHit nHit;

        for (int i = 0; i < num; i++)
        {
            do
            {
                spawnTarget = center.forward;
                spawnTarget = Quaternion.AngleAxis(Random.Range(1f, 360f), Vector3.up) * spawnTarget;
                spawnTarget *= Random.Range(minRadius, maxRadius);
                spawnTarget += center.position;
            } while (NavMesh.Raycast(center.position, spawnTarget, out nHit, NavMesh.AllAreas));

            m_GOFish = ObjectPool.m_Instance.LoadGameObjectFromPool(3100);
            if (m_GOFish == null) return;
            m_GOFish.SetActive(true);
            m_GOFish.transform.position = spawnTarget;
            m_FishCount++;
        }
    }
}
