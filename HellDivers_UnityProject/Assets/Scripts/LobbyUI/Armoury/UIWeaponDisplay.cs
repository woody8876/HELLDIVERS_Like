using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIWeaponDisplay : MonoBehaviour {

    #region SerializeField
    [Header("== GameObject Setting ==")]
//    [SerializeField] GameObject m_WeaponUI;
    [SerializeField] Text m_coin;
//    [SerializeField] GameObject m_Content;
    [Header("== Script Setting ==")]
    [SerializeField] SetPlayerWeapon m_SetPlayer;
    [SerializeField] UI_WeaponInfo m_Info;
    [SerializeField] UIWeaponList m_WeaponList;
    [SerializeField] UI_WeaponButton m_Button;
    [Header("== Private Field ==")]
    [SerializeField] int _curWeaponID;
    [SerializeField] int Coin;
    #endregion

    public int CurWeaponID { get { return _curWeaponID; } }
    public SetPlayerWeapon SetPlayer { get { return m_SetPlayer; } }
    public UI_WeaponInfo Info { get { return m_Info; } }
    public UIWeaponList WeaponList { get { return m_WeaponList; } }
    public GameObject SelectButton { get { return m_Button.Select; } }
    public GameObject LevelUpButton { get { return m_Button.LevelUp; } }

    #region Private Field
    #endregion


    #region Support Method
    public void SetCurID(int i) { _curWeaponID = i; }

    public void SetWeaponUI(GameObject go, int current, bool setCurID = true)
    {
        go.GetComponent<LobbyUI_Weapon>().SetID(current);
        go.GetComponent<LobbyUI_Weapon>().SetWeaponUI();
        if (setCurID) _curWeaponID = current;
    }


    private void CheckCoin()
    {
        int coin = int.Parse(m_coin.text);
    }
    #endregion

}
