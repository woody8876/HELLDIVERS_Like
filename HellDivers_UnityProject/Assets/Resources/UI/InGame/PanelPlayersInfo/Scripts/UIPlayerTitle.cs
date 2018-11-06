﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerTitle : MonoBehaviour
{
    public Player CurrentPlayer { get; private set; }
    [SerializeField] private Image m_RankIcon;
    [SerializeField] private Image m_DeadIcon;
    [SerializeField] private Text m_RankText;
    [SerializeField] private Text m_NameText;

    public void Init(Player player)
    {
        CurrentPlayer = player;

        int rank = CurrentPlayer.Info.Rank;
        if (rank > 40) rank = 40;
        else if (rank < 0) rank = 0;
        string fileName = string.Format("icon_rank_{0}", rank.ToString("00"));
        Sprite loadedSprite = UIHelper.LoadSprite(UIHelper.RankIconFolder, fileName);
        if (loadedSprite != null) m_RankIcon.sprite = loadedSprite;

        RefershInfo();
        SubscribePlayerEvent();
    }

    private void OnDestroy()
    {
        UnsubcribePlayerEvent();
    }

    private void SubscribePlayerEvent()
    {
        CurrentPlayer.OnStartSpawnNotify += OnAliveView;
        CurrentPlayer.OnStartDeathNotify += OnDeathView;
    }

    private void UnsubcribePlayerEvent()
    {
        CurrentPlayer.OnStartSpawnNotify -= OnAliveView;
        CurrentPlayer.OnStartDeathNotify -= OnDeathView;
    }

    private void OnAliveView()
    {
        m_RankIcon.gameObject.SetActive(true);
        m_RankText.gameObject.SetActive(true);
        m_DeadIcon.gameObject.SetActive(false);

        RefershInfo();
    }

    private void OnDeathView()
    {
        m_RankIcon.gameObject.SetActive(false);
        m_RankText.gameObject.SetActive(false);
        m_DeadIcon.gameObject.SetActive(true);

        RefershInfo();
    }

    public void RefershInfo()
    {
        m_RankText.text = CurrentPlayer.Info.Rank.ToString();
        m_NameText.text = CurrentPlayer.Info.Username;
    }
}