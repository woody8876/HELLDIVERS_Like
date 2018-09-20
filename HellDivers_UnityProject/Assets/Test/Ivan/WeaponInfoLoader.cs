using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfoLoader  {

	





    private static bool _LoadDataBase(string tablePsth, ref Dictionary<int, Weapon> Info)
    {

        TextAsset datas = Resources.Load<TextAsset>(tablePsth);
        if (datas != null)
        {
            string[] lines = datas.text.Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string[] weaponInfo = lines[i].Split(',');
                int index;
                Weapon data = new Weapon();
                index = int.Parse(weaponInfo[1]);
                data.Damage = int.Parse(weaponInfo[2]);
                data.Center_Damage = int.Parse(weaponInfo[3]);
                data.Explosion_Damage = int.Parse(weaponInfo[4]);
                data.FireRate = float.Parse(weaponInfo[5]);
                data.Capacity = int.Parse(weaponInfo[6]);
                data.Start_Mags = int.Parse(weaponInfo[7]);
                data.Max_Mags = int.Parse(weaponInfo[8]);
                data.Empty_Reload_Speed = float.Parse(weaponInfo[9]);
                data.Tactical_Reload_Speed= float.Parse(weaponInfo[10]);
                data.Min_Spread = float.Parse(weaponInfo[11]);
                data.Max_Spread = float.Parse(weaponInfo[12]);
                data.Spread_Increase_per_shot = float.Parse(weaponInfo[13]);
                data.RANGE = float.Parse(weaponInfo[14]);
                data.FireMode = float.Parse(weaponInfo[15]);
                Info.Add(index, data);
            }
            return true;
        }
        return false;
    }


}
