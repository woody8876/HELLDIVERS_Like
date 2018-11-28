using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaverManager{

    public static DataSaverManager Instance { private set; get; }
    public Dictionary<int, DataSaver> DataSaverMap { private set; get; }

    public void Init()
    {
        if (Instance == null) { Instance = this; }
        DataSaverMap = new Dictionary<int, DataSaver>();

        DataSaver[] saver = Resources.LoadAll<DataSaver>("PlayerDatas");
        foreach (var item in saver)
        {
            char cID = item.name[0];
            int iID = int.Parse(cID.ToString());
            DataSaverMap.Add(iID, item);
        }
    } 

}
