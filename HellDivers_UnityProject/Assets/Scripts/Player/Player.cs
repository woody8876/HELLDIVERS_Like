using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StratagemController))]
[RequireComponent(typeof(Weapon_Battle))]
[RequireComponent(typeof(PlayerAnimationsContorller))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public PlayerInfo Info { get; set; }
    public PlayerParts Parts { get; set; }
    public GameObject Display { get; private set; }
    public Animator Anima { get; private set; }

    private void Awake()
    {
        this.tag = "Player";
    }

    public bool Init(PlayerInfo playerInfo)
    {
        if (playerInfo == null) return false;

        Info = playerInfo;

        Anima = GetComponent<Animator>();
        Parts = GetComponent<PlayerParts>();

        return true;
    }

    // Use this for initialization
    private void Start()
    {
        Camera.main.GetComponent<CameraController>().m_Player = this.transform;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}