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
    [SerializeField] int CurWeaponID;
    [SerializeField] int Coin;
    #endregion

    public int curWeaponID { get { return CurWeaponID; } }
    public SetPlayerWeapon SetPlayer { get { return m_SetPlayer; } }
    public UI_WeaponInfo Info { get { return m_Info; } }
    public UIWeaponList WeaponList { get { return m_WeaponList; } }
    public GameObject SelectButton { get { return m_Button.Select; } }

    #region Private Field
    #endregion

    #region MonoBehaviors
    #endregion

    #region Support Method
    public void ChangeCurID(int i) { CurWeaponID = i; }

    private void CheckCoin()
    {
        int coin = int.Parse(m_coin.text);
    }
    #endregion

}
