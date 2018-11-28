using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionBriefingMap : MonoBehaviour {

    public static UIMissionBriefingMap Instance { get; private set; }
    
    private RectTransform m_RectTransform;
    public float RectWidth { get { return m_RectWidth; } }
    public float RectHeight { get { return m_RectHeight; } }
    public float MapWidth { get { return m_MapWidth; } }
    public float MapHeight { get { return m_MapHeight; } }

    private float m_RectWidth;
    private float m_RectHeight;
    private float m_MapWidth = 544.0f;
    private float m_MapHeight = 720.0f;

    [SerializeField] private GameObject m_GOMapPoint;
    // Use this for initialization
    void Start () {
        if (Instance == null) Instance = this;

        m_RectTransform = this.GetComponent<RectTransform>();
        m_RectWidth = m_RectTransform.sizeDelta.x;
        m_RectHeight = m_RectTransform.sizeDelta.y;
        
    }
	
	// Update is called once per frame
	void Update () {
        
        
    }
    public void AddPointPrefab(GameObject target, eMapPointType type)
    {
        GameObject go;
        UIMissionBriefingMapPoint p;
        go = Instantiate(m_GOMapPoint);
        go.SetActive(true);
        p = go.GetComponent<UIMissionBriefingMapPoint>();
        p.transform.SetParent(this.transform);
        p.Init(target, type);
    }
}
