using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIData
{
    /// <summary>
    /// Effective area
    /// </summary>
    public float m_fRadius;
    /// <summary>
    /// The shortest distance which Object will be collision.
    /// </summary>
    public float m_fProbeLength;
    /// <summary>
    /// The speed after calculate.
    /// </summary>
    public float m_Speed;
    /// <summary>
    /// The max speed we set.
    /// </summary>
    public float m_fMaxSpeed;
    /// <summary>
    /// The rotation after calculate.
    /// </summary>
    public float m_fRot;
    /// <summary>
    /// The max rotation we set.
    /// </summary>
    public float m_fMaxRot;
    /// <summary>
    /// The gameobject who use this function.
    /// </summary>
    public GameObject m_Go;
    /// <summary>
    /// The max distance this gameobject can see.
    /// </summary>
    public float m_fSight;
    /// <summary>
    /// The max distance this gameobject can attack.
    /// </summary>
    public float m_fAttackRange;
    /// <summary>
    /// Cool time that attack need.
    /// </summary>
    [HideInInspector]
    public float m_fAttackTime;
    /// <summary>
    /// The HP of the gameobject.
    /// </summary>
    public float m_fHp;
    /// <summary>
    /// The damage of the gameobject's attack.
    /// </summary>
    public float m_fAttack;
    /// <summary>
    /// The temp turn force of the gameobject after calculate.
    /// </summary>
    [HideInInspector]
    public float m_fTempTurnForce;
    /// <summary>
    /// The move force of the gameobject after calculate.
    /// </summary>
    [HideInInspector]
    public float m_fMoveForce;
    /// <summary>
    /// Define if gameobject can move.
    /// </summary>
    [HideInInspector]
    public bool m_bMove;
    /// <summary>
    /// Define if gameobject will collision.
    /// </summary>
    [HideInInspector]
    public bool m_bCol;

    /// <summary>
    /// The target of the gameobject.
    /// </summary>
    [HideInInspector]
    public GameObject m_TargetObject;
    /// <summary>
    /// The target's transform.
    /// </summary>
    [HideInInspector]
    public Vector3 m_vTarget;

    //[HideInInspector]
    //public FSMSystem m_FSMSystem;

}
