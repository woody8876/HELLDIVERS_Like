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
    public Dictionary<int, Weapon> WeaponInfoTable;
    #endregion Tables

    public GameData()
    {
        StratagemTable = new Dictionary<int, StratagemInfo>();
        WeaponInfoTable = new Dictionary<int, Weapon>();
    }

    public void LoadGameData()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else return;

        #region Init Tables

        StratagemTable = StratagemDataLoader.LoadData("Table/StartagemTable");
        WeaponInfoTable = WeaponInfoLoader.LoadData("Table/WeaponInfoTable");

        #endregion Init Tables
    }
}