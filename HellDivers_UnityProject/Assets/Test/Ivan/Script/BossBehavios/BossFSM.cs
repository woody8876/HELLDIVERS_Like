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
    #endregion
    public BossFSM()
    {
        m_StateID = eFSMStateID.INITIAL;
        m_fCurrentTime = 0.0f;
        m_Map = new Dictionary<eFSMTransition, BossFSM>();
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
    public virtual void Do()
    {

    }
    public virtual void CheckCondition()
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
    public override void Do()
    {
    }
    public override void CheckCondition()
    {
    }
}

public class BossSeekState :BossFSM
{
    float fSeekTime = 0.0f;
    public BossSeekState()
    {

    }
    public override void DoBeforeEnter()
    {
        fSeekTime = Random.Range(3.0f, 6.0f);
        m_fCurrentTime = 0.0f;
    }
    public override void DoBeforeLeave()
    {
    }
    public override void Do()
    {
    }
    public override void CheckCondition()
    {
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
    public override void Do()
    {
    }
    public override void CheckCondition()
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
    public override void Do()
    {
    }
    public override void CheckCondition()
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
    public override void Do()
    {
    }
    public override void CheckCondition()
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
    public override void Do()
    {
    }
    public override void CheckCondition()
    {
    }
}




