﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    public Player CurrentPlayer { get { return m_Player; } }

    private Player m_Player;
    private Dictionary<int, UIPlayerWeaponInfo> m_UIWeapons;
    private Dictionary<int, UIPlayerStratagemInfo> m_UIStratagems;
    [SerializeField] private Text m_PlayerName;
    [SerializeField] private Text m_PlayerRank;
    [SerializeField] private Image m_PlayerRankImg;
    [SerializeField] private GameObject m_WeaponInfoPrefab;
    [SerializeField] private GameObject m_StratagemInfoPrefab;

    public void Initialize(Player player)
    {
        m_Player = player;

        InitTitleDisplay();
        InitWeaponUI();
        InitStratagemDisplay();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void InitTitleDisplay()
    {
        if (m_Player == null) return;

        m_PlayerName.text = m_Player.Info.Username;
        m_PlayerRank.text = m_Player.Info.Rank.ToString();

        string rankImgFile = "icon_rank_";
        if (m_Player.Info.Rank < 1)
        {
            rankImgFile += "01";
        }
        else if (m_Player.Info.Rank > 40)
        {
            rankImgFile += "40";
        }
        else
        {
            rankImgFile += m_Player.Info.Rank.ToString("00");
        }

        string rankImgPath = string.Format("UI/Resource/Icons/Rank/{0}", rankImgFile);
        Sprite rankImg = Resources.Load<Sprite>(rankImgPath);
        m_PlayerRankImg.sprite = rankImg;
    }

    #region UI Weapon

    private void InitWeaponUI()
    {
        if (m_Player == null) return;

        m_UIWeapons = new Dictionary<int, UIPlayerWeaponInfo>();
        foreach (KeyValuePair<int, IWeaponBehaviour> weapon in m_Player.WeaponController.ActiveWeapon)
        {
            UIPlayerWeaponInfo weaponUI = Instantiate(m_WeaponInfoPrefab, this.transform).GetComponent<UIPlayerWeaponInfo>();
            weaponUI.Initialize(weapon.Value.weaponInfo);
            weaponUI.gameObject.SetActive(false);

            m_Player.WeaponController.OnFire += weaponUI.UpdateAmmoDisplay;
            m_Player.WeaponController.OnReload += weaponUI.StartReload;
            m_Player.WeaponController.OnReloadEnd += weaponUI.UpdateAmmoDisplay;

            m_UIWeapons.Add(weapon.Value.weaponInfo.ID, weaponUI);
        }

        DrawCurrentWeaponUI();
        m_Player.WeaponController.OnSwitch += DrawCurrentWeaponUI;
    }

    private void DrawCurrentWeaponUI()
    {
        if (m_UIWeapons.Count <= 0) return;

        foreach (KeyValuePair<int, UIPlayerWeaponInfo> weaponUI in m_UIWeapons)
        {
            if (weaponUI.Key == m_Player.WeaponController._CurrentWeapon)
            {
                weaponUI.Value.gameObject.SetActive(true);
                weaponUI.Value.UpdateAmmoDisplay();
            }
            else
            {
                if (weaponUI.Value.isActiveAndEnabled) weaponUI.Value.gameObject.SetActive(false);
            }
        }
    }

    private void HideWeaponUI()
    {
        if (m_UIWeapons.Count <= 0) return;

        foreach (KeyValuePair<int, UIPlayerWeaponInfo> weaponUI in m_UIWeapons)
        {
            if (weaponUI.Value.isActiveAndEnabled) weaponUI.Value.gameObject.SetActive(false);
        }
    }

    #endregion UI Weapon

    #region UI Stratagem

    private void InitStratagemDisplay()
    {
        if (m_Player == null) return;

        m_UIStratagems = new Dictionary<int, UIPlayerStratagemInfo>();
        for (int i = 0; i < m_Player.StratagemController.Stratagems.Count; i++)
        {
            Stratagem currentStratagem = m_Player.StratagemController.Stratagems[i];
            UIPlayerStratagemInfo stratagemUI = Instantiate(m_StratagemInfoPrefab, this.transform).GetComponent<UIPlayerStratagemInfo>();
            stratagemUI.Initialize(currentStratagem);
            stratagemUI.gameObject.SetActive(false);

            currentStratagem.OnThrow += stratagemUI.UpdateUses;
            m_Player.StratagemController.OnStartCheckingCode += DrawStratagemUI;
            m_Player.StratagemController.OnStopCheckingCode += HideStratagemUI;
            m_Player.StratagemController.OnCheckingCode += CheckingCodes;

            m_UIStratagems.Add(currentStratagem.Info.ID, stratagemUI);
        }
    }

    private void DrawStratagemUI()
    {
        HideWeaponUI();

        foreach (KeyValuePair<int, UIPlayerStratagemInfo> stratagemUI in m_UIStratagems)
        {
            stratagemUI.Value.UpdateUses();
            if (stratagemUI.Value.CurrentStratagem.IsCooling == false) stratagemUI.Value.StartCheckCodes();
            stratagemUI.Value.gameObject.SetActive(true);
        }
    }

    private void CheckingCodes()
    {
        foreach (KeyValuePair<int, UIPlayerStratagemInfo> stratagemUI in m_UIStratagems)
        {
            if (m_Player.StratagemController.StratagemsOnCheckingCode.Contains(stratagemUI.Value.CurrentStratagem))
            {
                stratagemUI.Value.CheckingCode(m_Player.StratagemController.InputCodeStep);
            }
            else
            {
                stratagemUI.Value.StopCheckCodes();
            }
        }
    }

    private void HideStratagemUI()
    {
        foreach (KeyValuePair<int, UIPlayerStratagemInfo> stratagemUI in m_UIStratagems)
        {
            stratagemUI.Value.gameObject.SetActive(false);
        }

        DrawCurrentWeaponUI();
    }

    #endregion UI Stratagem
}