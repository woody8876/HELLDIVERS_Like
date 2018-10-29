using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData{

    public int index;
    public int m_iCurHP;
    public int m_iMaxHP;
    public int m_iArmor_FrontMin;
    public int m_iArmor_FrontMax;
    public int m_iArmor_BackMin;
    public int m_iArmor_BackMax;

    public bool m_bMissiling;
    public bool m_bRushing;
    public bool m_bEarthquake;

    public Vector3 m_vCurFace;
    public Vector3 m_vCurPos;
    
    public GameObject m_Go;
    public GameObject m_curActive;

    public Transform m_vTarget;
    public Transform m_vCenter;

    public List<GameObject> m_Obstacle;

    public BossFSMSystem m_bossFSMSystem;





}
