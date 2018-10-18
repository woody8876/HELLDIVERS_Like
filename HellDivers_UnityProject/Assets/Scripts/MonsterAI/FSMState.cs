using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFSMTransition
{
    NullTransition = 0,
    Go_Idle,
    Go_MoveTo,
    Go_Chase,
    Go_Attack,
    GO_Wander,
    GO_WanderIdle,
    GO_Flee,
    Go_Dead,
}


public enum eFSMStateID
{
    NullStateID = 0,
    IdleStateID,
    MoveToStateID,
    ChaseStateID,
    AttackStateID,
    WanderStateID,
    WanderIdleStateID,
    FleeStateID,
    DeadStateID,
}

public class FSMState
{
    public eFSMStateID m_StateID;
    public Dictionary<eFSMTransition, FSMState> m_Map;
    public float m_fCurrentTime;

    public FSMState()
    {
        m_StateID = eFSMStateID.NullStateID;
        m_fCurrentTime = 0.0f;
        m_Map = new Dictionary<eFSMTransition, FSMState>();
    }

    public void AddTransition(eFSMTransition trans, FSMState toState)
    {
        if (m_Map.ContainsKey(trans))
        {
            return;
        }

        m_Map.Add(trans, toState);
    }
    public void DelTransition(eFSMTransition trans)
    {
        if (m_Map.ContainsKey(trans))
        {
            m_Map.Remove(trans);
        }

    }

    public FSMState TransitionTo(eFSMTransition trans)
    {
        if (m_Map.ContainsKey(trans) == false)
        {
            return null;
        }
        return m_Map[trans];
    }

    public virtual void DoBeforeEnter(AIData data)
    {

    }

    public virtual void DoBeforeLeave(AIData data)
    {

    }

    public virtual void Do(AIData data)
    {

    }

    public virtual void CheckCondition(AIData data)
    {

    }
}


public class FSMIdleState : FSMState
{

    private float m_fIdleTim;


    public FSMIdleState()
    {
        m_StateID = eFSMStateID.IdleStateID;
        m_fIdleTim = Random.Range(1.0f, 3.0f);

    }


    public override void DoBeforeEnter(AIData data)
    {
        m_fCurrentTime = 0.0f;
        m_fIdleTim = Random.Range(1.0f, 3.0f);
    }

    public override void DoBeforeLeave(AIData data)
    {

    }

    public override void Do(AIData data)
    {
        m_fCurrentTime += Time.deltaTime;
    }

    public override void CheckCondition(AIData data)
    {
        bool bAttack = false;
        GameObject go = AIData.AIFunction.CheckEnemyInSight(data, ref bAttack);
        if (go != null)
        {
            data.m_TargetObject = go;
            if (bAttack)
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Attack);
            }
            else
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
            }
            return;
        }
        if (m_fCurrentTime > m_fIdleTim)
        {


            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_MoveTo);
        }
    }
}

public class FSMMoveToState : FSMState
{
    //private int m_iCurrentWanderPt;
    //private GameObject[] m_WanderPoints;
    public FSMMoveToState()
    {
        m_StateID = eFSMStateID.MoveToStateID;
        //m_iCurrentWanderPt = -1;
        //m_WanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
    }


    public override void DoBeforeEnter(AIData data)
    {
        //int iNewPt = Random.Range(0, m_WanderPoints.Length);
        //if (m_iCurrentWanderPt == iNewPt)
        //{
        //    return;
        //}
        //m_iCurrentWanderPt = iNewPt;
        //data.m_vTarget = m_WanderPoints[m_iCurrentWanderPt].transform.position;
        //data.m_bMove = true;
    }

    public override void DoBeforeLeave(AIData data)
    {

    }

    public override void Do(AIData data)
    {
        data.m_vTarget = data.m_PlayerGO.transform.position;
        Vector3 v = (SteeringBehaviours.GroupBehavior(data, 20, true) + SteeringBehaviours.GroupBehavior(data, 20, false)) * 2f * Time.deltaTime;
        //SteeringBehaviours.Seek(data);
        SteeringBehaviours.NavMove(data);
        data.m_Go.transform.position += v;
    }

    public override void CheckCondition(AIData data)
    {
        bool bAttack = false;
        GameObject go = AIData.AIFunction.CheckEnemyInSight(data, ref bAttack);
        if (go != null)
        {
            data.m_TargetObject = go;
            if (bAttack)
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Attack);
            }
            else
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
            }
            return;
        }

        if (data.m_bMove == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Idle);
        }
    }
}

public class FSMChaseState : FSMState
{
    public FSMChaseState()
    {
        m_StateID = eFSMStateID.ChaseStateID;
    }


