using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameMain : MonoBehaviour
{
    public static UIGameMain Instance { get; private set; }

    [SerializeField] private Transform m_PlayerInfoPanel;
    [SerializeField] private GameObject m_PlayerInfoPrefab;
    [SerializeField] private Transform m_StratagemActPanel;
    [SerializeField] private GameObject m_StratagemActPrefab;
    [SerializeField] private GameObject m_DynamicHPBar;

    private class PlayerUI
    {
        public UIPlayerInfo m_PlayerInfo;
        public UIDynamicHPBar m_HPBar;
        public List<UIPlayerStratagemAct> m_StratagemActTimers = new List<UIPlayerStratagemAct>();
    }

    private Dictionary<Player, PlayerUI> m_PlayerUIMap;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        m_PlayerUIMap = new Dictionary<Player, PlayerUI>();
    }

    public void AddPlayer(Player player)
    {
        PlayerUI playerUI = new PlayerUI();

        GameObject playerInfo = Instantiate(m_PlayerInfoPrefab, m_PlayerInfoPanel);
        UIPlayerInfo uiPlayerInfo = playerInfo.GetComponent<UIPlayerInfo>();
        uiPlayerInfo.Initialize(player);
        playerUI.m_PlayerInfo = uiPlayerInfo;

        GameObject playerHpBar = Instantiate(m_DynamicHPBar, this.transform);
        UIDynamicHPBar hpBar = playerHpBar.GetComponent<UIDynamicHPBar>();
        hpBar.Initialize(player);
        playerUI.m_HPBar = hpBar;

        for (int i = 0; i < player.StratagemController.Stratagems.Count; i++)
        {
            GameObject stratagemActTimer = Instantiate(m_StratagemActPrefab, m_StratagemActPanel);
            UIPlayerStratagemAct actTimer = stratagemActTimer.GetComponent<UIPlayerStratagemAct>();
            actTimer.Initialize(player.StratagemController.Stratagems[i]);
            playerUI.m_StratagemActTimers.Add(actTimer);
        }

        m_PlayerUIMap.Add(player, playerUI);
    }

    public void RemovePlayer(Player player)
    {
        if (m_PlayerUIMap.ContainsKey(player) == false) return;

        PlayerUI playerUI = m_PlayerUIMap[player];
        Destroy(playerUI.m_PlayerInfo.gameObject);
        Destroy(playerUI.m_HPBar);

        for (int i = 0; i < playerUI.m_StratagemActTimers.Count; i++)
        {
            Destroy(playerUI.m_StratagemActTimers[i].gameObject);
        }

        m_PlayerUIMap.Remove(player);
    }
}