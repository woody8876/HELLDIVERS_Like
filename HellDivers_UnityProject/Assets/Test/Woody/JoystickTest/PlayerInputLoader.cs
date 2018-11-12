using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputLoader {

    public static Dictionary<int, InputInfo> LoadData(string filePath)
    {
        Dictionary<int, InputInfo> playerInputInfo = new Dictionary<int, InputInfo>();
        if (_LoadDataBase(filePath, ref playerInputInfo) == true)
        {
            return playerInputInfo;
        }
        else
        {
            return null;
        }
    }

    private static bool _LoadDataBase(string tablePath, ref Dictionary<int, InputInfo> Info)
    {
        Info.Clear();
        TextAsset datas = Resources.Load<TextAsset>(tablePath);
        if (datas != null)
        {
            string[] lines = datas.text.Split('\n');
            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] playerInputInfo = lines[i].Split(',');
                InputInfo data = new InputInfo();
                data.SetID(int.Parse(playerInputInfo[0]));
                data.SetHorizontal(playerInputInfo[1]);
                data.SetVertical(playerInputInfo[2]);
                data.SetDirectionHorizontal(playerInputInfo[3]);
                data.SetDirectionVertical(playerInputInfo[4]);
                data.SetStratagemHorizontal(playerInputInfo[5]);
                data.SetStratagemVertical(playerInputInfo[6]);
                data.SetFire(playerInputInfo[7]);
                data.SetStratagem(playerInputInfo[8]);
                data.SetRun(playerInputInfo[9]);
                data.SetWeaponSwitch(playerInputInfo[10]);
                data.SetReload(playerInputInfo[11]);
                data.SetMeleeAttack(playerInputInfo[12]);
                data.SetInteractive(playerInputInfo[13]);
                data.SetRoll(playerInputInfo[14]);
                data.SetGrenade(playerInputInfo[15]);
                Info.Add(data.ID, data);
            }
            return true;
        }
        return false;
    }
}
