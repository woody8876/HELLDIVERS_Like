using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerWeapon : MonoBehaviour {

    [Header("== UI Setting ==")]
        [SerializeField] Text m_tPlayerName;
        [SerializeField] Text m_tRank;
        [SerializeField] GameObject m_WeaponUI;
        [SerializeField] GameObject m_Stratagemes;
        [SerializeField] GameObject m_SelectWeapon;
        [SerializeField] Button m_Confirm;
    [Header("== Lobby Weapon UI Data ==")]
        [SerializeField] int _iWeaponID;
        

    #region Field
    public UIMain_Armoury main_Armoury = new UIMain_Armoury();
    public int m_iWeaponID { get { return _iWeaponID; } }
    private string m_path = "Canvas/ARMOURY/Info_Interface/";
    #endregion

    void Start () {

        m_tPlayerName.text = main_Armoury.m_sPlayerID;
        m_tRank.text = main_Armoury.m_iRank.ToString(); 
        InitialWeapon("PlayerMenu/PrimaryWeapon", 1101, true);
        InitialWeapon("PlayerMenu/SecondaryWeapon", 1301, false);
        InitialStratagems("PlayerMenu/Stratagems/", 2001);
        InitialStratagems("PlayerMenu/Stratagems/", 2002);
        InitialStratagems("PlayerMenu/Stratagems/", 2003);
        InitialStratagems("PlayerMenu/Stratagems/", 2002, true);
	}
    private void InitialWeapon(string s, int i, bool b)
    {
        GameObject go = Instantiate(m_WeaponUI, GameObject.Find(m_path + s).transform) as GameObject;
        LobbyUI_Weapon uI = go.GetComponent<LobbyUI_Weapon>();
        uI.m_ID = i;
        uI.SetWeaponUI();
        uI.m_Primary = b;
        if (b) { main_Armoury.m_iPrimaryWeaponID = i; }
        else { main_Armoury.m_iSecondaryWeaponID = i; }
        go.GetComponent<Button>().onClick.AddListener(() => SelectWeaponUI(true));
        go.GetComponent<Button>().onClick.AddListener(() => Click(uI));

    }
    private void InitialStratagems(string s, int i, bool b = false)
    {
        GameObject go = Instantiate(m_Stratagemes, GameObject.Find(m_path + s).transform) as GameObject;
        go.name = main_Armoury.m_iStrategems.Count.ToString();
        LobbyUI_Stratagems uI = go.GetComponent<LobbyUI_Stratagems>();
        uI.m_ID = i;
        main_Armoury.m_iStrategems.Add(i);
        uI.SetStratagemUI(b);
       
    }
    #region Weapon Click Event
    public void SelectWeaponUI(bool b)
    {
        string s = "Select_Weapon";
        GameObject go = GameObject.Find(m_path + s)?? GameObject.Find(m_path + s + "(Clone)");
        if (!go) { go = Instantiate(m_SelectWeapon, GameObject.Find(m_path).transform) as GameObject; }
        go.SetActive(b);
    }
    private void Click(LobbyUI_Weapon ui)
    {
        main_Armoury.m_bPrimary = ui.m_Primary;
        _iWeaponID = ui.m_ID; 
        
    }
    #endregion


    #region Confirm Click Event
    #endregion

}
