using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MobManager
{
    static public MobManager m_Instance;

    private int m_FishCount;
    private int m_FishVariantCount;
    private int m_PatrolCount;
    private int m_TotalKill;
    private int m_TotalFishKill;
    private int m_TotalPatrolKill;
    public int TotalKill { get { return m_TotalKill; } private set { } }
    public int TotalFishKill { get { return m_TotalFishKill; } private set { } }
    public int TotalPatrolKill { get { return m_TotalPatrolKill; } private set { } }
    private GameObject m_GOPlayer;
    private GameObject m_GOFish;
    private GameObject m_GOFishVariant;
    private GameObject m_GOPatrol;
    private GameObject m_GOWarning;
    private GameObject m_GOBullet;
    private GameObject m_GOSpwanEffect;
    private GameObject m_GOMobPoint;

    public void Init()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }

        m_GOFish = Resources.Load("Mobs/Fish/Fish") as GameObject;
        m_GOFishVariant = Resources.Load("Mobs/Fish2/Fish2") as GameObject;
        m_GOPatrol = Resources.Load("Mobs/Patrol/Patrol") as GameObject;
        m_GOBullet = Resources.Load("Mobs/Patrol/PatrolBullet") as GameObject;
        m_GOWarning = Resources.Load("Mobs/Effect/EnemyAlert") as GameObject;
        m_GOSpwanEffect = Resources.Load("Mobs/Effect/SpawnEffect") as GameObject;
        m_GOMobPoint = Resources.Load("Radar/RadarPoint") as GameObject;

        ObjectPool.m_Instance.InitGameObjects(m_GOFish, 40, 3100);
        ObjectPool.m_Instance.InitGameObjects(m_GOFishVariant, 10, 3300);
        ObjectPool.m_Instance.InitGameObjects(m_GOPatrol, 50, 3200);
        ObjectPool.m_Instance.InitGameObjects(m_GOBullet, 20, 3201);
        ObjectPool.m_Instance.InitGameObjects(m_GOWarning, 5, 3210);
        ObjectPool.m_Instance.InitGameObjects(m_GOSpwanEffect, 30, 3001);
        ObjectPool.m_Instance.InitGameObjects(m_GOMobPoint, 40, 3002);
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

    public void SpawnPatrol(int num, Transform center, float minRadius, float maxRadius)
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

            m_GOPatrol = ObjectPool.m_Instance.LoadGameObjectFromPool(3100);
            if (m_GOPatrol == null) return;
            m_GOPatrol.transform.position = spawnTarget;
            m_GOPatrol.SetActive(true);
            m_PatrolCount++;
        }
    }

    public void SpawnFish(int num)
    {
        if (m_FishCount > 30) return;

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
            m_GOMobPoint = ObjectPool.m_Instance.LoadGameObjectFromPool(3002);
            if (m_GOFish == null) return;
            m_GOFish.transform.position = spawnTarget;
            m_GOMobPoint.SetActive(true);
            m_GOFish.SetActive(true);
            FishAI fishAI = m_GOFish.GetComponent<FishAI>();
            fishAI.m_RadarPoint = m_GOMobPoint;
            

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
            m_GOMobPoint = ObjectPool.m_Instance.LoadGameObjectFromPool(3002);
            if (m_GOFish == null) return;
            m_GOFish.transform.position = spawnTarget;
            m_GOMobPoint.SetActive(true);
            m_GOFish.SetActive(true);
            FishAI fishAI = m_GOFish.GetComponent<FishAI>();
            fishAI.m_RadarPoint = m_GOMobPoint;

            m_FishCount++;
        }
    }

    public void SpawnFishVariant(int num)
    {
        if (m_FishVariantCount > 5) return;

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
            m_GOFishVariant = ObjectPool.m_Instance.LoadGameObjectFromPool(3300);
            if (m_GOFishVariant == null) return;
            m_GOFishVariant.transform.position = spawnTarget;
            m_GOFishVariant.SetActive(true);
            m_FishVariantCount++;
        }
    }

    public void MobDead(int ID, AIData data)
    {
        ObjectPool.m_Instance.UnLoadObjectToPool(ID, data.m_Go);
    }
}

