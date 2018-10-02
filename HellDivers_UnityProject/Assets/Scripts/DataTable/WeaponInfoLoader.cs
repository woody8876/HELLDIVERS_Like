using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfoLoader  {

    public static Dictionary<int, WeaponInfo> LoadData(string filePath)
    {
        Dictionary<int, WeaponInfo> weaponInfo = new Dictionary<int, WeaponInfo>();
        if (_LoadDataBase(filePath, ref weaponInfo) == true)
        {
            Debug.Log("WeaponInfo has been load.");
            return weaponInfo;
        }
        else
        {
            Debug.Log("WeaponInfo load failed.");
            return null;
        }
    }

    private static bool _LoadDataBase(string tablePath, ref Dictionary<int, WeaponInfo> Info)
    {
        Info.Clear();
        TextAsset datas = Resources.Load<TextAsset>(tablePath);
        if (datas != null)
        {
            string[] lines = datas.text.Split('\n');
            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] weaponInfo = lines[i].Split(',');
                int index;
                WeaponInfo data = new WeaponInfo();
                index = int.Parse(weaponInfo[1]);
                data._Damage = int.Parse(weaponInfo[2]);
                data._Center_Damage = int.Parse(weaponInfo[3]);
                data._Explosion_Damage = int.Parse(weaponInfo[4]);
                data._FireRate = float.Parse(weaponInfo[5]);
                data._Capacity = int.Parse(weaponInfo[6]);
                data._Start_Mags = int.Parse(weaponInfo[7]);
                data._Max_Mags = int.Parse(weaponInfo[8]);
                data._Empty_Reload_Speed = float.Parse(weaponInfo[9]);
                data._Tactical_Reload_Speed= float.Parse(weaponInfo[10]);
                data._Min_Spread = float.Parse(weaponInfo[11]);
                data._Max_Spread = float.Parse(weaponInfo[12]);
                data._Spread_Increase_per_shot = float.Parse(weaponInfo[13]);
                data._Range = float.Parse(weaponInfo[14]);
                data._FireMode = float.Parse(weaponInfo[15]);
                Info.Add(index, data);
            }
            return true;
        }
        return false;
    }


}
