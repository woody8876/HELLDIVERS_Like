using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(StratagemController))]
[RequireComponent(typeof(PlayerAnimationsContorller))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    #region Private Variable

#pragma warning disable
    private PlayerInfo m_Data;
    private PlayerParts m_Parts;
    private PlayerController m_PlayerController;
    private StratagemController m_StratagemController;
    private WeaponController m_WeapoonController;
#pragma warning disable

    //private List<int> weapons = new List<int> { 1301};

    #endregion Private Variable

    public void Initialize(PlayerInfo data)
    {
        data.CopyTo(m_Data);

        // Setup stratagems
        if (m_StratagemController.Stratagems.Count > 0) m_StratagemController.Clear();
        m_StratagemController.AddStratagems(m_Data.stratagems, m_Parts.RightHand);

        // Setup weapons
        m_WeapoonController.AddMultiWeapons(m_Data.weapons, m_Parts.LaunchPoint);
    }

    #region MonoBehaviour

    private void Awake()
    {
        this.tag = "Player";
        m_Data = new PlayerInfo();
        m_Parts = GetComponent<PlayerParts>();
        m_PlayerController = GetComponent<PlayerController>();
        m_WeapoonController = GetComponent<WeaponController>();
        m_StratagemController = GetComponent<StratagemController>();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    #endregion MonoBehaviour
}