using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerWeapon : MonoBehaviour {
    [Header("== UI Setting ==")]
        [SerializeField] Text m_tPlayerName;
        [SerializeField] Text m_tRank;
        [SerializeField] GameObject m_WeaponUI;
        [SerializeField] GameObject m_SelectWeapon;
    [Header("== Lobby Weapon UI Data ==")]
        [SerializeField] int _iWeaponID;
    public UIMain_Weapon main_Weapon = new UIMain_Weapon();

    public int m_iWeaponID { get { return _iWeaponID; } }
    private string m_path = "Canvas/Info_Interface/";

    // Use this for initialization
    void Start () {

        m_tPlayerName.text = main_Weapon.m_sPlayerID;
        m_tRank.text = main_Weapon.m_iRank.ToString(); 
        InitialWeapon("PlayerMenu/PrimaryWeapon", 1101, true);
        InitialWeapon("PlayerMenu/SecondaryWeapon", 1301, false);
	}

    private void InitialWeapon(string s, int i, bool b)
    {
        GameObject go = Instantiate(m_WeaponUI, GameObject.Find(m_path + s).transform) as GameObject;
        UI_Weapon uI = go.GetComponent<UI_Weapon>();
        uI.m_ID = i;
        uI.SetWeaponUI();
        uI.m_Primary = b;
        if (b) { main_Weapon.m_iPrimaryWeaponID = i; }
        else { main_Weapon.m_iSecondaryWeaponID = i; }
        go.GetComponent<Button>().onClick.AddListener(() => SelectWeaponUI(true));
        go.GetComponent<Button>().onClick.AddListener(() => Click(uI));

    }

    public void SelectWeaponUI(bool b)
    {
        string s = "Select_Weapon";
        GameObject go = GameObject.Find(m_path + s)?? GameObject.Find(m_path + s + "(Clone)");
        if (!go) { go = Instantiate(m_SelectWeapon, GameObject.Find(m_path).transform) as GameObject; }
        go.SetActive(b);
    }

    private void Click(UI_Weapon ui)
    {
        main_Weapon.m_bPrimary = ui.m_Primary;
        _iWeaponID = ui.m_ID; 
        
    }
}
