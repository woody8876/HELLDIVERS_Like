using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public static PlayerManager Instance { get; private set; }
    public Dictionary<int, PlayerData> Players { get { return m_PlayerMap; } }
    private Dictionary<int, PlayerData> m_PlayerMap;

    public class PlayerData
    {
        public PlayerInfo info;
        public ControllerSetting controllerSetting;
    }

    public void Init()
    {
        if (Instance == null) Instance = this;
        m_PlayerMap = new Dictionary<int, PlayerData>();
    }

    public void CreatePlayer(int i, ControllerSetting controller)
    {
        PlayerData playerData = new PlayerData();
        PlayerInfo playerInfo = CreatPlayerInfo();
        playerData.info = playerInfo;
        playerData.controllerSetting = controller;
        m_PlayerMap.Add(i, playerData);
    }

    public PlayerInfo CreatPlayerInfo()
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.SetUsername("TestPlayer");
        playerInfo.AddWeapon(1101);
        playerInfo.AddWeapon(1301);
        playerInfo.AddStratagem(2001);
        playerInfo.AddStratagem(2002);
        playerInfo.AddStratagem(2003);
        playerInfo.AddStratagem(2000);
        playerInfo.Grenades.Add(4001);
        playerInfo.Grenades.Add(4002);
        playerInfo.Grenades.Add(4003);
        playerInfo.Grenades.Add(4004);
        playerInfo.UnlockItems();
        return playerInfo;
    }

    public void RefreshEquipment(int player, List<int> newEquipWeapons, List<int> newEquipStratagems)
    {
        m_PlayerMap[player].info.RefreshEquipWeapon(newEquipWeapons);
        m_PlayerMap[player].info.RefreshEquipStratagem(newEquipStratagems);
    }

    //public void LoadPlayerInfo()
    //{
    //}

    //public void SavePlayerInfo()
    //{
    //    foreach (KeyValuePair<int, PlayerInfo> player in Players)
    //    {
    //        PlayerInfo currentInfo = player.Value;
    //    }
    //}
}