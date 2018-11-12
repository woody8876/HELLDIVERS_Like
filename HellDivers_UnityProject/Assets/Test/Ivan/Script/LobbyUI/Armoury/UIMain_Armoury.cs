using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIMain_Armoury{

    public GameObject PrimaryWeapon;
    public GameObject SecondaryWeapon;
    public bool m_bPrimary;
    [Header("== In Game Data ==")]
    public string m_sPlayerID;
    public int m_iRank;
    public List<int> m_iWeapons;
    public List<int> m_iStrategems;

}
