using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelRadar : MonoBehaviour {

    public static UIPanelRadar Instance { get; private set; }
    public float RectWidth { get { return m_RectWidth; } }
    public float RectHeight { get { return m_RectHeight; } }
    public float RadarRadius { get { return m_RadarRadius; } }
    [SerializeField] private UIRadarPoint m_PointPrefab;
    private RectTransform m_RectTransform;
    private float m_RectWidth;
    private float m_RectHeight;
    private Image m_Image;
    private Color m_Color;
    private List<UIRadarPoint> pointList;
    [SerializeField] private float m_RadarRadius = 40.0f;


    private bool bAdd = true;

	// Use this for initialization
	void Start () {

        if (Instance == null) Instance = this;
        //else Destroy(this.gameObject);
        m_RectTransform = this.GetComponent<RectTransform>();
        m_RectWidth = m_RectTransform.sizeDelta.x;
        m_RectHeight = m_RectTransform.sizeDelta.y;
        m_Image = this.GetComponent<Image>();
        m_Color = m_Image.color;
    }
	
	// Update is called once per frame
	void Update () {
        if (bAdd)
        {
            m_Color.g += Time.deltaTime * 0.2f;
            m_Image.color = m_Color;
            if (m_Color.g > 0.2) bAdd = false;
        }
        else if(bAdd == false)
        {
            m_Color.g -= Time.deltaTime * 0.2f;
            m_Image.color = m_Color;
            if (m_Color.g <= 0) bAdd = true;
        }
    }

    public void AddPointPrefab(GameObject target)
    {
        UIRadarPoint p = GameObject.Instantiate(m_PointPrefab,this.transform);
        p.Init(target);
        pointList.Add(p);
    }
}
