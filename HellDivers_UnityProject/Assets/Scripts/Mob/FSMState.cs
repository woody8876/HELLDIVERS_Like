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
    Go_GetHurt,
    GO_WanderIdle,
    GO_Wander,
    Go_CallArmy,
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
    GetHurtStateID,
    WanderIdleStateID,
    WanderStateID,
    CallArmyState,
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
    }
    public override void DoBeforeEnter(AIData data)
    {
        m_fCurrentTime = 0.0f;
        m_fIdleTim = Random.Range(1.0f, 2.0f);
        data.m_AnimationController.SetAnimator(m_StateID, true);
    }

    public override void DoBeforeLeave(AIData data)
    {
        data.m_AnimationController.SetAnimator(m_StateID, false);
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
                if (m_fCurrentTime > m_fIdleTim)
                {
                    data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Attack);
                }
            }
        }
        if ((data.m_PlayerGO.transform.position - data.m_Go.transform.position).magnitude > data.m_fAttackRange)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
        }
        if (data.m_bIsPlayerDead)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_WanderIdle);
        }
    }
}

public class FSMMoveToState : FSMState
{
    public FSMMoveToState()
    {
        m_StateID = eFSMStateID.MoveToStateID;
    }


    public override void DoBeforeEnter(AIData data)
    {

    }

    public override void DoBeforeLeave(AIData data)
    {

    }

    public override void Do(AIData data)
    {
        data.navMeshAgent.enabled = true;
        data.m_vTarget = data.m_PlayerGO.transform.position;
        SteeringBehaviours.NavMove(data);
        Vector3 v = (SteeringBehaviours.GroupBehavior(data, 20, true) + SteeringBehaviours.GroupBehavior(data, 20, false)) * 2f * Time.deltaTime;
        data.m_Go.transform.position += v;
    }

    public override void CheckCondition(AIData data)
    {
        bool bAttack = false;
        GameObject go = AIData.AIFunction.CheckEnemyInSight(data, ref bAttack);
        if (go != null)
        {
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
        data.m_AnimationController.SetAnimator(m_StateID, true);
    }

    public override void DoBeforeLeave(AIData data)
    {
        data.m_AnimationController.SetAnimator(m_StateID, false);
    }

    public override void Do(AIData data)
    {
        data.navMeshAgent.enabled = true;
        data.m_vTarget = data.m_PlayerGO.transform.position;
        SteeringBehaviours.NavMove(data);
        Vector3 v = (SteeringBehaviours.GroupBehavior(data, 20, true) + SteeringBehaviours.GroupBehavior(data, 20, false)) * 2f * Time.deltaTime;
        data.m_Go.transform.position += v;
    }

    public override void CheckCondition(AIData data)
    {
        bool bAttack = false;
        bool bCheck = AIData.AIFunction.CheckTargetEnemyInSight(data, data.m_PlayerGO, ref bAttack);

        if (bAttack)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Attack);
        }

        if (data.m_bIsPlayerDead)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_WanderIdle);
        }
    }
}

public class FSMAttackState : FSMState
{
    private int AttackCount = 0;
    private int Count = 0;
    Vector3 vDir;
    public FSMAttackState()
    {
        m_StateID = eFSMStateID.AttackStateID;
    }

    public override void DoBeforeEnter(AIData data)
    {
        AttackCount = 0;
        Count = 0;
        data.navMeshAgent.enabled = false;
    }

    public override void DoBeforeLeave(AIData data)
    {

    }


    public override void Do(AIData data)
    {
        data.m_vTarget = data.m_PlayerGO.transform.position;
        vDir = data.m_PlayerGO.transform.position - data.m_Go.transform.position;

        if (Vector3.Angle(data.m_Go.transform.forward, vDir) >= 5.0f && Count < 1)
        {
            float fRight = Vector3.Dot(vDir.normalized, data.m_Go.transform.right);
            if (fRight >= 0)
            {
                data.m_Go.transform.Rotate(new Vector3(0, 10, 0), Space.Self);
            }
            else if (fRight < 0)
            {
                data.m_Go.transform.Rotate(new Vector3(0, -10, 0), Space.Self);
            }
        }
        if (Vector3.Angle(vDir, data.m_Go.transform.forward) <= 10.0f && Count < 1)
        {
            data.m_AnimationController.SetAnimator(m_StateID);
            Count++;
        }

        Vector3 v = (SteeringBehaviours.GroupBehavior(data, 20, true) + SteeringBehaviours.GroupBehavior(data, 20, false)) * 2f * Time.deltaTime;
        data.m_Go.transform.position += v;

        AnimatorStateInfo info = data.m_AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Attack"))
        {
            if (info.normalizedTime > 0.2f && AttackCount < 1)
            {
                DoDamage(data);
                AttackCount++;
            }
        }
    }

    public override void CheckCondition(AIData data)
    {
        AnimatorStateInfo info = data.m_AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Attack"))
        {
            if (info.normalizedTime > 0.9f)
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Idle);
            }
            return;
        }
        if ((data.m_PlayerGO.transform.position - data.m_Go.transform.position).magnitude > data.m_fAttackRange)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
        }
        if (data.m_bIsPlayerDead)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_WanderIdle);
        }
    }

    private void DoDamage(AIData data)
    {
        IDamageable target = data.m_PlayerGO.transform.GetComponent<IDamageable>();
        target.TakeDamage(10.0f, data.m_Go.transform.position);
    }
}

