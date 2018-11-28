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
        public DataSaver data;
    }

    public void Init()
    {
        if (Instance == null) Instance = this;
        m_PlayerMap = new Dictionary<int, PlayerData>();
    }

    public void CreatePlayer(int i, ControllerSetting controller, DataSaver data)
    {
        PlayerData playerData = new PlayerData();
        PlayerInfo playerInfo = CreatPlayerInfo(i);
        playerData.info = playerInfo;
        playerData.controllerSetting = controller;
        playerData.data = data;
        if (m_PlayerMap.ContainsKey(i)) m_PlayerMap[i] = playerData;
        else m_PlayerMap.Add(i, playerData);
    }

    public void LoadPlayer(int i, ControllerSetting controller, DataSaver data)
    {
        PlayerData playerData = new PlayerData();
        PlayerInfo playerInfo = LoadPlayerInfo(data);
        playerData.info = playerInfo;
        playerData.controllerSetting = controller;
        playerData.data = data;
        if (m_PlayerMap.ContainsKey(i)) m_PlayerMap[i] = playerData;
        else m_PlayerMap.Add(i, playerData);
    }

    public PlayerInfo CreatPlayerInfo(int i)
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.SetUsername("Player" + i);
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

    public PlayerInfo LoadPlayerInfo(DataSaver data)
    {
        PlayerInfo player = new PlayerInfo();
        player.SetUsername(data.Username);
        player.AddExp(data.Exp);
        player.AddMoney(data.Money);
        player.RefreshEquipWeapon(data.Weapons);
        player.RefreshEquipStratagem(data.Stratagems);
        player.RefreshEquipGrenade(data.Grenades);
        player.RefreshUnlockedWeapons(data.UnlockedWeapons);
        player.RefreshUnlockedStratagems(data.UnlockedStratagems);

        return player;
    }

    public void SavePlayerInfo(int i)
    {
        PlayerInfo info = Players[i].info;
        DataSaver data = Players[i].data;
        data.Username = info.Username;
        data.Rank = info.Rank;
        data.Exp = info.Exp;
        data.Money = info.Money;
        data.Weapons = info.Weapons;
        data.Stratagems = info.Stratagems;
        data.Grenades = info.Grenades;
        data.UnlockedWeapons = info.UnlockedWeapons;
        data.UnlockedStratagems = info.UnlockedStratagems;
    }

    public void RefreshEquipment(int player, List<int> newEquipWeapons, List<int> newEquipStratagems)
    {
        m_PlayerMap[player].info.RefreshEquipWeapon(newEquipWeapons);
        m_PlayerMap[player].info.RefreshEquipStratagem(newEquipStratagems);
    }


}