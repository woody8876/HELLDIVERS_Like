using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobInfoLoader {

    public static Dictionary<int, MobInfo> LoadData(string filePath)
    {
        Dictionary<int, MobInfo> mobIngo = new Dictionary<int, MobInfo>();
        if (_LoadDataBase(filePath, ref mobIngo) == true)
        {
            return mobIngo;
        }
        else
        {
            return null;
        }
    }
    private static bool _LoadDataBase(string tablePath, ref Dictionary<int, MobInfo> Info)
    {
        Info.Clear();
        TextAsset datas = Resources.Load<TextAsset>(tablePath);
        if (datas != null)
        {
            string[] lines = datas.text.Split('\n');
            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] mobInfo = lines[i].Split(',');
                MobInfo data = new MobInfo();
                data.SetID(int.Parse(mobInfo[0]));
                data.SetHP(float.Parse(mobInfo[1]));
                data.SetProbeLength(float.Parse(mobInfo[2]));
                data.SetSight(float.Parse(mobInfo[3]));
                data.SetRadius(float.Parse(mobInfo[4]));
                data.SetVisionLength(float.Parse(mobInfo[5]));
                data.SetAttackRange(float.Parse(mobInfo[6]));
                data.SetAttackDamage(float.Parse(mobInfo[7]));
                data.SetMoney(float.Parse(mobInfo[8]));
                data.SetExp(float.Parse(mobInfo[9]));
                Info.Add(data.m_ID, data);
            }
            return true;
        }
        return false;
    }
}
