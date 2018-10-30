using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AIData
{
    public float m_fRadius = 1;
    public float m_fProbeLength = 10;
    public float m_fPatrolVisionLength;
    public float m_Speed;
    public float m_fMaxSpeed;
    public float m_fMaxRot;
    public GameObject m_Go;
    public GameObject m_PlayerGO;

    [HideInInspector] public float m_fRot;
    [HideInInspector] public float m_fTempTurnForce;
    [HideInInspector] public float m_fMoveForce;
    [HideInInspector] public bool m_bCol;
    [HideInInspector] public bool m_bMove;
    public NavMeshAgent navMeshAgent;
    
    public GameObject m_TargetObject;
    [HideInInspector]
    public Vector3 m_vTarget;

    [HideInInspector]
    public FSMSystem m_FSMSystem;

    public MobAnimationsController m_AnimationController;
    

    #region Attack attribute
    [HideInInspector]
    public float m_fAttackTime;
    public float m_fHp;
    public float m_fAttack;
    public float m_fSight = 6;
    public float m_fAttackRange = 3;
    #endregion

    public class AIFunction
    {
        public static GameObject CheckEnemyInSight(AIData data, ref bool bAttack)
        {
            GameObject go = data.m_PlayerGO;
            Vector3 v = go.transform.position - data.m_Go.transform.position;
            float fDist = v.magnitude;
            if (fDist < data.m_fAttackRange)
            {
                bAttack = true;
                return go;
            }
            else if (fDist < data.m_fSight)
            {
                bAttack = false;
                return go;
            }
            return null;
        }

        public static bool CheckTargetEnemyInSight(AIData data, GameObject target, ref bool bAttack)
        {
            GameObject go = target;
            Vector3 v = go.transform.position - data.m_Go.transform.position;
            float fDist = v.magnitude;
            if (fDist < data.m_fAttackRange)
            {
                bAttack = true;
                return true;
            }
            else if (fDist < data.m_fSight)
            {
                bAttack = false;
                return true;
            }
            return false;
        }
    }
}
