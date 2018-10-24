using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSMData
{
    public PlayerFSMSystem m_PlayerFSMSystem;
    public bool m_FinishAni = false;
    public string m_NowAnimation = "Origin";
    public PlayerController m_PlayerController;
    public CharacterController m_CharacterController;
    public PlayerAnimationsContorller m_AnimationController;
    public WeaponController m_WeaponController;
    public StratagemController m_StratagemController;
    public Animator m_Animator;
}
