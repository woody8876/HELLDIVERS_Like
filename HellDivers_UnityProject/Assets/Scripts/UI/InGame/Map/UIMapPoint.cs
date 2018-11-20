using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMapPoint : MonoBehaviour {

    private Vector3 m_Pos = new Vector3();
    private Vector3 m_Center = new Vector3();
    private Vector3 m_Dir = new Vector3();
    private int m_PlayersCount = 0;
    [SerializeField] private Image m_Image;
    private Color m_Color;
    public GameObject CurrentTarget { get { return m_CurrentTarget; } }
    private GameObject m_CurrentTarget;

    public void Init(GameObject target, eMapPointType type)
    {
        m_CurrentTarget = target;
        m_Image = this.GetComponent<Image>();
        m_Color = m_Image.color;
        FindRadarCenter();
        CalculatePosition();
        switch (type)
        {
            case eMapPointType.FISH:
                FishAI fish = target.GetComponent<FishAI>();
                fish.OnSpawn += ShowPoint;
                fish.OnDeath += HidePoint;
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
        FishAI fish = m_CurrentTarget.GetComponent<FishAI>();
        fish.OnSpawn -= ShowPoint;
        fish.OnDeath -= HidePoint;
    }

    private void ShowPoint()
    {
        this.gameObject.SetActive(true);
        CalculatePosition();
    }

    private void HidePoint()
    {
        UIPanelMap.Instance.PointList.Remove(this.gameObject);
        ObjectPool.m_Instance.UnLoadObjectToPool(3002, this.gameObject);
    }

    private void FindRadarCenter()
    {
        m_Center = MapInfo.Instance.MapOrigin.transform.position;
        m_Center.x += UIPanelMap.Instance.MapWidth * 0.5f;
        m_Center.z += UIPanelMap.Instance.MapHeight * 0.5f;
    }

    private void CalculatePosition()
    {
        Vector3 targetPos = m_CurrentTarget.transform.position;
        m_Dir = targetPos - m_Center;
        Debug.Log(m_Dir);
        m_Pos = this.transform.localPosition;
        m_Pos.x = m_Dir.x * (UIPanelMap.Instance.RectWidth * 0.5f) / UIPanelMap.Instance.MapWidth;
        m_Pos.y = m_Dir.z * (UIPanelMap.Instance.RectHeight * 0.5f) / UIPanelMap.Instance.MapHeight;
        this.transform.localPosition = m_Pos;
    }
}
