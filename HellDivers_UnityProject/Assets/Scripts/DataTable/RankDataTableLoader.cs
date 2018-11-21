using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class RankData
{
    public int Rank { get { return rank; } }
    public int Exp { get { return exp; } }

    private int rank;
    private int exp;
}

public static class RankDataTableLoader
{
    public static Dictionary<int, RankData> LoadData(string path)
    {
        Dictionary<int, RankData> table = new Dictionary<int, RankData>();
        if (_LoadDataTable(path, ref table))
        {
            Debug.Log("RankDatabase has been load.");
            return table;
        }
        else
        {
            Debug.Log("RankDatabase loading failed.");
            return null;
        }
    }

    private static bool _LoadDataTable(string path, ref Dictionary<int, RankData> table)
    {
        TextAsset tableFile = Resources.Load<TextAsset>(path);
        if (tableFile == null) return false;

        string[] line = tableFile.text.Split('\n');
        for (int i = 1; i < line.Length - 1; i++)
        {
            string[] colum = line[i].Split(',');
            RankData data = new RankData();
            FieldInfo[] infos = data.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            infos[0].SetValue(data, int.Parse(colum[0]));
            infos[1].SetValue(data, int.Parse(colum[1]));
            table.Add(data.Rank, data);
        }

        return true;
    }
}