using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMapPoint : MonoBehaviour {

    private Vector3 m_Pos = new Vector3();
    private Vector3 m_Center = new Vector3();
    private Vector3 m_Dir = new Vector3();
    [SerializeField] private Image m_Image;
    private Color m_Color;
    public GameObject CurrentTarget { get { return m_CurrentTarget; } }
    private GameObject m_CurrentTarget;
    private Player m_CurrentPlayer;
    private eMapPointType m_CurrentType;

    public void Init(GameObject target, eMapPointType type)
    {
        m_CurrentTarget = target;
        m_CurrentType = type;
        FindRadarCenter();
        CalculatePosition();
        switch (type)
        {
            case eMapPointType.FISH:
                FishAI fish = target.GetComponent<FishAI>();
                fish.OnSpawn += ShowPoint;
                fish.OnDeath += HidePoint;
                break;
            case eMapPointType.MISSIONTOWER:
                MissionTower missionTower = target.GetComponent<MissionTower>();
                missionTower.OnActive += CompletePoint;
                ShowPoint();
                break;
        }
    }
    public void Init(Player player, eMapPointType type)
    {
        m_CurrentPlayer = player;
        m_CurrentTarget = player.gameObject;
        m_CurrentType = type;
        FindRadarCenter();
        CalculatePosition();
        switch (m_CurrentType)
        {
            case eMapPointType.PLAYER:
                m_CurrentPlayer.OnSpawnBegin += ShowPoint;
                m_CurrentPlayer.OnDeathBegin += HidePoint;
                break;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FindRadarCenter();
        CalculatePosition();
    }

    private void OnDestroy()
    {
        if (m_CurrentTarget == null) return;
        switch (m_CurrentType)
        {
            case eMapPointType.FISH:
                FishAI fish = m_CurrentTarget.GetComponent<FishAI>();
                fish.OnSpawn -= ShowPoint;
                fish.OnDeath -= HidePoint;
                break;
            case eMapPointType.MISSIONTOWER:
                MissionTower missionTower = m_CurrentTarget.GetComponent<MissionTower>();
                missionTower.OnActive -= CompletePoint;
                break;
            case eMapPointType.PLAYER:
                m_CurrentPlayer.OnSpawnBegin -= ShowPoint;
                m_CurrentPlayer.OnDeathBegin -= HidePoint;
                break;
        }
    }

    private void ShowPoint()
    {
        this.gameObject.SetActive(true);
        CalculatePosition();
    }

    private void CompletePoint()
    {
        m_Image = this.GetComponent<Image>();
        m_Color = m_Image.color;
        m_Color.r = 192.0f / 255.0f;
        m_Color.g = 192.0f / 255.0f;
        m_Color.b = 192.0f / 255.0f;
        m_Image.color = m_Color;
    }

    private void HidePoint()
    {
        switch (m_CurrentType)
        {
            case eMapPointType.FISH:
                ObjectPool.m_Instance.UnLoadObjectToPool(9202, this.gameObject);
                break;
            case eMapPointType.PLAYER:
                ObjectPool.m_Instance.UnLoadObjectToPool(9201, this.gameObject);
                break;
        }
    }

    private void FindRadarCenter()
    {
        m_Center = MapInfo.Instance.MapOrigin.transform.position;
    }

    private void CalculatePosition()
    {
        Vector3 targetPos = m_CurrentTarget.transform.position;
        m_Dir = targetPos - m_Center;
        m_Pos = this.transform.localPosition;
        m_Pos.x = m_Dir.x * (UIPanelMap.Instance.RectWidth) / UIPanelMap.Instance.MapWidth;
        m_Pos.y = m_Dir.z * (UIPanelMap.Instance.RectHeight) / UIPanelMap.Instance.MapHeight;
        this.transform.localPosition = m_Pos;
    }
}
