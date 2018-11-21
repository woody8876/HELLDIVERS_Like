using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerWeapon : MonoBehaviour {

    #region SerializeField
    [Header("== UI Setting ==")]
        [SerializeField] ControlEvent m_Control;
        [SerializeField] Text m_tPlayerName;
        [SerializeField] Text m_tRank;
        [SerializeField] Text m_tConfirm;
        [SerializeField] GameObject m_WeaponUI;
        [SerializeField] GameObject m_StratagemeUI;
        [SerializeField] GameObject m_SelectWeapon;
        [SerializeField] Image m_Confirm;
        [SerializeField] GameObject m_PrimaryWeapon;
        [SerializeField] GameObject m_SecondaryWeapon;
        [SerializeField] GameObject m_Stratageme;
    #endregion

    #region Public Field
    public int PlayerID { get { return m_Control.PlayerID; } }
    public int PriWeaponID { private set; get; }
    public int SecWeaponID { private set; get; }
    public ControlEvent Control { get { return m_Control; } }
    public bool m_bPrimary;
    public GameObject m_primary;
    public GameObject m_secondary;

    #endregion

    #region Private Field
    private Color m_Ready = new Color(1, 1, 1, 1);
    private Color m_HighLight = new Color(0.788f, 0.635f, 0.133f, 1.0f);
    private Color m_BGColor = new Color(1, 1, 1, 0.286f);
    private GameObject m_CurrentObject;
    private GameObject m_UpObject;
    private GameObject m_DownObject;
    private string m_path = "Canvas/ARMOURY/Player";

    #endregion

    #region MonoBehaviors

    void Start () {
        m_path += PlayerID +"/";
        SetPlayer(PlayerID);
        m_CurrentObject = m_Confirm.gameObject;
        OnSelect();
        SubsrtiptAxisEvent();
    }
    private void OnEnable()
    {
        if (m_CurrentObject == null) return;
        SubsrtiptAxisEvent();
    }

    private void OnDisable()
    {
        UnsubscribeAxisEvent();
    }
    #endregion

    #region Init Function

    private void SetPlayer(int player)
    {
        m_tPlayerName.text = PlayerManager.Instance.Players[player].info.Username;
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
        uI.SetID(PlayerManager.Instance.Players[player].info.Weapons[i]);
        uI.SetWeaponUI();
        if (b)  PriWeaponID = uI.ID;
        else SecWeaponID = uI.ID;
    }

    private void InitialStratagems(string s, int player, int i, bool b = false)
    {
        GameObject go = Instantiate(m_StratagemeUI, GameObject.Find(m_path + s).transform) as GameObject;
        LobbyUI_Stratagems uI = go.GetComponent<LobbyUI_Stratagems>();
        go.name = PlayerManager.Instance.Players[player].info.Stratagems[i].ToString();
        uI.m_ID = PlayerManager.Instance.Players[player].info.Stratagems[i];
        //main_Armoury.m_iStrategems.Add(i);
         uI.SetStratagemUI(b);
       
    }

    #endregion

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
    #endregion

    #region Confirm Event

    private void OnConfirm()
    {
        SetColor(ref m_Confirm, m_HighLight);
    }

    private void DisConfirm()
    {
        SetColor(ref m_Confirm, m_BGColor);
    }

    private void CancelConfirm()
    {
        m_tConfirm.text = "CORFIRM";
        SetColor(ref m_Confirm, m_HighLight);
        this.GetComponentInParent<PlayArmoury>().SetPlayerState(PlayerID, false);
    }

    private void SubmitConfirm()
    {
        PlayerManager.Instance.RefreshEquipment(PlayerID, WeaponList());
        m_tConfirm.text = "READY";
        SetColor(ref m_Confirm, m_Ready);
        this.GetComponentInParent<PlayArmoury>().SetPlayerState(PlayerID);
    }
    
    #endregion

    private List<int> WeaponList()
    {
        List<int> pList = new List<int>();
        pList.Add(PriWeaponID);
        pList.Add(SecWeaponID);
        return pList;
    }

    private void SetColor(ref Image target, Color color) { target.color = color; }

    private void SetObject(GameObject up, GameObject down)
    {
        m_UpObject = up;
        m_DownObject = down;
    }

    private void SubsrtiptAxisEvent()
    {
        Control.AxisUp += ButtonUp;
        Control.AxisDown += ButtonDown;
        Control.AxisSubmit += ButtonSumbit;
    }

    private void UnsubscribeAxisEvent()
    {
        Control.AxisUp -= ButtonUp;
        Control.AxisDown -= ButtonDown;
        Control.AxisSubmit -= ButtonSumbit;
    }

    #region Button Support Function

    private void OnSelect()
    {
        if (m_CurrentObject == m_PrimaryWeapon)
        {
            SetObject(m_PrimaryWeapon, m_SecondaryWeapon);
            m_primary.GetComponent<LobbyUI_Weapon>().SetHighlightBG();
        }
        else if (m_CurrentObject == m_SecondaryWeapon)
        {
            SetObject(m_PrimaryWeapon, m_Stratageme);
            m_secondary.GetComponent<LobbyUI_Weapon>().SetHighlightBG();
        }
        else if(m_CurrentObject == m_Stratageme)
        {
            SetObject(m_SecondaryWeapon, m_Confirm.gameObject);

        }
        else if (m_CurrentObject == m_Confirm.gameObject)
        {
            SetObject(m_Stratageme, m_Confirm.gameObject);
            OnConfirm();
        }
    }

    private void DisSelect()
    {
        if (m_CurrentObject == m_PrimaryWeapon)
        {
            m_primary.GetComponent<LobbyUI_Weapon>().SetBG();
        }
        else if (m_CurrentObject == m_SecondaryWeapon)
        {
            m_secondary.GetComponent<LobbyUI_Weapon>().SetBG();
        }
        else if (m_CurrentObject == m_Stratageme)
        {

        }
        else if (m_CurrentObject == m_Confirm.gameObject)
        {
            DisConfirm();
        }

    }

    private void OnClick()
    {
        if (m_CurrentObject == m_PrimaryWeapon)
        {
            SelectWeaponUI(true);
            m_bPrimary = true;
        }
        else if (m_CurrentObject == m_SecondaryWeapon)
        {
            SelectWeaponUI(true);
            m_bPrimary = false;
        }
        else if(m_CurrentObject == m_Stratageme)
        {

        }
        else if (m_CurrentObject == m_Confirm.gameObject)
        {
            SubmitConfirm();
        }
    }
    #endregion

    #region Button Behaviors

    private void ButtonUp()
    {
        DisSelect();
        m_CurrentObject = m_UpObject;
        OnSelect();
    }

    private void ButtonDown()
    {
        DisSelect();
        m_CurrentObject = m_DownObject;
        OnSelect();
    }

    private void ButtonSumbit()
    {
        OnClick(); ;
    } 
    #endregion
}
