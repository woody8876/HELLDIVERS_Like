using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionBriefingMapPoint : MonoBehaviour {

    #region Variable
    private Vector3 m_Pos = new Vector3();
    private Vector3 m_Center = new Vector3();
    private Vector3 m_Dir = new Vector3();
    private Image m_Image;
    private Color m_OriColor;
    private Color m_Color;
    #endregion

    public GameObject CurrentTarget { get { return m_CurrentTarget; } }
    private GameObject m_CurrentTarget;
    private eMapPointType m_CurrentType;

    public void Init(GameObject target, eMapPointType type)
    {
        m_Image = this.GetComponent<Image>();
        m_OriColor = m_Image.color;
        m_Color = m_Image.color;

        m_CurrentTarget = target;
        m_CurrentType = type;
        FindRadarCenter();
        CalculatePosition();
        UIMissionBriefingPoint.Instance.Select += Selected;
        UIMissionBriefingPoint.Instance.UnSelect += UnSelected;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void Selected()
    {
        m_Color.r = 192; 
        m_Color.g = 192;
        m_Color.b = 192;
        m_Image.color = m_Color;
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("HAHAHA");
        }
    }
    private void UnSelected()
    {
        m_Image.color = m_OriColor;
    }

    private void FindRadarCenter()
    {
        m_Center = new Vector3(49.9f, 54.6f, 255.4f);
    }

    private void CalculatePosition()
    {
        FindRadarCenter();
        Vector3 targetPos = m_CurrentTarget.transform.position;
        m_Dir = targetPos - m_Center;
        m_Pos = this.transform.localPosition;
        m_Pos.x = m_Dir.x * (UIMissionBriefingMap.Instance.RectWidth) / UIMissionBriefingMap.Instance.MapWidth - UIMissionBriefingMap.Instance.RectWidth * 0.5f;
        m_Pos.y = m_Dir.z * (UIMissionBriefingMap.Instance.RectHeight) / UIMissionBriefingMap.Instance.MapHeight - UIMissionBriefingMap.Instance.RectHeight * 0.5f;
        this.transform.localPosition = m_Pos;
    }
}
