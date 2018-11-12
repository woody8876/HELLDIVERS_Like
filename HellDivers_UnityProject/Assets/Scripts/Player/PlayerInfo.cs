using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    #region Properties

    public string Username { get { return username; } }
    public int Rank { get { return rank; } }
    public List<int> Weapons { get { return weapons; } }
    public List<int> Stratagems { get { return stratagems; } }
    public List<int> Grenades { get { return grenades; } }
    public int Money { get { return money; } }

    #endregion Properties

    #region Private Variable

    [SerializeField] private List<int> weapons = new List<int>();
    [SerializeField] private List<int> stratagems = new List<int>();
    [SerializeField] private List<int> grenades = new List<int>();
    [SerializeField] private string username;
    [SerializeField] private int rank;
    [SerializeField] private int money;
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
        other.money = this.money;
    }

    #endregion Public Function
}