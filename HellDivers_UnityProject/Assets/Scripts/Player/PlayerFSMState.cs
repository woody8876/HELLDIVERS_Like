using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlayerFSMTrans
{
    NullTransition,
    Go_Start,
    Go_Gun,
    Go_MeleeAttack,
    Go_Reload,
    Go_Stratagem,
    Go_Throw,
    Go_SwitchWeapon,
    Go_PickUp,
    Go_Victory,
    Go_Dead,
    Go_Relive,
    Go_Roll,
}

public enum ePlayerFSMStateID
{
    NullStateID,
    StartStateID,
    GunStateID,
    MeleeAttackID,
    ReloadStateID,
    StratagemStateID,
    ThrowStateID,
    SwitchWeaponID,
    PickUpID,
    VictoryID,
    DeadStateID,
    ReliveStateID,
    RollStateID,
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

    public virtual void DoBeforeEnter(PlayerController data)
    {
    }

    public virtual void DoBeforeLeave(PlayerController data)
    {
    }

    public virtual void Do(PlayerController data)
    {
    }

    public virtual void CheckCondition(PlayerController data)
    {
    }
}

public class PlayerFSMStartState : PlayerFSMState
{
    public PlayerFSMStartState()
    {
        m_StateID = ePlayerFSMStateID.StartStateID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
    }

    public override void DoBeforeLeave(PlayerController data)
    {
    }

    public override void Do(PlayerController data)
    {
        data.m_MoveMode = "Stop";
    }

    public override void CheckCondition(PlayerController data)
    {
        AnimatorStateInfo info = data.m_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Standing"))
        {
            if (info.normalizedTime > 0.9f)
            {
                data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Gun);
            }
        }
    }
}

public class PlayerFSMGunState : PlayerFSMState
{
    private bool shoot;

    public PlayerFSMGunState()
    {
        m_StateID = ePlayerFSMStateID.GunStateID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
    }

    public override void DoBeforeLeave(PlayerController data)
    {
        data.m_PAC.SetAnimator(m_StateID, false);
    }

    public override void Do(PlayerController data)
    {
        if (GameData.Instance.WeaponInfoTable[data.m_WeaponController._CurrentWeapon].FireMode == 0)
        {
            if (Input.GetAxis("Fire1") < 0 || Input.GetButton("Fire1"))
            {
                if (data.m_WeaponController.ShootState())
                {
                    data.m_PAC.SetAnimator(m_StateID);
                    shoot = true;
                }
                else shoot = false;
            }
            else shoot = false;
        }
        else
        {
            if ((Input.GetAxis("Fire1") < 0 || Input.GetButton("Fire1")) && data.m_WeaponController.CurrentWeaponInfo.Ammo > 0)
            {
                if (data.m_WeaponController.ShootState())
                {
                    data.m_PAC.SetAnimator(m_StateID);
                    shoot = true;
                }
            }
            else
            {
                data.m_WeaponController.m_fSpreadIncrease = 0;
                shoot = false;
            }
        }
    }

    public override void CheckCondition(PlayerController data)
    {
        if (Input.GetButton("Reload"))
        {
            if (data.m_WeaponController.ReloadState())
            {
                data.m_PAC.SetAnimator(m_StateID, false);
                data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Reload);
            }
        }
        else if (Input.GetButton("Stratagem") && shoot == false)
        {
            if (data.m_StratagemController.Stratagems.Count > 0)
            {
                data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Stratagem);
            }
        }
        else if (Input.GetButtonDown("WeaponSwitch"))
        {
            if (data.m_WeaponController.SwitchWeaponState())
            {
                data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_SwitchWeapon);
            }
        }
        else if (Input.GetButtonDown("Interactive"))
        {
            data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_PickUp);
        }
        else if (Input.GetButtonDown("MeleeAttack"))
        {
            data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_MeleeAttack);
        }
    }
}

public class PlayerFSMMeleeAttackState : PlayerFSMState
{
    public PlayerFSMMeleeAttackState()
    {
        m_StateID = ePlayerFSMStateID.MeleeAttackID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
        data.m_PAC.SetAnimator(m_StateID);
        data.m_MoveMode = "Stop";
    }

    public override void DoBeforeLeave(PlayerController data)
    {
    }

