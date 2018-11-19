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
    private int m_TotalFishVariantKill;
    private int m_TotalPatrolKill;
    public int TotalKill { get { return m_TotalKill; } private set { } }
    public int TotalFishKill { get { return m_TotalFishKill; } private set { } }
    public int TotalPatrolKill { get { return m_TotalPatrolKill; } private set { } }
    private GameObject m_GOPlayer;
    private GameObject m_GOFish;
    private GameObject m_GOFishVariant;
    private GameObject m_GOPatrol;
    private GameObject m_GOTank;
    private GameObject m_GOWarning;
    private GameObject m_GOBullet;
    private GameObject m_GOSpwanEffect;
    private GameObject m_GORadarPoint;
    private GameObject m_GORMapPoint;
    private Player m_Player;

    public void Init()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }

        m_GORadarPoint = Resources.Load("UI/InGame/Radar/TargetPoint") as GameObject;
        m_GORMapPoint = Resources.Load("UI/InGame/Map/MapPoint") as GameObject;
        m_GOFish = Resources.Load("Mobs/Fish/Fish") as GameObject;
        m_GOFishVariant = Resources.Load("Mobs/Fish2/Fish2") as GameObject;
        m_GOPatrol = Resources.Load("Mobs/Patrol/Patrol") as GameObject;
        m_GOTank = Resources.Load("Mobs/Tank/Tank") as GameObject;
        m_GOBullet = Resources.Load("Mobs/Patrol/PatrolBullet") as GameObject;
        m_GOWarning = Resources.Load("Mobs/Effect/EnemyAlert") as GameObject;
        m_GOSpwanEffect = Resources.Load("Mobs/Effect/SpawnEffect") as GameObject;

        ObjectPool.m_Instance.InitGameObjects(m_GORadarPoint, 40, 3002);
        ObjectPool.m_Instance.InitGameObjects(m_GORMapPoint, 40, 3003);
        ObjectPool.m_Instance.InitGameObjects(m_GOFish, 40, 3100);
        ObjectPool.m_Instance.InitGameObjects(m_GOFishVariant, 10, 3300);
        ObjectPool.m_Instance.InitGameObjects(m_GOPatrol, 50, 3200);
        ObjectPool.m_Instance.InitGameObjects(m_GOTank, 10, 3400);
        ObjectPool.m_Instance.InitGameObjects(m_GOBullet, 20, 3201);
        ObjectPool.m_Instance.InitGameObjects(m_GOWarning, 5, 3210);
        ObjectPool.m_Instance.InitGameObjects(m_GOSpwanEffect, 30, 3001);
        m_TotalKill = 0;
        m_TotalFishKill = 0;
        m_TotalPatrolKill = 0;
    }

    public void DecreaseMobCount(int ID)
    {
        m_TotalKill++;

        switch (ID)
        {
            case 3100:
                m_TotalFishKill++;
                m_FishCount--;
                break;
            case 3200:
                m_TotalPatrolKill++;
                m_PatrolCount--;
                break;
            case 3300:
                m_TotalFishVariantKill++;
                m_FishCount--;
                break;
        }
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
        if (m_FishCount > 20) return;

        List<Player> pList = InGamePlayerManager.Instance.Players;
        Vector3 Center = new Vector3();
        Center.Set(0, 0, 0);
        for (int i = 0; i < pList.Count; i++)
        {
            Center += pList[i].transform.position;
        }
        Center /= pList.Count;

        Vector3 spawnTarget = Center;
        NavMeshHit nHit;
        do
        {
            spawnTarget = Vector3.forward;
            spawnTarget = Quaternion.AngleAxis(Random.Range(1f, 360f), Vector3.up) * spawnTarget;
            spawnTarget *= Random.Range(25f, 35f);
            spawnTarget += Center;
        } while (NavMesh.Raycast(Center, spawnTarget, out nHit, NavMesh.AllAreas));

        for (int i = 0; i < num; i++)
        {
            m_GOFish = ObjectPool.m_Instance.LoadGameObjectFromPool(3100);
            if (m_GOFish == null) return;
            m_GOFish.transform.position = spawnTarget;
            m_GOFish.SetActive(true);
            m_FishCount++;

            if (UIPanelRadar.Instance != null)
                UIPanelRadar.Instance.AddPointPrefab(m_GOFish, eMapPointType.FISH);
            if (UIPanelMap.Instance != null)
                UIPanelMap.Instance.AddPointPrefab(m_GOFish, eMapPointType.FISH, 3002);
        }

        m_GOPatrol = ObjectPool.m_Instance.LoadGameObjectFromPool(3200);
        if (m_GOPatrol == null) return;
        m_GOPatrol.transform.position = spawnTarget;
        m_GOPatrol.SetActive(true);
        PatrolAI patrolAI = m_GOPatrol.GetComponent<PatrolAI>();
        patrolAI.m_bGoIdle = true;
        m_PatrolCount++;
        if (UIPanelRadar.Instance != null)
            UIPanelRadar.Instance.AddPointPrefab(m_GOFish, eMapPointType.FISH);
        if (UIPanelMap.Instance != null)
            UIPanelMap.Instance.AddPointPrefab(m_GOFish, eMapPointType.FISH, 3002);
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
            m_GOFish.transform.position = spawnTarget;
            m_GOFish.SetActive(true);
            m_FishCount++;

            if (UIPanelRadar.Instance != null)
                UIPanelRadar.Instance.AddPointPrefab(m_GOFish, eMapPointType.FISH);
            if (UIPanelMap.Instance != null)
                UIPanelMap.Instance.AddPointPrefab(m_GOFish, eMapPointType.FISH, 3002);
        }
    }

    public void SpawnFishVariant(int num)
    {
        if (m_FishCount > 5) return;

        List<Player> pList = InGamePlayerManager.Instance.Players;
        Vector3 Center = new Vector3();
        Center.Set(0, 0, 0);
        for (int i = 0; i < pList.Count; i++)
        {
            Center += pList[i].transform.position;
        }
        Center /= pList.Count;

        Vector3 spawnTarget = new Vector3();
        NavMeshHit nHit;
        do
        {
            spawnTarget = Vector3.forward;
            spawnTarget = Quaternion.AngleAxis(Random.Range(1f, 360f), Vector3.up) * spawnTarget;
            spawnTarget *= Random.Range(25f, 35f);
            spawnTarget += Center;
        } while (NavMesh.Raycast(Center, spawnTarget, out nHit, NavMesh.AllAreas));

        for (int i = 0; i < num; i++)
        {
            m_GOFish = ObjectPool.m_Instance.LoadGameObjectFromPool(3300);
            if (m_GOFish == null) return;
            m_GOFish.transform.position = spawnTarget;
            m_GOFish.SetActive(true);

            m_FishVariantCount++;
        }
    }

    public void UnloadMob(int ID, MobInfo data)
    {
        ObjectPool.m_Instance.UnLoadObjectToPool(ID, data.m_Go);
        if (UIPanelRadar.Instance == null) return;
        UIPanelRadar.Instance.DeletePointPrefab(data.m_Go);
    }
}

