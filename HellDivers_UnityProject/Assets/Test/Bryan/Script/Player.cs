using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bryan.Character
{
    [RequireComponent(typeof(Weapon_Battle))]
    [RequireComponent(typeof(StratagemController))]
    [RequireComponent(typeof(PlayerAnimationsContorller))]
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
#pragma warning disable
        private PlayerData m_Data;
        private PlayerController m_PlayerController;
        private Weapon_Battle m_WeapoonBattle;
        private StratagemController m_StratagemController;
        private PlayerParts m_Parts;
        private GameObject m_Display;
        private Animator m_Animator;
#pragma warning disable

        public void Initialize(PlayerData data)
        {
            m_Data = data;
        }

        // Use this for initialization
        private void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Parts = GetComponent<PlayerParts>();
            m_PlayerController = GetComponent<PlayerController>();
            m_WeapoonBattle = GetComponent<Weapon_Battle>();
            m_StratagemController = GetComponent<StratagemController>();
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}