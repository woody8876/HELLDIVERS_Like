using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StratagemDataLoader
{
    public static Dictionary<int, StratagemData> LoadData(string filePah)
    {
        Dictionary<int, StratagemData> stratagemTable = new Dictionary<int, StratagemData>();

        if (_LoadDataBase(filePah, ref stratagemTable) == true)
        {
            Debug.LogFormat("StratagemDatabase has been load.");
            return stratagemTable;
        }
        else
        {
            Debug.LogFormat("StratagemDatabase loading faild.");
            return null;
        }
    }

    private static bool _LoadDataBase(string tablePath, ref Dictionary<int, StratagemData> table)
    {
        table.Clear();

        TextAsset datas = Resources.Load<TextAsset>(tablePath);
        if (datas != null)
        {
            string[] line = datas.text.Split('\n');
            for (int i = 1; i < line.Length - 1; i++)
            {
                string[] colum = line[i].Split(',');

                StratagemData data = new StratagemData();
                data.id = int.Parse(colum[0]);
                data.rank = int.Parse(colum[1]);
                data.title = colum[2];
                data.type = (StratagemType)Enum.Parse(typeof(StratagemType), colum[3]);
                data.code = _GetCodes(colum[4]);
                data.uses = int.Parse(colum[5]);
                data.cooldown = float.Parse(colum[6]);
                data.activation = float.Parse(colum[7]);
                table.Add(data.id, data);
            }

            return true;
        }

        return false;
    }

    private static StratagemCode[] _GetCodes(string code)
    {
        StratagemCode[] codes = new StratagemCode[code.Length];

        for (int i = 0; i < code.Length; i++)
        {
            codes[i] = (StratagemCode)Enum.Parse(typeof(StratagemCode), code[i].ToString());
        }

        return codes;
    }

    public static void Print(Dictionary<int, StratagemData> table)
    {
        foreach (KeyValuePair<int, StratagemData> d in table)
        {
            string info = string.Format("Id:{0}, Titlle:{1}", d.Key, d.Value.title);
            Debug.Log(info);
        }
    }
}