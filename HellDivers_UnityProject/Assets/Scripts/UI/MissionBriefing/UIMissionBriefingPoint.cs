using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionBriefingPoint : MonoBehaviour {

    #region Event
    public delegate void UIMissionBriefingPointHolder();
    public event UIMissionBriefingPointHolder Select;
    public event UIMissionBriefingPointHolder UnSelect;
    #endregion

    public static UIMissionBriefingPoint Instance { get; private set; }

    [SerializeField] private UIMissionBriefingMap m_Map;
    private RectTransform m_MapRect;
    private Vector3 m_Pos;
    private float m_RectWidth;
    private float m_RectHeight;
    // Use this for initialization
    void Start () {
        if (Instance == null) Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
        MovePoint();
        if (UIMissionBriefingMap.Instance.PointList == null) return;

        if (UIMissionBriefingMap.Instance.PointList.Count < 1) return;

        foreach(GameObject go in UIMissionBriefingMap.Instance.PointList)
        {
            if (Vector3.Distance(this.transform.position, go.transform.position) < 5f)
            {
                if (Select != null) Select();
            }
            else
            {
                if (UnSelect != null) UnSelect();
            } 
        }
    }

    private void MovePoint()
    {
        m_RectWidth = UIMissionBriefingMap.Instance.RectWidth;
        m_RectHeight = UIMissionBriefingMap.Instance.RectHeight;
        float h = Input.GetAxis("StratagemHorizontal");
        float v = Input.GetAxis("StratagemVertical");
        this.transform.localPosition += new Vector3(h, v, 0) * 2f;
        if (this.transform.localPosition.x > m_RectWidth * 0.5f)
        {
            m_Pos = this.transform.localPosition;
            m_Pos.x = m_RectWidth * 0.5f;
            this.transform.localPosition = m_Pos;
        }
        else if (this.transform.localPosition.x < m_RectWidth * 0.5f * -1.0f)
        {
            m_Pos = this.transform.localPosition;
            m_Pos.x = m_RectWidth * 0.5f * -1.0f;
            this.transform.localPosition = m_Pos;
        }
        if (this.transform.localPosition.y > m_RectHeight * 0.5f)
        {
            m_Pos = this.transform.localPosition;
            m_Pos.y = m_RectHeight * 0.5f;
            this.transform.localPosition = m_Pos;
        }
        else if (this.transform.localPosition.y < m_RectHeight * 0.5f * -1.0f)
        {
            m_Pos = this.transform.localPosition;
            m_Pos.y = m_RectHeight * 0.5f * -1.0f;
            this.transform.localPosition = m_Pos;
        }
    }
}
