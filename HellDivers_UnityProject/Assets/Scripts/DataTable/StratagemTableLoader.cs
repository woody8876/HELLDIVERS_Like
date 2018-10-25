using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StratagemDataLoader
{
    public static string Path;

    public static Dictionary<int, StratagemInfo> LoadData(string filePah)
    {
        Dictionary<int, StratagemInfo> stratagemTable = new Dictionary<int, StratagemInfo>();

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

    private static bool _LoadDataBase(string tablePath, ref Dictionary<int, StratagemInfo> table)
    {
        table.Clear();

        TextAsset datas = Resources.Load<TextAsset>(tablePath);
        if (datas != null)
        {
            string[] line = datas.text.Split('\n');
            for (int i = 1; i < line.Length - 1; i++)
            {
                string[] colum = line[i].Split(',');
                StratagemInfo data = new StratagemInfo();
                FieldInfo[] infos = data.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                infos[0].SetValue(data, int.Parse(colum[0]));
                infos[1].SetValue(data, int.Parse(colum[1]));
                infos[2].SetValue(data, colum[2]);
                infos[3].SetValue(data, (StratagemInfo.eType)Enum.Parse(typeof(StratagemInfo.eType), colum[3]));
                infos[4].SetValue(data, _GetCodes(colum[4]));
                infos[5].SetValue(data, int.Parse(colum[5]));
                infos[6].SetValue(data, float.Parse(colum[6]));
                infos[7].SetValue(data, float.Parse(colum[7]));
                infos[8].SetValue(data, colum[8]);
                infos[9].SetValue(data, colum[9]);

                table.Add(data.ID, data);
            }

            return true;
        }

        return false;
    }

    private static StratagemInfo.eCode[] _GetCodes(string code)
    {
        StratagemInfo.eCode[] codes = new StratagemInfo.eCode[code.Length];

        for (int i = 0; i < code.Length; i++)
        {
            codes[i] = (StratagemInfo.eCode)Enum.Parse(typeof(StratagemInfo.eCode), code[i].ToString());
        }

        return codes;
    }
}