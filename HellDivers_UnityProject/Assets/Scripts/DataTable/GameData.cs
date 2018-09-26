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

    public Dictionary<string, StratagemInfo> StratagemTable;

    #endregion Tables

    public GameData()
    {
        StratagemTable = new Dictionary<string, StratagemInfo>();
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

        #endregion Init Tables
    }
}