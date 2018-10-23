using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSMSystem : MonoBehaviour {



    private List<BossFSM> m_states;
    private Dictionary<BossFSM.eFSMTransition, BossFSM> m_GlobalMap;
    private BossFSM.eFSMStateID m_currentStateID;
    public BossFSM.eFSMStateID CurrentStateID { get { return m_currentStateID; } }
    private BossFSM m_currentState;
    public BossFSM CurrentState { get { return m_currentState; } }
    private AIData m_Data;
    private EnemyData m_enemyData;
    public BossFSMSystem(AIData data)
    {
        m_Data = data;
        m_states = new List<BossFSM>();
        m_GlobalMap = new Dictionary<BossFSM.eFSMTransition, BossFSM>();
    }

    public void AddGlobalTransition(BossFSM.eFSMTransition t, BossFSM s)
    {
        m_GlobalMap.Add(t, s);
    }

    public void PerformGlobalTransition(BossFSM.eFSMTransition t)
    {
        if (m_GlobalMap.ContainsKey(t))
        {
            m_currentState.DoBeforeLeave();
            m_currentState = m_GlobalMap[t];
            m_currentState.DoBeforeEnter();
            m_currentStateID = m_currentState.m_StateID;
        }
    }

    public void AddState(BossFSM s)
    {
        if (s == null)
        {
            return;
        }

        if (m_states.Count == 0)
        {
            m_states.Add(s);
            m_currentState = s;
            m_currentStateID = s.m_StateID;
            return;
        }

        foreach (BossFSM state in m_states)
        {
            if (state.m_StateID == s.m_StateID)
            {
                return;
            }
        }
        m_states.Add(s);
    }

    public void DeleteState(BossFSM.eFSMStateID id)
    {
        if (id == BossFSM.eFSMStateID.INITIAL)
        {
            return;
        }

        foreach (BossFSM state in m_states)
        {
            if (state.m_StateID == id)
            {
                m_states.Remove(state);
                return;
            }
        }
    }

    public void PerformTransition(BossFSM.eFSMTransition trans)
    {
        if (trans == BossFSM.eFSMTransition.NULLTRANSITION)
        {
            return;
        }

        BossFSM state = m_currentState.TransitionTo(trans);
        if (state == null)
        {
            return;
        }

        // Update the currentStateID and currentState		
        m_currentState.DoBeforeLeave();
        m_currentState = state;
        m_currentStateID = state.m_StateID;
        m_currentState.DoBeforeEnter();

    }

    public void DoState()
    {
        m_currentState.CheckCondition(m_enemyData);
        m_currentState.Do(m_Data);
    }
}
