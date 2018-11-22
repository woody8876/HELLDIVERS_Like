using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    #region Singleton

    private static GameData m_Instance;
    public static GameData Instance { get { return m_Instance; } }

    #endregion Singleton

    #region Tables

    public Dictionary<int, RankData> RankTable;
    public Dictionary<int, StratagemInfo> StratagemTable;
    public Dictionary<int, WeaponInfo> WeaponInfoTable;
    public Dictionary<int, GrenadeInfo> GrenadeInfoTable;
    public Dictionary<int, MobInfo> MobInfoTable;
    public Dictionary<int, List<int>> UnlockWeaponsTable;
    public Dictionary<int, List<int>> UnlockStratagemsTable;

    #endregion Tables

    public void Init()
    {
        if (m_Instance == null) m_Instance = this;
        RankTable = new Dictionary<int, RankData>();
        StratagemTable = new Dictionary<int, StratagemInfo>();
        WeaponInfoTable = new Dictionary<int, WeaponInfo>();
        GrenadeInfoTable = new Dictionary<int, GrenadeInfo>();
        MobInfoTable = new Dictionary<int, MobInfo>();
        UnlockWeaponsTable = new Dictionary<int, List<int>>();
        UnlockStratagemsTable = new Dictionary<int, List<int>>();
        LoadGameData();
    }

    private void LoadGameData()
    {
        RankTable = RankDataTableLoader.LoadData("Table/RankDataTable");
        StratagemTable = StratagemDataLoader.LoadData("Table/StratagemTable");
        WeaponInfoTable = WeaponInfoLoader.LoadData("Table/WeaponInfoTable");
        GrenadeInfoTable = GrenadeInfoLoader.LoadData("Table/GrenadeInfoTable");
        MobInfoTable = MobInfoLoader.LoadData("Table/MobInfoTable");
        UnlockWeaponsTable = UnlockItemLoader.LoadData("Table/UnlockWeaponsTable");
        UnlockStratagemsTable = UnlockItemLoader.LoadData("Table/UnlockStratagemsTable");
    }
}