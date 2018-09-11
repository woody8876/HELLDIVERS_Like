using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StratagemDatabase
{
    public static Dictionary<int, StratagemData> StratagemMap { get; private set; }

    public static void LoadDataBase(string path, string fileName)
    {
        string fullPath = string.Format("{0}/{1}", path, fileName);
        TextAsset datas = Resources.Load(fullPath) as TextAsset;
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
                data.Code = GetCodes(colum[4]);
                data.Uses = int.Parse(colum[5]);
                data.Cooldown = float.Parse(colum[6]);
                data.Activation = float.Parse(colum[7]);
            }

            StratagemMap.Add(data.ID, data);
        }
        else
        {
            Debug.LogErrorFormat("Stratagem Database loading failed. (Can't find table file : {0})", fullPath);
        }

        Debug.Log("Stratagem database has loaded.");
    }

    private static StratagemCode[] GetCodes(string code)
    {
        StratagemCode[] codes = new StratagemCode[code.Length];

        for (int i = 0; i < code.Length; i++)
        {
            int state = int.Parse(code[i].ToString());

            switch (state)
            {
                case 1:
                    codes[i] = StratagemCode.Up;
                    break;

                case 2:
                    codes[i] = StratagemCode.Up;
                    break;

                case 3:
                    codes[i] = StratagemCode.Up;
                    break;

                default:
                    codes[i] = StratagemCode.Up;
                    break;
            }
        }

        return codes;
    }
}