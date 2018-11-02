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

    public Dictionary<int, StratagemInfo> StratagemTable;
    public Dictionary<int, WeaponInfo> WeaponInfoTable;
    public Dictionary<int, GrenadeInfo> GrenadeInfoTable;

    #endregion Tables

    public void Init()
    {
        if (m_Instance == null) m_Instance = this;

        StratagemTable = new Dictionary<int, StratagemInfo>();
        WeaponInfoTable = new Dictionary<int, WeaponInfo>();
        GrenadeInfoTable = new Dictionary<int, GrenadeInfo>();
        LoadGameData();
    }

    private void LoadGameData()
    {
        StratagemTable = StratagemDataLoader.LoadData("Table/StratagemTable");
        WeaponInfoTable = WeaponInfoLoader.LoadData("Table/WeaponInfoTable");
        GrenadeInfoTable = GrenadeInfoLoader.LoadData("Table/GrenadeInfoTable");
    }
}