    public override void Do(PlayerController data)
    {
    }

    public override void CheckCondition(PlayerController data)
    {
        AnimatorStateInfo info = data.m_PAC.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("MeleeAttack"))
        {
            if (info.normalizedTime > 0.8f)
            {
                data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Gun);
            }
        }
    }
}

public class PlayerFSMReloadState : PlayerFSMState
{
    public PlayerFSMReloadState()
    {
        m_StateID = ePlayerFSMStateID.ReloadStateID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
        AnimatorStateInfo info = data.m_PAC.Animator.GetCurrentAnimatorStateInfo(1);
        if (!info.IsName("Reload") || !data.m_PAC.Animator.IsInTransition(1))
        {
            float speed = data.m_WeaponController.CurrentWeaponInfo.ReloadSpeed;
            speed = (3.33f / speed);
            data.m_PAC.SetAnimator(m_StateID, speed);
        }
    }

    public override void DoBeforeLeave(PlayerController data)
    {
    }

    public override void Do(PlayerController data)
    {

    }

    public override void CheckCondition(PlayerController data)
    {
        AnimatorStateInfo info = data.m_PAC.Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("Reload"))
        {
            if(info.normalizedTime > 0.9f)
            data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Gun);
        }
        else
        {
            data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Gun);
        }
    }
}

public class PlayerFSMStratagemState : PlayerFSMState
{
    public PlayerFSMStratagemState()
    {
        m_StateID = ePlayerFSMStateID.StratagemStateID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
        data.m_MoveMode = "Stop";
        data.m_PAC.SetAnimator(m_StateID, true);
        data.m_StratagemController.StartCheckCodes();
    }

    public override void DoBeforeLeave(PlayerController data)
    {
        data.m_PAC.SetAnimator(m_StateID, false);
    }

    public override void Do(PlayerController data)
    {
    }

    public override void CheckCondition(PlayerController data)
    {
        if (data.m_StratagemController.IsReady)
        {
            data.m_PAC.SetAnimator(m_StateID);
            data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Throw);
        }
        else if (!Input.GetButton("Stratagem"))
        {
            data.m_StratagemController.StopCheckCodes();
            data.m_PAC.SetAnimator(m_StateID, false);
            data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Gun);
        }
    }
}

public class PlayerFSMThrowState : PlayerFSMState
{
    bool bThrow;
    public PlayerFSMThrowState()
    {
        m_StateID = ePlayerFSMStateID.ThrowStateID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
        bThrow = false;
        data.m_MoveMode = "Throw";
    }

    public override void DoBeforeLeave(PlayerController data)
    {
        data.m_PAC.SetAnimator(m_StateID, false);
    }

    public override void Do(PlayerController data)
    {
        if (Input.GetAxis("Fire1") < 0)
        {
            bThrow = true;
            //Start Timer...
        }

        if ((Input.GetAxis("Fire1") == 0 && bThrow) || Input.GetButton("Fire1"))
        {
            data.m_MoveMode = "Throwing";
            data.m_PAC.SetAnimator(m_StateID, true);
            //Get Force...
        }

        AnimatorStateInfo info = data.m_PAC.Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("ThrowOut"))
        {
            if (info.normalizedTime > 0.5f)
            {
                data.m_StratagemController.Throw();
            }
        }
    }

    public override void CheckCondition(PlayerController data)
    {
        AnimatorStateInfo info = data.m_PAC.Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("ThrowOut"))
        {
            if (info.normalizedTime > 0.9f)
            {
                data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Gun);
            }
        }
    }
}

public class PlayerFSMSwitchWeaponState : PlayerFSMState
{
    public PlayerFSMSwitchWeaponState()
    {
        m_StateID = ePlayerFSMStateID.SwitchWeaponID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
        data.m_MoveMode = "MoveOnly";
        data.m_PAC.SetAnimator(m_StateID);
    }

    public override void DoBeforeLeave(PlayerController data)
    {
    }

    public override void Do(PlayerController data)
    {
    }

    public override void CheckCondition(PlayerController data)
    {
        AnimatorStateInfo info = data.m_PAC.Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("SwitchWeapon"))
        {
            if (info.normalizedTime > 0.9f)
            {
                data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Gun);
            }
        }
    }
}

public class PlayerFSMPickUpState : PlayerFSMState
{
    private float Count = 0;

