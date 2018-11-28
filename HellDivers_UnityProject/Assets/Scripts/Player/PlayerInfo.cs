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
    public List<int> UnlockedWeapons { get { return unlockWeapons; } }
    public List<int> UnlockedStratagems { get { return unlockStratagems; } }
    public List<int> Weapons { get { return weapons; } }
    public List<int> Stratagems { get { return stratagems; } }
    public List<int> Grenades { get { return grenades; } }
    public int Money { get { return money; } }

    #endregion Properties

    #region Private Variable

    [SerializeField] private List<int> unlockWeapons = new List<int>();
    [SerializeField] private List<int> unlockStratagems = new List<int>();
    [SerializeField] private List<int> weapons = new List<int>();
    [SerializeField] private List<int> stratagems = new List<int>();
    [SerializeField] private List<int> grenades = new List<int>();
    [SerializeField] private string username;
    [SerializeField] private int rank = 1;
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

    #region Level UP to Unlock Items

    public void UnlockItems()
    {
        UnlockWeapons();
        UnlockStratagems();
    }

    private void UnlockWeapons()
    {
        if (GameData.Instance.UnlockWeaponsTable.ContainsKey(rank) == false) return;
        for (int i = 0; i < GameData.Instance.UnlockWeaponsTable[rank].Count; i++)
        {
            bool exist = false;
            int ids = GameData.Instance.UnlockWeaponsTable[rank][i];
            foreach (int id in unlockWeapons)
            {
                if (GameData.Instance.WeaponInfoTable[id].Type == GameData.Instance.WeaponInfoTable[ids].Type)
                {
                    exist = true;
                    break;
                }
            }
            if (!exist) unlockWeapons.Add(GameData.Instance.UnlockWeaponsTable[rank][i]);
        }
    }

    private void UnlockStratagems()
    {
        if (GameData.Instance.UnlockStratagemsTable.ContainsKey(rank) == false) return;
        for (int i = 0; i < GameData.Instance.UnlockStratagemsTable[rank].Count; i++)
        {
            bool exist = false;
            int ids = GameData.Instance.UnlockStratagemsTable[rank][i];
            foreach (int id in unlockStratagems)
            {
                if (id == ids)
                {
                    exist = true;
                    break;
                }
            }
            if (!exist) unlockStratagems.Add(GameData.Instance.UnlockStratagemsTable[rank][i]);
        }
    }

    public bool LevelUpWeapon(ref int id)
    {
        for (int i = 0; i < unlockWeapons.Count; i++)
        {
            if (unlockWeapons[i] == id)
            {
                id += 1;
                unlockWeapons[i] = id;
            }
        }
        return false;
    }

    #endregion

    #region Refresh List

    public bool RefreshEquipWeapon(List<int> pList)
    {
        if (pList.Count < 1) { return false; }
        weapons = pList;
        return true;
    }

    public bool RefreshEquipStratagem(List<int> pList)
    {
        if (pList.Count < 1) { return false; }
        stratagems = pList;
        return true;
    }

    public bool RefreshEquipGrenade(List<int> pList)
    {
        if (pList.Count < 1) { return false; }
        grenades = pList;
        return true;
    }

    public bool RefreshUnlockedWeapons(List<int> pList)
    {
        if (pList.Count < 1) { return false; }
        unlockWeapons = pList;
        return true;
    }

    public bool RefreshUnlockedStratagems(List<int> pList)
    {
        if (pList.Count < 1) { return false; }
        unlockStratagems = pList;
        return true;
    }

    #endregion

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

    public void CostMoney(int money)
    {
        if (this.money < money) return;
        this.money -= money;
    }

    public void AddExp(int exp)
    {
        this.exp += exp;
        LevelUp(this.exp);
    }

    private void LevelUp(int exp)
    {
        while (exp > GameData.Instance.RankTable[rank + 1].Exp)
        {
            rank++;
            UnlockItems();
        }
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