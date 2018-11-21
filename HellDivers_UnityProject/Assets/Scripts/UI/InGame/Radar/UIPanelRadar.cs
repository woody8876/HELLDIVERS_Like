using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelRadar : MonoBehaviour {

    public delegate void UIRadarEventHolder();
    public event UIRadarEventHolder UpdatePoint;

    #region Variable
    public static UIPanelRadar Instance { get; private set; }
    private GameObject m_GORadarPoint;
    public float RectWidth { get { return m_RectWidth; } }
    public float RectHeight { get { return m_RectHeight; } }
    public float RadarRadius { get { return m_RadarRadius; } }
    public Color Color { get { return m_Color; } }
    public float Timer { get { return m_Timer; } }
    public List<GameObject> PointList { get { return m_PointList; } }
    [SerializeField] private UIRadarPoint m_PointPrefab;
    private RectTransform m_RectTransform;
    private float m_RectWidth;
    private float m_RectHeight;
    private Image m_Image;
    private Color m_Color;
    private float m_Timer;
    private bool bAdd = true;
    private List<GameObject> m_PointList = new List<GameObject>();

    [SerializeField] private float m_RadarRadius = 100.0f;
    #endregion

    public void Init()
    {
        if (Instance == null) Instance = this;
        m_GORadarPoint = Resources.Load("UI/InGame/Radar/PlayerPoint") as GameObject;
        ObjectPool.m_Instance.InitGameObjects(m_GORadarPoint, 3, 9101);
        m_GORadarPoint = Resources.Load("UI/InGame/Radar/MobPoint") as GameObject;
        ObjectPool.m_Instance.InitGameObjects(m_GORadarPoint, 40, 9102);
        m_RectTransform = this.GetComponent<RectTransform>();
        m_RectWidth = m_RectTransform.sizeDelta.x;
        m_RectHeight = m_RectTransform.sizeDelta.y;
        m_Image = this.GetComponent<Image>();
        m_Color = m_Image.color;
    }

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        Blink();
        CountTimer();
    }

    public void AddPointPrefab(GameObject target, eMapPointType type)
    {
        GameObject go;
        UIRadarPoint p;
        switch (type)
        {
            case eMapPointType.PLAYER:
                go = ObjectPool.m_Instance.LoadGameObjectFromPool(9101);
                go.SetActive(true);
                p = go.GetComponent<UIRadarPoint>();
                p.Init(target, type);
                p.transform.SetParent(this.transform);
                m_PointList.Add(go);
                break;
            case eMapPointType.FISH:
                go = ObjectPool.m_Instance.LoadGameObjectFromPool(9102);
                go.SetActive(true);
                p = go.GetComponent<UIRadarPoint>();
                p.Init(target, type);
                p.transform.SetParent(this.transform);
                m_PointList.Add(go);
                break;
        }
    }
    public void AddPointPrefab(Player player, eMapPointType type)
    {
        GameObject go;
        UIRadarPoint p;
        switch (type)
        {
            case eMapPointType.PLAYER:
                go = ObjectPool.m_Instance.LoadGameObjectFromPool(9101);
                go.SetActive(true);
                p = go.GetComponent<UIRadarPoint>();
                p.Init(player, type);
                p.transform.SetParent(this.transform);
                m_PointList.Add(go);
                break;
        }
    }
    private void Blink()
    {
        if (bAdd)
        {
            m_Color.b += Time.deltaTime * 0.1f;
            m_Color.g += Time.deltaTime * 0.1f;
            m_Image.color = m_Color;
            if (m_Color.b > 0.35) bAdd = false;
        }
        else if (bAdd == false)
        {
            m_Color.b -= Time.deltaTime * 0.1f;
            m_Color.g -= Time.deltaTime * 0.1f;
            m_Image.color = m_Color;
            if (m_Color.b <= 0.3) bAdd = true;
        }
    }

    private void CountTimer()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer >= 1.0f && m_Color.b > 0.35f)
        {
            if(m_PointList.Count > 0)
            {
                UpdatePoint();
            }
            m_Timer = 0.0f;
        }
    }
}
