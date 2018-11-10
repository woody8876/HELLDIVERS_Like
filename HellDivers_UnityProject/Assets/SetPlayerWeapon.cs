using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerWeapon : MonoBehaviour {

    [SerializeField] string m_PlayerID;
    [SerializeField] int m_Rank;
    [SerializeField] Text m_tPlayerName;
    [SerializeField] Text m_tRank;
    [SerializeField] GameObject m_WeaponUI;
    [SerializeField] GameObject m_SelectWeapon;

    public bool m_bPrimary;
    public int m_WeaponID;

    private string m_path = "Canvas/Info_Interface/";

    // Use this for initialization
    void Start () {
        m_tPlayerName.text = m_PlayerID;
        m_tRank.text = m_Rank.ToString(); 
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
        go.GetComponent<Button>().onClick.AddListener(() => SelectWeaponUI(true));
        go.GetComponent<Button>().onClick.AddListener(() => SelectButton(uI));

    }

    public void SelectWeaponUI(bool b)
    {
        string s = "Select_Weapon";
        GameObject go = GameObject.Find(m_path + s)?? GameObject.Find(m_path + s + "(Clone)");
        if (!go) { go = Instantiate(m_SelectWeapon, GameObject.Find(m_path).transform) as GameObject; }
        go.SetActive(b);
    }

    private void SelectButton(UI_Weapon ui)
    {
        m_bPrimary = ui.m_Primary;
        m_WeaponID = ui.m_ID;
    }

}
