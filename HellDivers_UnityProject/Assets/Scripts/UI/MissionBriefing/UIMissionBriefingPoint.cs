using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionBriefingPoint : MonoBehaviour {

    [SerializeField] private GameObject m_GO;
    [SerializeField] private UIMissionBriefingMap m_Map;
    private RectTransform m_MapRect;
    private Vector3 m_Pos;
    private float m_RectWidth;
    private float m_RectHeight;
    // Use this for initialization
    void Start () {
        m_GO = Instantiate(m_GO);
        m_GO.transform.position = new Vector3(366f, 54.6f, 337f);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIMissionBriefingMap.Instance.AddPointPrefab(m_GO, eMapPointType.MISSIONTOWER);
        }
        MovePoint();

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
