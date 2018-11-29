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

        if (UIMissionBriefingMap.Instance.PointList.Count < 1) return;
        foreach(GameObject go in UIMissionBriefingMap.Instance.PointList)
        {
            if (Vector3.Distance(this.transform.position, go.transform.position) < 20f)
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
        float h = Input.GetAxis("DirectionHorizontal");
        float v = Input.GetAxis("DirectionVertical");
        this.transform.localPosition += new Vector3(h, v, 0) * 3f;

        if (this.transform.localPosition.x > 350)
        {
            m_Pos = this.transform.localPosition;
            m_Pos.x = 350;
            this.transform.localPosition = m_Pos;
        }
        else if (this.transform.localPosition.x < -350)
        {
            m_Pos = this.transform.localPosition;
            m_Pos.x = -350;
            this.transform.localPosition = m_Pos;
        }
        if (this.transform.localPosition.y > 250)
        {
            m_Pos = this.transform.localPosition;
            m_Pos.y = 250;
            this.transform.localPosition = m_Pos;
        }
        else if (this.transform.localPosition.y < -250)
        {
            m_Pos = this.transform.localPosition;
            m_Pos.y = -250;
            this.transform.localPosition = m_Pos;
        }
    }
}
