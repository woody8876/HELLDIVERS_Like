using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfoLoader {

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
                WeaponInfo data = new WeaponInfo();
                data.SetID(int.Parse(weaponInfo[0]));
                data.SetType(int.Parse(weaponInfo[1]));
                data.SetTitle(weaponInfo[2]);
                data.SetDamage(float.Parse(weaponInfo[3]));
                data.SetExplosion(float.Parse(weaponInfo[4]));
                data.SetFireRate(float.Parse(weaponInfo[5]));
                data.SetCapacity(int.Parse(weaponInfo[6]));
                data.SetStart_Mags(int.Parse(weaponInfo[7]));
                data.SetMax_Mags(int.Parse(weaponInfo[8]));
                data.SetEmpty_Reload_Speed (float.Parse(weaponInfo[9]));
                data.SetTactical_Reload_Speed(float.Parse(weaponInfo[10]));
                data.SetMin_Spread (float.Parse(weaponInfo[11]));
                data.SetMax_Spread (float.Parse(weaponInfo[12]));
                data.SetSpread_Increase_per_shot (float.Parse(weaponInfo[13]));
                data.SetRange (float.Parse(weaponInfo[14]));
                data.SetFireMode (float.Parse(weaponInfo[15]));
                data.SetName(weaponInfo[16]);
                Info.Add(data.ID, data);
            }
            return true;
        }
        return false;
    }


}
