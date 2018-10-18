using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIData
{
    public float m_fRadius;
    public float m_fProbeLength;
    public float m_Speed;
    public float m_fMaxSpeed;
    public float m_fMaxRot;
    public GameObject m_Go;
    public GameObject m_PlayerGO;

    [HideInInspector] public float m_fRot;
    [HideInInspector] public float m_fTempTurnForce;
    [HideInInspector] public float m_fMoveForce;
    [HideInInspector] public bool m_bMove;


    public GameObject m_TargetObject;
    [HideInInspector]
    public Vector3 m_vTarget;

    #region Attack attribute
    [HideInInspector]
    public float m_fAttackTime;
    public float m_fHp;
    public float m_fAttack;
    public float m_fSight;
    public float m_fAttackRange;
//    [HideInInspector]
//    public FSMSystem m_FSMSystem;
    #endregion

}
