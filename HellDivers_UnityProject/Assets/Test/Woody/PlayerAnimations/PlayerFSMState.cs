using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlayerFSMTrans
{
    NullTransition,
    Go_Gun,
    Go_Reload,
    Go_Stratagem,
    Go_Throw,
}
public enum ePlayerFSMStateID
{
    NullStateID,
    GunStateID,
    ReloadStateID,
    StratagemStateID,
    ThrowStateID,
}

public class PlayerFSMState
{

    public ePlayerFSMStateID m_StateID;
    public Dictionary<ePlayerFSMTrans, PlayerFSMState> m_Map;
    public float m_fCurrentTime;
    public PlayerAnimationsContorller m_PAC = MonoBehaviour.FindObjectOfType<PlayerAnimationsContorller>();
    public WeaponController WP = MonoBehaviour.FindObjectOfType<WeaponController>();
    public StratagemController SC = MonoBehaviour.FindObjectOfType<StratagemController>();
    public Animator m_Animator;

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

    public virtual void DoBeforeEnter(PlayerFSMData data)
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
    bool shoot;
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
            shoot = true;
        }
        else
        {
            shoot = false;
        }
        m_PAC.Attack(m_StateID, shoot);
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Reload);
        }
        if (Input.GetButton("Stratagem") && !shoot)
        {
            data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Stratagem);
        }
    }
}

public class PlayerFSMReloadState : PlayerFSMState
{
    int count = 0;
    public PlayerFSMReloadState()
    {
        m_StateID = ePlayerFSMStateID.ReloadStateID;
    }


    public override void DoBeforeEnter(PlayerFSMData data)
    {
        count = 0;
    }

    public override void DoBeforeLeave(PlayerFSMData data)
    {

    }

    public override void Do(PlayerFSMData data)
    {
        if (count < 1) m_PAC.Attack(m_StateID, false);
        count++;
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        m_Animator = m_PAC.Animator;
        AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("Reload"))
        {
            if (m_PAC.FinishAnimator(data))
            {
                data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Gun);
            }
        }
    }
}

public class PlayerFSMStratagemState : PlayerFSMState
{
    int count = 0;
    bool bCorrect;
    public PlayerFSMStratagemState()
    {
        m_StateID = ePlayerFSMStateID.StratagemStateID;
    }
    public override void DoBeforeEnter(PlayerFSMData data)
    {
        count = 0;
        data.m_NowAnimation = "Stratagem";
        Debug.Log(data.m_NowAnimation);
    }

    public override void DoBeforeLeave(PlayerFSMData data)
    {
       
    }

    public override void Do(PlayerFSMData data)
    {
        if(count < 1) m_PAC.Attack(m_StateID, true);
        count++;
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Throw);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            bCorrect = false;
            m_PAC.Attack(m_StateID, bCorrect);
            data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Gun);
        }
    }
}

public class PlayerFSMThrowState : PlayerFSMState
{
    
    public PlayerFSMThrowState()
    {
        m_StateID = ePlayerFSMStateID.ThrowStateID;
    }


    public override void DoBeforeEnter(PlayerFSMData data)
    {
        data.m_NowAnimation = "Throw";
    }

    public override void DoBeforeLeave(PlayerFSMData data)
    {
        m_Animator.SetBool("ThrowStandby", false);
    }

    public override void Do(PlayerFSMData data)
    {
        if (Input.GetMouseButtonDown(0))
        {
            data.m_NowAnimation = "Throwing";
            m_PAC.Attack(m_StateID, false);
        }
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        m_Animator = m_PAC.Animator;
        AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("ThrowOut"))
        {
            if (m_PAC.FinishAnimator(data))
            {
                data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Gun);
            }
        }
    }
}
