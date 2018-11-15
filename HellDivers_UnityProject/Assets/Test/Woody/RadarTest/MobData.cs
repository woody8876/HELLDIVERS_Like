using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobData {

    private static MobData m_Instance;
    public static MobData Instance { get { return m_Instance; } }

    public Dictionary<int, AIData> AIDataTable;

    public void Init()
    {
        if (m_Instance == null) m_Instance = this;

        AIDataTable = new Dictionary<int, AIData>();
        LoadGameData();
    }
    private void LoadGameData()
    {
        AIDataTable = MobInfoLoader.LoadData("Table/MobInfoTable");
    }
}
