using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetPlayerWeapon : MonoBehaviour {

    [Header("== UI Setting ==")]
        [SerializeField] Text m_tPlayerName;
        [SerializeField] Text m_tRank;
        [SerializeField] GameObject m_WeaponUI;
        [SerializeField] GameObject m_Stratagemes;
        [SerializeField] GameObject m_SelectWeapon;
        public GameObject PrimaryWeapon;
        public GameObject SecondaryWeapon;
        [SerializeField] Button m_Confirm;
    [Header("== Lobby Weapon UI Data ==")]


    #region Field
    public int PlayerID;
    public int PriWeaponID { private set; get; }
    public int SecWeaponID { private set; get; }
    public bool m_bPrimary;
    private string m_path = "Canvas/ARMOURY/Player";
    public GameObject m_primary;
    public GameObject m_secondary;
    #endregion

    void Start () {
        m_path += PlayerID +"/";
        m_Confirm.onClick.AddListener(() => Confirm());
        SetPlayer(PlayerID);
        EventSystem.current.SetSelectedGameObject(m_Confirm.gameObject); 
    }


    private void SetPlayer(int player)
    {
        m_tPlayerName.text = PlayerManager.Instance.Players[player].Username;
        m_tRank.text = "1";
        InitialWeapon(ref m_primary, "PlayerMenu/PrimaryWeapon", player, 0, true);
        InitialWeapon(ref m_secondary, "PlayerMenu/SecondaryWeapon", player, 1, false);
        InitialStratagems("PlayerMenu/Stratagems/", player, 0);
        InitialStratagems("PlayerMenu/Stratagems/", player, 1);

    }

    private void InitialWeapon(ref GameObject go,string s, int player, int i, bool b)
    {
        go = Instantiate(m_WeaponUI, GameObject.Find(m_path + s).transform) as GameObject;
        LobbyUI_Weapon uI = go.GetComponent<LobbyUI_Weapon>();
        uI.SetID(PlayerManager.Instance.Players[player].Weapons[i]);
        uI.SetWeaponUI();
        uI.SetPrimary(b);
        if (b)  PriWeaponID = uI.ID;
        else SecWeaponID = uI.ID;
        go.GetComponent<Button>().onClick.AddListener(() => SelectWeaponUI(true));
        go.GetComponent<Button>().onClick.AddListener(() => Click(uI));

    }

    private void InitialStratagems(string s, int player, int i, bool b = false)
    {
        GameObject go = Instantiate(m_Stratagemes, GameObject.Find(m_path + s).transform) as GameObject;
        LobbyUI_Stratagems uI = go.GetComponent<LobbyUI_Stratagems>();
        go.name = PlayerManager.Instance.Players[player].Stratagems[i].ToString();
        uI.m_ID = PlayerManager.Instance.Players[player].Stratagems[i];
        //main_Armoury.m_iStrategems.Add(i);
         uI.SetStratagemUI(b);
       
    }

    #region Public Method
    public void SelectWeaponUI(bool b)
    {
        string s = "Select_Weapon_SinglePlayer";
        GameObject go = GameObject.Find(m_path + s)?? GameObject.Find(m_path + s + "(Clone)");
        if (!go) { go = Instantiate(m_SelectWeapon, this.transform.parent) as GameObject; }
        go.SetActive(b);
        this.gameObject.SetActive(!b);
        if (b == false) { SetWeapon(); }
    }

    public void SetPriWeaponID(int i) { PriWeaponID = i; }

    public void SetSecWeaponID(int i) { SecWeaponID = i; }
    #endregion

    #region Weapon Click Event
    private void SetWeapon()
    {
        m_primary.GetComponent<LobbyUI_Weapon>().SetWeaponUI();
        m_secondary.GetComponent<LobbyUI_Weapon>().SetWeaponUI();
    }

    private void Click(LobbyUI_Weapon ui)
    {
        m_bPrimary = ui.Primary;
    }
    #endregion


    #region Confirm Click Event
    private List<int> WeaponList()
    {
        List<int> pList = new List<int>();
        pList.Add(PriWeaponID);
        pList.Add(SecWeaponID);
        return pList;
    }

    private void Confirm()
    {
        PlayerManager.Instance.RefreshEquipment(PlayerID, WeaponList());
        SceneController.Instance.ToGameScene();
    }
    #endregion

}
