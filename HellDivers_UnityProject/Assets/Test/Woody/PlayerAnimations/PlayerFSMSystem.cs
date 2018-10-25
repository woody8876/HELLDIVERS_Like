using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSMSystem
{
    private List<PlayerFSMState> m_states;
    private Dictionary<ePlayerFSMTrans, PlayerFSMState> m_GlobalMap;
    private ePlayerFSMStateID m_currentStateID;
    public ePlayerFSMStateID CurrentStateID { get { return m_currentStateID; } }
    private PlayerFSMState m_currentState;
    public PlayerFSMState CurrentState { get { return m_currentState; } }
    private PlayerController m_Data;
    // Use this for initialization
    public PlayerFSMSystem(PlayerController data)
    {
        m_Data = data;
        m_states = new List<PlayerFSMState>();
        m_GlobalMap = new Dictionary<ePlayerFSMTrans, PlayerFSMState>();
    }

    public void AddGlobalTransition(ePlayerFSMTrans t, PlayerFSMState s)
    {
        m_GlobalMap.Add(t, s);
    }

    public void PerformGlobalTransition(ePlayerFSMTrans t)
    {
        if (m_GlobalMap.ContainsKey(t))
        {
            m_currentState.DoBeforeLeave(m_Data);
            m_currentState = m_GlobalMap[t];
            m_currentState.DoBeforeEnter(m_Data);
            m_currentStateID = m_currentState.m_StateID;
        }
    }

    public void AddState(PlayerFSMState s)
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

        foreach (PlayerFSMState state in m_states)
        {
            if (state.m_StateID == s.m_StateID)
            {
                return;
            }
        }
        m_states.Add(s);

    }

    public void DeleteState(ePlayerFSMStateID id)
    {
        if (id == ePlayerFSMStateID.NullStateID)
        {
            return;
        }

        foreach (PlayerFSMState state in m_states)
        {
            if (state.m_StateID == id)
            {
                m_states.Remove(state);
                return;
            }
        }
    }

    public void PerformTransition(ePlayerFSMTrans trans)
    {
        if (trans == ePlayerFSMTrans.NullTransition)
        {
            return;
        }

        PlayerFSMState state = m_currentState.TransitionTo(trans);
        if (state == null)
        {
            return;
        }

        // Update the currentStateID and currentState


        m_currentState.DoBeforeLeave(m_Data);
        m_Data.m_NowAnimation = "Origin";

        m_currentState = state;
        m_currentStateID = state.m_StateID;
        m_currentState.DoBeforeEnter(m_Data);
    }

    public void DoState()
    {
        m_currentState.CheckCondition(m_Data);
        m_currentState.Do(m_Data);
    }
}