public class FSMGetHurtState : FSMState
{
    public FSMGetHurtState()
    {
        m_StateID = eFSMStateID.GetHurtStateID;
    }


    public override void DoBeforeEnter(AIData data)
    {
        data.navMeshAgent.enabled = false;
        data.m_AnimationController.SetAnimator(m_StateID);
    }

    public override void DoBeforeLeave(AIData data)
    {

    }

    public override void Do(AIData data)
    {

    }

    public override void CheckCondition(AIData data)
    {
        AnimatorStateInfo info = data.m_AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("GetHurt"))
        {
            if (info.normalizedTime > 0.9f)
            {
                Vector3 v = data.m_PlayerGO.transform.position - data.m_Go.transform.position;
                float fDist = v.magnitude;

                if (fDist < data.m_fAttackRange)
                {
                    data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Attack);
                }
                else
                {
                    data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
                }
            }
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
        data.navMeshAgent.enabled = false;
        data.m_AnimationController.SetAnimator(m_StateID);
    }

    public override void DoBeforeLeave(AIData data)
    {
       
    }

    public override void Do(AIData data)
    {

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
        data.m_vTarget = new Vector3(0, 0, 0);
        m_fCurrentTime = 0.0f;
        m_fIdleTime = Random.Range(1.0f, 3.0f);
        data.m_AnimationController.SetAnimator(m_StateID, true);
    }

    public override void DoBeforeLeave(AIData data)
    {
        data.m_AnimationController.SetAnimator(m_StateID, false);
    }

    public override void Do(AIData data)
    {
        m_fCurrentTime += Time.deltaTime;
    }

    public override void CheckCondition(AIData data)
    {
        float Dist = (data.m_PlayerGO.transform.position - data.m_Go.transform.position).magnitude;

        if (m_fCurrentTime > m_fIdleTime && SteeringBehaviours.CreatRandomTarget(data) == true)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Wander);
        }
        if (Dist < data.m_fPatrolVisionLength)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Flee);
            return;
        }
        if (data.m_bIsPlayerDead == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
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
        data.navMeshAgent.enabled = true;
        data.m_AnimationController.SetAnimator(m_StateID, true);
    }
    public override void DoBeforeLeave(AIData data)
    {
        data.navMeshAgent.enabled = false;
        data.m_AnimationController.SetAnimator(m_StateID, false);
    }
    public override void Do(AIData data)
    {
        data.m_vTarget.y = data.m_Go.transform.position.y;
        SteeringBehaviours.NavMove(data);
    }
    public override void CheckCondition(AIData data)
    {
        float Dist = (data.m_PlayerGO.transform.position - data.m_Go.transform.position).magnitude;

        if (Vector3.Distance(data.m_vTarget, data.m_Go.transform.position) < 0.1f)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_WanderIdle);
        }
        if (data.m_bIsPlayerDead == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
        }
        if (Dist < data.m_fPatrolVisionLength)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Flee);
        }
    }
}

public class FSMCallArmyState : FSMState
{
    public FSMCallArmyState()
    {
        m_StateID = eFSMStateID.CallArmyState;
    }


    public override void DoBeforeEnter(AIData data)
    {
        MobManager.m_Instance.SpawnFish(5);
        data.m_AnimationController.SetAnimator(m_StateID);
    }

    public override void DoBeforeLeave(AIData data)
    {

    }

    public override void Do(AIData data)
    {

    }

    public override void CheckCondition(AIData data)
    {
        float Dist = (data.m_PlayerGO.transform.position - data.m_Go.transform.position).magnitude;

        AnimatorStateInfo info = data.m_AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("CallArmy"))
        {
            if (info.normalizedTime > 0.9f)
            {
                if (Dist < data.m_fPatrolVisionLength)
                {
                    data.m_FSMSystem.PerformTransition(eFSMTransition.GO_Flee);
                    return;
                }
                data.m_FSMSystem.PerformTransition(eFSMTransition.GO_WanderIdle);
            }
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
        data.navMeshAgent.enabled = true;
        data.m_AnimationController.SetAnimator(m_StateID, true);
    }

    public override void DoBeforeLeave(AIData data)
    {
        data.navMeshAgent.enabled = false;
        data.m_AnimationController.SetAnimator(m_StateID, false);
    }

    public override void Do(AIData data)
    {
        data.m_vTarget = data.m_Go.transform.position + (data.m_Go.transform.position - data.m_PlayerGO.transform.position).normalized;
        data.m_vTarget.y = data.m_Go.transform.position.y;
        SteeringBehaviours.NavMove(data);
    }

    public override void CheckCondition(AIData data)
    {
        float Dist = (data.m_PlayerGO.transform.position - data.m_Go.transform.position).magnitude;

        if (Dist > data.m_fPatrolVisionLength * 1.5f)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_CallArmy);
        }
    }
}
