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
    Go_SwitchWeapon,
    Go_Dead,
}
public enum ePlayerFSMStateID
{
    NullStateID,
    GunStateID,
    ReloadStateID,
    StratagemStateID,
    ThrowStateID,
    SwitchWeaponID,
    DeadStateID,
}

public class PlayerFSMState
{

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
        if (GameData.Instance.WeaponInfoTable[data.m_WeaponController._CurrentWeapon].FireMode == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (data.m_WeaponController.ShootState()) shoot = true;
                else shoot = false;
            }
            else shoot = false;
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                if (data.m_WeaponController.ShootState()) shoot = true;
            }
            else shoot = false;
        }
        data.m_AnimationController.SetAnimator(m_StateID, shoot);
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        if (Input.GetButtonDown("Reload"))
        {
            if (data.m_WeaponController.ReloadState())
            {
                data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Reload);
            }
        }
        if (Input.GetButton("Stratagem"))
        {
            if (data.m_StratagemController.Stratagems.Count > 0)
            {
                data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Stratagem);
            }
        }
        if (Input.GetButton("WeaponSwitch"))
        {
            if (data.m_WeaponController.SwitchWeaponState())
            {
                data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_SwitchWeapon);
            }
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
        if (count < 1) data.m_AnimationController.SetAnimator(m_StateID);
        count++;
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        AnimatorStateInfo info = data.m_AnimationController.Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("Reload"))
        {
            if (data.m_AnimationController.FinishAnimator(data))
            {
                data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Gun);
            }
        }
    }
}

public class PlayerFSMStratagemState : PlayerFSMState
{
    public PlayerFSMStratagemState()
    {
        m_StateID = ePlayerFSMStateID.StratagemStateID;
    }
    public override void DoBeforeEnter(PlayerFSMData data)
    {
        data.m_NowAnimation = "Stratagem";
        data.m_Animator.SetBool("InputCodes", true);
        data.m_AnimationController.SetAnimator(m_StateID, false);
    }

    public override void DoBeforeLeave(PlayerFSMData data)
    {
        
    }

    public override void Do(PlayerFSMData data)
    {
        if (data.m_StratagemController.IsCheckingCode == false)
        {
            data.m_StratagemController.StartCheckCodes();
        }
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        if (data.m_StratagemController.IsReady)
        {
            data.m_AnimationController.SetAnimator(m_StateID, true);
            data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Throw);
        }

        else if (Input.GetButtonUp("Stratagem"))
        {
            data.m_StratagemController.StopCheckCodes();
            data.m_Animator.SetBool("InputCodes", false);
            data.m_AnimationController.SetAnimator(m_StateID, data.m_StratagemController.IsReady);
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
        data.m_AnimationController.SetAnimator(m_StateID, false);
    }

    public override void DoBeforeLeave(PlayerFSMData data)
    {
        data.m_Animator.SetBool("InputCodes", false);
        data.m_AnimationController.SetAnimator(m_StateID, false);
    }

    public override void Do(PlayerFSMData data)
    {
        if (Input.GetButtonUp("Fire1"))
        {
            data.m_NowAnimation = "Throwing";
            data.m_AnimationController.SetAnimator(m_StateID, true);
        }

        //data.m_AnimationController.Animator = data.m_AnimationController.Animator;
        AnimatorStateInfo info = data.m_AnimationController.Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("ThrowOut"))
        {
            if (info.normalizedTime > 0.7f)
            {
                //Throw(time)...
                data.m_StratagemController.Throw();
            }
        }
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        data.m_AnimationController.Animator = data.m_AnimationController.Animator;
        AnimatorStateInfo info = data.m_AnimationController.Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("ThrowOut"))
        {
            if (data.m_AnimationController.FinishAnimator(data))
            {
                data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Gun);
            }
        }
    }
}

public class PlayerFSMSwitchWeaponState : PlayerFSMState
{
    int count = 0;
    public PlayerFSMSwitchWeaponState()
    {
        m_StateID = ePlayerFSMStateID.SwitchWeaponID;
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
        if (count < 1) data.m_AnimationController.SetAnimator(m_StateID);
        count++;
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        AnimatorStateInfo info = data.m_AnimationController.Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("TakeWeapon"))
        {
            if (data.m_AnimationController.FinishAnimator(data))
            {
                data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Gun);
            }
        }
    }
}

public class PlayerFSMDeadState : PlayerFSMState
{
    public PlayerFSMDeadState()
    {
        m_StateID = ePlayerFSMStateID.DeadStateID;
    }


    public override void DoBeforeEnter(PlayerFSMData data)
    {
        data.m_AnimationController.SetAnimator(m_StateID, false);
        data.m_NowAnimation = "Dead";
        data.m_AnimationController.SetAnimator(m_StateID);
    }

    public override void DoBeforeLeave(PlayerFSMData data)
    {
    }

    public override void Do(PlayerFSMData data)
    {
        AnimatorStateInfo info = data.m_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Death"))
        {
            if (info.normalizedTime < 0.95f) data.m_PlayerController.bIsDead = false;
            else data.m_PlayerController.bIsDead = true;
            return;
        }
    }

    public override void CheckCondition(PlayerFSMData data)
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            data.m_AnimationController.SetAnimator(m_StateID,true);
            data.m_PlayerController.bIsDead = false;
            data.m_PlayerFSMSystem.PerformTransition(ePlayerFSMTrans.Go_Gun);
        }
    }
}

