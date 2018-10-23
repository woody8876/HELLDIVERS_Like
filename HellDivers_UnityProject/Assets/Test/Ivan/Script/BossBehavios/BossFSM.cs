using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM {

    public enum eFSMTransition
    {
        NULLTRANSITION = -1,
        GO_IDLE,
        G0_SEEK,
        G0_RUSH,
        GO_MISSILE,
        GO_THROWROCK,
        GO_EARTHQIAKE
    }
    public enum eFSMStateID
    {
        INITIAL = -1,
        IDLE,
        SEEK,
        RUSH,
        MISSILE,
        THROWROCK,
        EARTHQUACK,
    }
    #region Boss FSM Field
    public eFSMStateID m_StateID;
    public Dictionary<eFSMTransition, BossFSM> m_Map;
    public float m_fCurrentTime;
    public BossStateFuntion m_BSF;
    #endregion
    public BossFSM()
    {
        m_StateID = eFSMStateID.INITIAL;
        m_fCurrentTime = 0.0f;
        m_Map = new Dictionary<eFSMTransition, BossFSM>();
        m_BSF = new BossStateFuntion();
    }
    public void AddTransition(eFSMTransition trans, BossFSM toState)
    {
        if (m_Map.ContainsKey(trans)) return;
        m_Map.Add(trans, toState);
    }
    public void DeletTransition(eFSMTransition trans)
    {
        if (m_Map.ContainsKey(trans)) m_Map.Remove(trans);
    }
    public BossFSM TransitionTo(eFSMTransition trans)
    {
        if (!m_Map.ContainsKey(trans)) return null;
        return m_Map[trans];

    }
    public virtual void DoBeforeEnter()
    {

    }
    public virtual void DoBeforeLeave()
    {

    }
    public virtual void Do(AIData data)
    {

    }
    public virtual void CheckCondition(EnemyData data)
    {

    }
}

public class BossIdleState : BossFSM{
    public BossIdleState() { }
    public override void DoBeforeEnter()
    {
    }
    public override void DoBeforeLeave()
    {
    }
    public override void Do(AIData data)
    {
    }
    public override void CheckCondition(EnemyData enemyData)
    {
    }
}

public class BossSeekState :BossFSM
{
    float fSeekTime = 0.0f;
    Vector3 vec;
    public BossSeekState()
    {
        m_StateID = eFSMStateID.SEEK;
    }
    public override void DoBeforeEnter()
    {
        fSeekTime = Random.Range(3.0f, 6.0f);
        m_fCurrentTime = 0.0f;
    }
    public override void DoBeforeLeave()
    {
    }
    public override void Do(AIData data)
    {
        vec = m_BSF.Seek(data.m_Go.transform, data.m_vTarget);
    }
    public override void CheckCondition(EnemyData enemyData)
    {
        if (enemyData.m_iCurHP >= enemyData.m_iMaxHP * 0.7f)
        {

        }
        else if (enemyData.m_iCurHP >= enemyData.m_iMaxHP * 0.2f)
        {

        }
        else
        {

        }
    }
}

public class BossRushState : BossFSM
{
    public BossRushState() { }
    public override void DoBeforeEnter()
    {
    }
    public override void DoBeforeLeave()
    {
    }
    public override void Do(AIData data)
    {
    }
    public override void CheckCondition(EnemyData enemyData)
    {
    }
}

public class BossMissleState : BossFSM
{
    public BossMissleState() { }
    public override void DoBeforeEnter()
    {
    }
    public override void DoBeforeLeave()
    {
    }
    public override void Do(AIData data)
    {
    }
    public override void CheckCondition(EnemyData enemyData)
    {
    }
}

public class BossThrowRockState : BossFSM
{
    public BossThrowRockState() { }
    public override void DoBeforeEnter()
    {
    }
    public override void DoBeforeLeave()
    {
    }
    public override void Do(AIData data)
    {
    }
    public override void CheckCondition(EnemyData enemyData)
    {
    }
}

public class BossEarthquakeState : BossFSM
{
    public BossEarthquakeState() { }
    public override void DoBeforeEnter()
    {
    }
    public override void DoBeforeLeave()
    {
    }
    public override void Do(AIData data)
    {
    }
    public override void CheckCondition(EnemyData enemyData)
    {
    }
}




