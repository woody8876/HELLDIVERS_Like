using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStratagemsDisplay : MonoBehaviour {

    #region SerializeField
    [Header("== Script Setting ==")]
    [SerializeField] SetPlayerWeapon m_SetPlayer;
    [SerializeField] UI_StratagemsInfo m_Info;
    [SerializeField] UIStratagemsList m_StratagemsList;
    [Header("== Private Field ==")]
    [SerializeField] int _curStratagemID;
    #endregion

    #region Public Getter
    public int CurStratagemID { get { return _curStratagemID; } }
    public SetPlayerWeapon SetPlayer { get { return m_SetPlayer; } }
    public UI_StratagemsInfo Info { get { return m_Info; } }
    public UIStratagemsList StratagemList { get { return m_StratagemsList; } }
    #endregion

    public void SetCurID (int i) { _curStratagemID = i; }
    public void SetStratagemUI(GameObject go, int currentID, bool setCurID = true)
    {
        go.GetComponent<LobbyUI_Stratagems>().SetStratagemUI(currentID);
        if (setCurID) _curStratagemID = currentID;
    }
}
