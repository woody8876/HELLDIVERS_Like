using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoElement : MonoBehaviour
{
    private Player m_CurrentPlayer;

    [SerializeField] private Transform m_Panel_Title;
    [SerializeField] private Transform m_Panel_StratagemCD;
    [SerializeField] private Transform m_Panel_WeaponAndGrenade;
    [SerializeField] private Transform m_Panel_Weapon;
    [SerializeField] private Transform m_Panel_Grenade;
    [SerializeField] private Transform m_Panel_Stratagem;
    [SerializeField] private UIPlayerTitle m_PlayerTitle;
    [SerializeField] private GameObject m_StratagemCDInfoPrefab;
    [SerializeField] private GameObject m_WeaponInfoPrefab;
    [SerializeField] private GameObject m_GrenadeInfoPrefab;
    [SerializeField] private GameObject m_StratagemInfoPrefab;

    private void Awake()
    {
        if (m_Panel_Title == null) m_Panel_Title = this.transform.Find("Panel_Title");
        if (m_Panel_StratagemCD == null) m_Panel_StratagemCD = this.transform.Find("Panel_StratagemsCD");
        if (m_Panel_WeaponAndGrenade == null) m_Panel_WeaponAndGrenade = this.transform.Find("Panel_WeaponsAndGrenade");
        if (m_Panel_Weapon == null) m_Panel_Weapon = this.transform.Find("Panel_WeaponsAndGrenade/Weapons");
        if (m_Panel_Grenade == null) m_Panel_Grenade = this.transform.Find("Panel_WeaponsAndGrenade/Grenades");
        if (m_Panel_Stratagem == null) m_Panel_Stratagem = this.transform.Find("Panel_Stratagems");
    }

    public void Init(Player player)
    {
        m_CurrentPlayer = player;

        m_PlayerTitle = Instantiate(m_PlayerTitle, m_Panel_Title).GetComponent<UIPlayerTitle>();
        m_PlayerTitle.Init(player);
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