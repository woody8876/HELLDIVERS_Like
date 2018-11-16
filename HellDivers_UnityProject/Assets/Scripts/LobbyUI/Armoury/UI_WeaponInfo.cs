using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponInfo : MonoBehaviour {
    
    #region SerializeField
    [Header("== Set Current UI ==")]
    [SerializeField] Text m_WeaponName;
    [SerializeField] Image m_IWeaponTexture;

    [SerializeField] Text m_FireMode;
    [Header("== Set LevelUP UI ==")]

    #endregion

    int m_iType;
    int m_iCurrentID;
    Vector2 m_Size = new Vector2();
    Dictionary<int, List<int>> weapons = new Dictionary<int, List<int>>();

    public void SetWeapon(LobbyUI_Weapon uI_Weapon)
    {
        m_Size.y = -2;
        //SetCurrentUI(uI_Weapon);
        //SetCurrent_FireMode(uI_Weapon);
        //SetCurrent_FireRate(uI_Weapon);
        //SetCurrent_Power(uI_Weapon);
        //SetCurrent_Magazine(uI_Weapon);
        //SetCurrent_Range(uI_Weapon);
        //SetCurrent_Stability(uI_Weapon);
    }


    #region Private method
    private void GetCurrentID(LobbyUI_Weapon uI_Weapon)
    {
        m_iType = uI_Weapon.m_WeaponInfo.Type;
        m_iCurrentID = uI_Weapon.m_WeaponInfo.ID;
    }

    private void SetCurrentUI()
    {

    }
    private void SetCurrent_Power()
    {
    }
    private void SetCurrent_FireRate()
    {
    }
    private void SetCurrent_Stability()
    {
    }
    private void SetCurrent_Magazine()
    {
    }
    private void SetCurrent_Range()
    {
    }
    private void SetCurrent_FireMode(LobbyUI_Weapon uI_Weapon)
    {
        m_FireMode.text = (uI_Weapon.m_WeaponInfo.FireMode == 0) ? "SEMI - AUTO" : "FULL - AUTO";
    }
    #endregion
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



    private void Start()
    {
        CreateDictionary();
    }
}
