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
    private List<GameObject> m_PointList = new List<GameObject>();

    [SerializeField] private float m_RadarRadius = 40.0f;
    #endregion


    private bool bAdd = true;

	// Use this for initialization
	void Start () {
        if (Instance == null) Instance = this;

        m_GORadarPoint = Resources.Load("UI/InGame/Radar/TargetPoint") as GameObject;
        ObjectPool.m_Instance.InitGameObjects(m_GORadarPoint, 40, 9101);
        m_RectTransform = this.GetComponent<RectTransform>();
        m_RectWidth = m_RectTransform.sizeDelta.x;
        m_RectHeight = m_RectTransform.sizeDelta.y;
        m_Image = this.GetComponent<Image>();
        m_Color = m_Image.color;
    }
	
	// Update is called once per frame
	void Update () {
        Blink();
        CountTimer();
    }

    public void AddPointPrefab(GameObject target, eMapPointType type)
    {
        switch (type)
        {
            case eMapPointType.FISH:
                GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(9101);
                go.SetActive(true);
                UIRadarPoint p = go.GetComponent<UIRadarPoint>();
                p.Init(target, type);
                p.transform.parent = this.transform;
                m_PointList.Add(go);
                break;
        }
    }
    private void Blink()
    {
        if (bAdd)
        {
            m_Color.g += Time.deltaTime * 0.2f;
            m_Image.color = m_Color;
            if (m_Color.g > 0.2) bAdd = false;
        }
        else if (bAdd == false)
        {
            m_Color.g -= Time.deltaTime * 0.2f;
            m_Image.color = m_Color;
            if (m_Color.g <= 0) bAdd = true;
        }
    }

    private void CountTimer()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer >= 2.0f && m_Color.g > 0.2f)
        {
            if(m_PointList.Count > 0)
            {
                UpdatePoint();
            }
            m_Timer = 0.0f;
        }
    }
}
