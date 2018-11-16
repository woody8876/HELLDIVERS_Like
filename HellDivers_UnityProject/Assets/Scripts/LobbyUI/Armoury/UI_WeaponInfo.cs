using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponInfo : MonoBehaviour {
    
    #region SerializeField
    [Header("== Set Current UI ==")]
    [SerializeField] Text m_WeaponName;
    [SerializeField] Image m_IWeaponTexture;
    [SerializeField] GameObject m_WeaponAbilities;
    [SerializeField] GameObject m_WeaponAbility;
    [Header("== Set LevelUP UI ==")]

    #endregion

    public Dictionary<int, List<int>> weapons = new Dictionary<int, List<int>>();
    int m_iType;
    int m_iCurrentID;
    UIWeaponAbility m_Power;
    UIWeaponAbility m_Magazine;
    UIWeaponAbility m_FireRate;
    UIWeaponAbility m_Range;
    UIWeaponAbility m_Stability;
    UIWeaponAbility m_FireMode;


    public void SetWeapon()
    {
        float cur;
        float next;
        float max;
        string mode;
        Get_Power(out cur, out next, out max);
        m_Power.SetAbility(m_Power.name, GetLength(cur, max), GetLength(next, max));
        Get_FireRate(out cur, out next, out max);
        m_Magazine.SetAbility(m_Power.name, GetLength(cur, max), GetLength(next, max));
        Get_Stability(out cur, out next, out max);
        m_FireRate.SetAbility(m_Power.name, GetLength(cur, max), GetLength(next, max));
        Get_Magazine(out cur, out next, out max);
        m_Range.SetAbility(m_Power.name, GetLength(cur, max), GetLength(next, max));
        Get_Range(out cur, out next, out max);
        m_Stability.SetAbility(m_Power.name, GetLength(cur, max), GetLength(next, max));
        Get_FireMode(out mode);
        m_FireMode.SetWord(m_FireMode.name, mode);
    }

    #region Get Value Method
    private void Get_Power(out float cur, out float next, out float max)
    {
        List<int> pList = weapons[m_iType];
        int MaxLevel = pList[pList.Count - 1];
        cur = GameData.Instance.WeaponInfoTable[m_iCurrentID].Damage;
        if (m_iCurrentID != MaxLevel)
        {
            max = GameData.Instance.WeaponInfoTable[MaxLevel].Damage;
            next = GameData.Instance.WeaponInfoTable[m_iCurrentID + 1].Damage;
        }else { max = next = cur; }
    }
    private void Get_FireRate(out float cur, out float next, out float max)
    {
        List<int> pList = weapons[m_iType];
        int MaxLevel = pList[pList.Count - 1];
        cur = GameData.Instance.WeaponInfoTable[m_iCurrentID].FirePerMinute;
        if (m_iCurrentID != MaxLevel)
        {
            max = GameData.Instance.WeaponInfoTable[MaxLevel].FirePerMinute;
            next = GameData.Instance.WeaponInfoTable[m_iCurrentID + 1].FirePerMinute;
        } else { max = next = cur; }
    }
    private void Get_Stability(out float cur, out float next, out float max)
    {
        List<int> pList = weapons[m_iType];
        int MaxLevel = pList[pList.Count - 1];
        cur = GameData.Instance.WeaponInfoTable[m_iCurrentID].Max_Spread;
        if (m_iCurrentID != MaxLevel)
        {
            max = GameData.Instance.WeaponInfoTable[MaxLevel].Max_Spread;
            next = GameData.Instance.WeaponInfoTable[m_iCurrentID + 1].Max_Spread;
        }
        else { max = next = cur; }

    }
    private void Get_Magazine(out float cur, out float next, out float max)
    {
        List<int> pList = weapons[m_iType];
        int MaxLevel = pList[pList.Count - 1];
        cur = GameData.Instance.WeaponInfoTable[m_iCurrentID].Max_Mags;
        if (m_iCurrentID != MaxLevel)
        {
            max = GameData.Instance.WeaponInfoTable[MaxLevel].Max_Mags;
            next = GameData.Instance.WeaponInfoTable[m_iCurrentID + 1].Max_Mags;
        }
        else { max = next = cur; }

    }
    private void Get_Range(out float cur, out float next, out float max)
    {
        List<int> pList = weapons[m_iType];
        int MaxLevel = pList[pList.Count - 1];
        cur = GameData.Instance.WeaponInfoTable[m_iCurrentID].Range;
        if (m_iCurrentID != MaxLevel)
        {
            max = GameData.Instance.WeaponInfoTable[MaxLevel].Range;
            next = GameData.Instance.WeaponInfoTable[m_iCurrentID + 1].Range;
        }
        else { max = next = cur; }

    }
    private void Get_FireMode(out string mode)
    {
        float i = GameData.Instance.WeaponInfoTable[m_iCurrentID].FireMode;
        mode = (i == 0) ? "SEMI - AUTO" : "FULL - AUTO";
    }
    #endregion

    #region Create Method
    private void CreateDictionary()
    {
        foreach (var item in GameData.Instance.WeaponInfoTable)
        {
            if (weapons.ContainsKey(GameData.Instance.WeaponInfoTable[item.Key].Type) == false)
            {
                List<int> pList = new List<int>();
                pList.Add(item.Key);
                weapons.Add(GameData.Instance.WeaponInfoTable[item.Key].Type, pList);
            }
            else
            {
                weapons[GameData.Instance.WeaponInfoTable[item.Key].Type].Add(item.Key);
            }
        }
    }
    private void CreateObject(UIWeaponAbility ability, string name)
    {
        ability = Instantiate(m_WeaponAbility, m_WeaponAbilities.transform).GetComponent<UIWeaponAbility>();
        ability.name = name;        
    }
    private void GetCurrentID(LobbyUI_Weapon uI_Weapon)
    {
        m_iType = uI_Weapon.m_WeaponInfo.Type;
        m_iCurrentID = uI_Weapon.m_WeaponInfo.ID;
    }
    private float GetLength(float target, float max)
    {
        float length = (target / max) * 200;
        return length;
    }
    #endregion Create Method

    private void Start()
    {
        CreateDictionary();
        CreateObject(m_Power, "Power");
        CreateObject(m_Magazine, "Magazine");
        CreateObject(m_FireRate, "FireRate");
        CreateObject(m_Range, "Range");
        CreateObject(m_Stability, "Stability");
        CreateObject(m_FireMode, "FireMode");
    }
}
