using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInfo m_Info;
    public PlayerInfo Info { get { return m_Info; } }
    public PlayerParts Parts { get; set; }
    public GameObject Display { get; private set; }
    public Animator Anima { get; private set; }

    private Weapon_Battle m_Weapon;
    private StratagemController m_Stratagem;

    private void Awake()
    {
        Display = Resources.Load("Characters/Ch01/ch01") as GameObject;
        Display = Instantiate(Display, this.transform);
        Display.transform.position -= Vector3.up;
        Anima = Display.GetComponent<Animator>();
        Parts = Display.GetComponent<PlayerParts>();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}