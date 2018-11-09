using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerWeapon : MonoBehaviour {

    [SerializeField] string m_PlayerID;
    [SerializeField] int m_Rank;
    [SerializeField] Text m_tPlayerName;
    [SerializeField] Text m_tRank;

    private string m_path = "Canvas/Info_Interface/";

    // Use this for initialization
    void Start () {
        m_tPlayerName.text = m_PlayerID;
        m_tRank.text = m_Rank.ToString(); 
        InitialWeapon("PlayerMenu/PrimaryWeapon", 1101);
        InitialWeapon("PlayerMenu/SecondaryWeapon", 1301);
	}

    private void InitialWeapon(string s, int i)
    {
        GameObject go = Instantiate(Resources.Load("Lobby/WeaponUI"), GameObject.Find(m_path + s).transform) as GameObject;
        go.GetComponent<UI_Weapon>().m_ID = i;
        go.GetComponent<UI_Weapon>().SetWeaponUI();
        go.GetComponent<Button>().onClick.AddListener(() => SelectWeaponUI(true));

    }
    
    private void SelectWeaponUI(bool b)
    {
        string s = "Select_Weapon";
        GameObject go = GameObject.Find(m_path + s)?? GameObject.Find(m_path + s + "(Clone)");
        if (go == null)
        {
            go = Instantiate(Resources.Load("Lobby/" + s), GameObject.Find(m_path).transform) as GameObject;
        }
        go.SetActive(b);
    }
}
