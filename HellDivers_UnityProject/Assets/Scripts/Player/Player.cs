using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(StratagemController))]
[RequireComponent(typeof(PlayerAnimationsContorller))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(CharacterController))]
public class Player : Character, IHealable
{
    #region Private Variable

    private PlayerInfo m_Data;
    private PlayerParts m_Parts;
    private StratagemController m_StratagemController;
    private WeaponController m_WeapoonController;

    #endregion Private Variable

    public void Initialize(PlayerInfo data)
    {
        data.CopyTo(m_Data);

        // Setup stratagems
        if (m_StratagemController.Stratagems.Count > 0) m_StratagemController.Clear();
        m_StratagemController.AddStratagems(m_Data.Stratagems, m_Parts.RightHand);

        // Setup weapons
        m_WeapoonController.ClearWeapon();
        m_WeapoonController.AddMultiWeapons(m_Data.Weapons, m_Parts.LaunchPoint);
    }

    #region MonoBehaviour

    private void Awake()
    {
        this.tag = "Player";
        m_Data = new PlayerInfo();
        m_Parts = GetComponent<PlayerParts>();
        m_WeapoonController = GetComponent<WeaponController>();
        m_StratagemController = GetComponent<StratagemController>();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// Increase current health point HP.
    /// </summary>
    /// <param name="heal">Heal point</param>
    /// <returns>Was the current health point increased or not ?</returns>
    public bool TakeHealth(float heal)
    {
        if (IsDead || m_CurrentHp >= m_MaxHp) return false;

        CurrentHp += heal;
        return true;
    }

    public override void Death()
    {
        base.Death();
    }

    #endregion MonoBehaviour
}