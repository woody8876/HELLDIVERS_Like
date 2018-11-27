using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIWeaponDisplay : MonoBehaviour {

    #region SerializeField
    [Header("== GameObject Setting ==")]
    [SerializeField] Text m_coin;
    [Header("== Script Setting ==")]
    [SerializeField] SetPlayerWeapon m_SetPlayer;
    [SerializeField] UI_WeaponInfo m_Info;
    [SerializeField] UIWeaponList m_WeaponList;
    [SerializeField] UI_WeaponButton m_Button;
    [Header("== Private Field ==")]
    [SerializeField] int _curWeaponID;
    #endregion

    #region Public Getter

    public int CurWeaponID { get { return _curWeaponID; } }
    public SetPlayerWeapon SetPlayer { get { return m_SetPlayer; } }
    public UI_WeaponInfo Info { get { return m_Info; } }
    public UIWeaponList WeaponList { get { return m_WeaponList; } }
    public UI_WeaponButton Button { get { return m_Button; } }

    #endregion


    #region Support Method

    public void SetCurID(int i) { _curWeaponID = i; }

    public void SetWeaponUI(GameObject go, int currentID, bool setCurID = true)
    {
        go.GetComponent<LobbyUI_Weapon>().SetWeaponUI(currentID);
        if (setCurID) _curWeaponID = currentID;
    }

    public void SetCoin(int coin) { m_coin.text = coin.ToString(); }
    
    #endregion

}