    public PlayerFSMPickUpState()
    {
        m_StateID = ePlayerFSMStateID.PickUpID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
        Count = 0;
        data.m_PAC.SetAnimator(m_StateID);
    }

    public override void DoBeforeLeave(PlayerController data)
    {
    }

    public override void Do(PlayerController data)
    {
        data.m_MoveMode = "Stop";
    }

    public override void CheckCondition(PlayerController data)
    {
        AnimatorStateInfo info = data.m_PAC.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("PickUp"))
        {
            if (info.normalizedTime > 0.6f && Count < 1)
            {
                data.m_Player.InteractWithItem();
                Count++;
            }
            if (info.normalizedTime > 0.9f)
            {
                data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Gun);
            }
        }
    }
}

public class PlayerFSMVictoryState : PlayerFSMState
{
    public PlayerFSMVictoryState()
    {
        m_StateID = ePlayerFSMStateID.VictoryID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
        data.m_PAC.SetAnimator(m_StateID);
    }

    public override void DoBeforeLeave(PlayerController data)
    {
    }

    public override void Do(PlayerController data)
    {
    }

    public override void CheckCondition(PlayerController data)
    {
        AnimatorStateInfo info = data.m_PAC.Animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("Victory"))
        {
            if (info.normalizedTime > 0.8f)
            {
                data.m_PlayerFSM.PerformPreviousTransition();
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

    public override void DoBeforeEnter(PlayerController data)
    {
        data.m_MoveMode = "Dead";
        data.m_PAC.SetAnimator(m_StateID);
    }

    public override void DoBeforeLeave(PlayerController data)
    {
    }

    public override void Do(PlayerController data)
    {
        AnimatorStateInfo info = data.m_Animator.GetCurrentAnimatorStateInfo(3);
        if (info.IsName("Death"))
        {
            if (info.normalizedTime < 0.95f) data.bIsDead = false;
            else
            {
                data.bIsDead = true;
                data.bIsAlive = false;
            }
            return;
        }
    }

    public override void CheckCondition(PlayerController data)
    {
    }
}

public class PlayerFSMReliveState : PlayerFSMState
{
    public PlayerFSMReliveState()
    {
        m_StateID = ePlayerFSMStateID.ReliveStateID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
        data.m_MoveMode = "Stop";
        data.m_PAC.SetAnimator(m_StateID);
    }

    public override void DoBeforeLeave(PlayerController data)
    {
        data.m_MoveMode = "Origin";
        data.m_PAC.ResetAnimator(data);
    }

    public override void Do(PlayerController data)
    {
        AnimatorStateInfo info = data.m_Animator.GetCurrentAnimatorStateInfo(3);
        if (info.IsName("Relive"))
        {
            if (info.normalizedTime < 0.9f) data.bIsAlive = false;
            else
            {
                data.bIsAlive = true;
                data.bIsDead = false;
            }
            return;
        }
    }

    public override void CheckCondition(PlayerController data)
    {
        AnimatorStateInfo info = data.m_PAC.Animator.GetCurrentAnimatorStateInfo(3);
        if (info.IsName("Relive"))
        {
            if (info.normalizedTime > 0.95f)
            {
                data.m_PlayerFSM.PerformTransition(ePlayerFSMTrans.Go_Gun);
                data.m_PlayerFSM.PerformPreviousTransition();
            }
        }
    }
}

public class PlayerFSMRollState : PlayerFSMState
{
    public PlayerFSMRollState()
    {
        m_StateID = ePlayerFSMStateID.RollStateID;
    }

    public override void DoBeforeEnter(PlayerController data)
    {
        data.m_MoveMode = "Stop";
        data.m_PAC.SetAnimator(m_StateID);
    }

    public override void DoBeforeLeave(PlayerController data)
    {
        data.m_MoveMode = "Origin";
    }

    public override void Do(PlayerController data)
    {
    }

    public override void CheckCondition(PlayerController data)
    {
        AnimatorStateInfo info = data.m_PAC.Animator.GetCurrentAnimatorStateInfo(3);
        if (info.IsName("Roll"))
        {
            if (info.normalizedTime > 0.8f)
            {
                data.m_PlayerFSM.PerformPreviousTransition();
            }
        }
    }
}