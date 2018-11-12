using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public static PlayerManager Instance { get; private set; }
    public Dictionary<int, PlayerInfo> Players { get { return m_PlayerMap; } }
    private Dictionary<int, PlayerInfo> m_PlayerMap;

    public void Init()
    {
        if (Instance == null) Instance = this;
        m_PlayerMap = new Dictionary<int, PlayerInfo>();
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.SetUsername("TestPlayer");
        playerInfo.AddWeapon(1101);
        playerInfo.AddWeapon(1301);
        playerInfo.AddStratagem(2001);
        playerInfo.AddStratagem(2002);
        m_PlayerMap.Add(0, playerInfo);
    }

    public void LoadPlayer()
    {
    }

    public void SavePlayer()
    {
    }
}