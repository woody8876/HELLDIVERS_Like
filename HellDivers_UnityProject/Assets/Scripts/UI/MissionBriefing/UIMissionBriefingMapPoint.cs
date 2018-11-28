using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionBriefingMapPoint : MonoBehaviour {

    private Vector3 m_Pos = new Vector3();
    private Vector3 m_Center = new Vector3();
    private Vector3 m_Dir = new Vector3();
    public GameObject CurrentTarget { get { return m_CurrentTarget; } }
    private GameObject m_CurrentTarget;
    private eMapPointType m_CurrentType;
    [SerializeField] private UIMissionBriefingPoint m_Point;
    public void Init(GameObject target, eMapPointType type)
    {
        m_CurrentTarget = target;
        m_CurrentType = type;
        FindRadarCenter();
        CalculatePosition();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(this.transform.position, m_Point.transform.position) < 10f)
        {
            Debug.Log("Close");
        }
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
