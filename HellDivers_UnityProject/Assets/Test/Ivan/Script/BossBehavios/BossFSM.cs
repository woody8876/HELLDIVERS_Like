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

    public eFSMStateID m_StateID;
    public Dictionary<eFSMTransition, BossFSM> m_Map;
    public float m_fCurrentTime;

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
public class BossIdleState { }
public class BossSeekState { }
public class BossRushState { }
public class BossMissleState { }
public class BossThrowRockState { }
public class BossEarthquakeState { }


