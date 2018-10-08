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
        private PlayerData m_Data;
        private PlayerParts m_Parts;
        private GameObject m_Display;
        private Animator m_Animator;

        private float m_Hp;

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}