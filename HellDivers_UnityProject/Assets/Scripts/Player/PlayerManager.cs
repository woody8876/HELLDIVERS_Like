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
        PlayerInfo playerInfo = CreatPlayerInfo();
        m_PlayerMap.Add(1, playerInfo);
    }

    public PlayerInfo CreatPlayerInfo()
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.SetUsername("TestPlayer");
        playerInfo.AddWeapon(1101);
        playerInfo.AddWeapon(1301);
        playerInfo.UnlockWeapon(1001);
        playerInfo.UnlockWeapon(1101);
        playerInfo.UnlockWeapon(1301);
        playerInfo.UnlockWeapon(1401);
        playerInfo.AddStratagem(2000);
        playerInfo.AddStratagem(2001);
        playerInfo.AddStratagem(2002);
        playerInfo.Grenades.Add(4001);
        playerInfo.Grenades.Add(4002);
        playerInfo.Grenades.Add(4003);
        playerInfo.Grenades.Add(4004);
        return playerInfo;
    }

    public void RefreshEquipment(int player, List<int> newEquipWeapon)
    {
        m_PlayerMap[player].RefreshEquipWeapon(newEquipWeapon);
    }
        
    public void LoadPlayerInfo()
    {
    }

    public void SavePlayerInfo()
    {
        foreach (KeyValuePair<int, PlayerInfo> player in Players)
        {
            PlayerInfo currentInfo = player.Value;
        }
    }
}