    public override void DoBeforeEnter(AIData data)
    {

    }

    public override void DoBeforeLeave(AIData data)
    {

    }

    public override void Do(AIData data)
    {
        data.m_vTarget = data.m_TargetObject.transform.position;
        SteeringBehaviours.Seek(data);
        SteeringBehaviours.NavMove(data);
    }

    public override void CheckCondition(AIData data)
    {
        bool bAttack = false;
        bool bCheck = AIData.AIFunction.CheckTargetEnemyInSight(data, data.m_TargetObject, ref bAttack);
        if (bCheck == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Idle);
            return;
        }
        if (bAttack)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Attack);
        }
    }
}

public class FSMAttackState : FSMState
{
    private float fAttackTime = 0.0f;
    public FSMAttackState()
    {
        m_StateID = eFSMStateID.AttackStateID;
    }


    public override void DoBeforeEnter(AIData data)
    {
        fAttackTime = Random.Range(1.0f, 3.0f);
        m_fCurrentTime = 0.0f;
    }

    public override void DoBeforeLeave(AIData data)
    {

    }


    public override void Do(AIData data)
    {
        // Check Animation complete.
        //...

        if (m_fCurrentTime > fAttackTime)
        {
            m_fCurrentTime = 0.0f;
            data.m_Go.transform.forward = (data.m_PlayerGO.transform.position - data.m_Go.transform.position).normalized;
            // Do attack.
        }
        m_fCurrentTime += Time.deltaTime;
    }

    public override void CheckCondition(AIData data)
    {
        bool bAttack = false;
        bool bCheck = AIData.AIFunction.CheckTargetEnemyInSight(data, data.m_TargetObject, ref bAttack);
        if (bCheck == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Idle);
            return;
        }
        if (bAttack == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
            return;
        }
    }
}

public class FSMDeadState : FSMState
{
    public FSMDeadState()
    {
        m_StateID = eFSMStateID.DeadStateID;
    }


    public override void DoBeforeEnter(AIData data)
    {

    }

    public override void DoBeforeLeave(AIData data)
    {

    }

    public override void Do(AIData data)
    {
        Debug.Log("Do Dead State");
    }

    public override void CheckCondition(AIData data)
    {

    }
}

public class FSMWanderIdleState : FSMState
{

    private float m_fIdleTime;


    public FSMWanderIdleState()
    {
        m_StateID = eFSMStateID.WanderIdleStateID;
    }


    public override void DoBeforeEnter(AIData data)
    {
        m_fCurrentTime = 0.0f;
        m_fIdleTime = Random.Range(1.0f, 3.0f);
    }

    public override void DoBeforeLeave(AIData data)
    {

    }

    public override void Do(AIData data)
    {
        m_fCurrentTime += Time.deltaTime;
    }

    public override void CheckCondition(AIData data)
    {
        float Dist = (data.m_PlayerGO.transform.position - data.m_Go.transform.position).magnitude;

        if (Dist < data.m_fPatrolVisionLength)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Flee);
        }
        if (m_fCurrentTime > m_fIdleTime && SteeringBehaviours.CreatRandomTarget(data) == true)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Wander);
        }
    }
}

public class FSMWanderState : FSMState
{
    public FSMWanderState()
    {
        m_StateID = eFSMStateID.WanderStateID;
    }
    public override void DoBeforeEnter(AIData data)
    {

    }
    public override void DoBeforeLeave(AIData data)
    {

    }
    public override void Do(AIData data)
    {
        SteeringBehaviours.Seek(data);
        SteeringBehaviours.Move(data);
    }
    public override void CheckCondition(AIData data)
    {
        float Dist = (data.m_PlayerGO.transform.position - data.m_Go.transform.position).magnitude;

        if (Dist < data.m_fPatrolVisionLength)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Flee);
        }
        else if (Vector3.Distance(data.m_vTarget, data.m_Go.transform.position) < 0.1f)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_WanderIdle);
        }
    }
}

public class FSMFleeState : FSMState
{
    public FSMFleeState()
    {
        m_StateID = eFSMStateID.FleeStateID;
    }


    public override void DoBeforeEnter(AIData data)
    {

    }

    public override void DoBeforeLeave(AIData data)
    {

    }

    public override void Do(AIData data)
    {
        SteeringBehaviours.Flee(data);
        SteeringBehaviours.Move(data);
    }

    public override void CheckCondition(AIData data)
    {
        float Dist = (data.m_PlayerGO.transform.position - data.m_Go.transform.position).magnitude;

        if (Dist > data.m_fPatrolVisionLength * 1.5f)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_WanderIdle);
        }
    }
}
