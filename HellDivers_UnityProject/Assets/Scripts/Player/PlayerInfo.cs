using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    #region Properties

    public string Username { get { return username; } }
    public int Rank { get { return rank; } }
    public int Exp { get { return exp; } }
    public List<int> Weapons { get { return weapons; } }
    public List<int> Stratagems { get { return stratagems; } }
    public List<int> Grenades { get { return grenades; } }
    public List<int> UnLockWeapons { get { return unlockweapons; } }
    public int Money { get { return money; } }

    #endregion Properties

    #region Private Variable

    [SerializeField] private List<int> unlockweapons = new List<int>();
    [SerializeField] private List<int> weapons = new List<int>();
    [SerializeField] private List<int> stratagems = new List<int>();
    [SerializeField] private List<int> grenades = new List<int>();
    [SerializeField] private string username;
    [SerializeField] private int rank;
    [SerializeField] private int money;
    [SerializeField] private int exp;
    private int totalMissionFought;
    private int totalMissionWon;
    private int totalDeaths;
    private int totalShotsFired;
    private int totalEnemiesKilled;

    #endregion Private Variable

    #region Public Function

    public bool SetUsername(string name)
    {
        if (username != null && username.Length > 50) return false;

        username = name;
        return true;
    }

    public bool UnlockWeapon(int id)
    {
        foreach (var item in unlockweapons) { if (id == item) return false; }
        unlockweapons.Add(id);
        return true;
    }

    public bool LevelUpWeapon(ref int id)
    {
        for (int i = 0; i < unlockweapons.Count; i++)
        {
            if (unlockweapons[i] == id)
            {
                id += 1;
                unlockweapons[i] = id;
            }
        }
        return false;
    }

    public bool RefreshEquipWeapon(List<int> pList)
    {
        if (weapons.Count < 1) { return false; }
        weapons = pList;
        return true;
    }

    public bool AddWeapon(int id)
    {
        if (GameData.Instance.WeaponInfoTable.ContainsKey(id) == false) return false;

        weapons.Add(id);
        return true;
    }

    public bool AddStratagem(int id)
    {
        if (GameData.Instance.StratagemTable.ContainsKey(id) == false) return false;

        stratagems.Add(id);
        return true;
    }

    public void AddMoney(int money)
    {
        this.money += money;
    }

    public void AddExp(int exp)
    {
        this.exp += exp;
    }

    public PlayerInfo Clone()
    {
        PlayerInfo clone = new PlayerInfo();
        CopyTo(clone);
        return clone;
    }

    public void CopyTo(PlayerInfo other)
    {
        other.username = this.username;
        other.rank = this.rank;
        other.totalMissionFought = this.totalMissionFought;
        other.totalMissionWon = this.totalMissionWon;
        other.totalDeaths = this.totalDeaths;
        other.totalShotsFired = this.totalShotsFired;
        other.totalEnemiesKilled = this.totalEnemiesKilled;
        other.weapons = this.weapons;
        other.stratagems = this.stratagems;
        other.grenades = this.grenades;
        other.money = this.money;
        other.exp = this.exp;
    }

    #endregion Public Function
}