using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlayerFSMTrans
{
    NullTransition,
    Go_Gun,
    Go_Stratagem,
}
public enum ePlayerFSMStateID
{
    NullStateID,
    GunStateID,
    StratagemStateID,
}

public class PlayerFSMState{

    public ePlayerFSMStateID m_StateID;
    public Dictionary<ePlayerFSMTrans, PlayerFSMState> m_Map;
    public float m_fCurrentTime;

    public PlayerFSMState()
    {
        m_StateID = ePlayerFSMStateID.NullStateID;
        m_fCurrentTime = 0.0f;
        m_Map = new Dictionary<ePlayerFSMTrans, PlayerFSMState>();
    }

    public void AddTransition(ePlayerFSMTrans trans, PlayerFSMState toState)
    {
        if (m_Map.ContainsKey(trans))
        {
            return;
        }

        m_Map.Add(trans, toState);
    }
    public void DelTransition(ePlayerFSMTrans trans)
    {
        if (m_Map.ContainsKey(trans))
        {
            m_Map.Remove(trans);
        }

    }

    public PlayerFSMState TransitionTo(ePlayerFSMTrans trans)
    {
        if (m_Map.ContainsKey(trans) == false)
        {
            return null;
        }
        return m_Map[trans];
    }

    public virtual void DoBeforeEnter(PlayerFSMData data )
    {

    }

    public virtual void DoBeforeLeave(PlayerFSMData data)
    {

    }

    public virtual void Do(PlayerFSMData data)
    {

    }

    public virtual void CheckCondition(PlayerFSMData data)
    {

    }
}

public class PlayerFSMGunState : PlayerFSMState
{
    public PlayerFSMGunState()
    {
        m_StateID = ePlayerFSMStateID.GunStateID;

    }


    public override void DoBeforeEnter(PlayerFSMData data)
    {

    }

    public override void DoBeforeLeave(PlayerFSMData data)
    {

    }

    public override void Do(PlayerFSMData data)
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Shoot");
            // Animator shoot ...
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reload");
            // Animator Reload ...
        }
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Stratagem);
            Debug.Log("Stratagem");
        }
    }
}

public class PlayerFSMStratagemState : PlayerFSMState
{

    private float m_fIdleTim;


    public PlayerFSMStratagemState()
    {
        m_StateID = ePlayerFSMStateID.StratagemStateID;
    }


    public override void DoBeforeEnter(PlayerFSMData data)
    {

    }

    public override void DoBeforeLeave(PlayerFSMData data)
    {

    }

    public override void Do(PlayerFSMData data)
    {

    }

    public override void CheckCondition(PlayerFSMData data)
    {

    }
}
