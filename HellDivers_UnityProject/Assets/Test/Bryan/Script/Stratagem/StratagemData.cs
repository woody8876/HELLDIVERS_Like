using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemData
{
    public int ID { get; set; }
    public int Rank { get; set; }
    public string Title { get; set; }
    public StratagemType Type { get; set; }
    public StratagemCode[] Code { get; set; }
    public int Uses { get; set; }
    public float Cooldown { get; set; }
    public float Activation { get; set; }
}

public class StratagemDataBase
{
    public static Dictionary<int, StratagemData> StratagemMap { get; set; }

    private static void LoadDataBase()
    {
        string filePath = "Table/StratagemTable";
        TextAsset datas = Resources.Load(filePath) as TextAsset;
        if (datas != null)
        {
            StratagemData data = new StratagemData();

            string[] line = datas.text.Split('\n');
            for (int i = 0; i < line.Length; i++)
            {
                string[] colum = line[i].Split(',');

                data.ID = int.Parse(colum[0]);
                data.Rank = int.Parse(colum[1]);
                data.Title = colum[2];
                data.Type = (StratagemType)Enum.Parse(typeof(StratagemType), colum[3]);
                data.Uses = int.Parse(colum[5]);
                data.Cooldown = float.Parse(colum[6]);
                data.Activation = float.Parse(colum[7]);
            }

            StratagemMap.Add(data.ID, data);
        }
        else
        {
            Debug.LogErrorFormat("Load Stratagem Data Table ! ({0})", filePath);
        }
    }
}

public enum StratagemType
{
    Undefine = -1, Supply, Defensive, Offensive, Special
}

public enum StratagemCode
{
    Up, Down, Left, Right
}