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
        [SerializeField] Image m_Confirm;
    [Header("== Prefabs Setting ==")]
        [SerializeField] GameObject m_WeaponUI;
        [SerializeField] GameObject m_StratagemeUI;
        [SerializeField] GameObject m_SelectWeapon;
        [SerializeField] GameObject m_SelectStratagem;
    [Header("== Set Panel  ==")]
        [SerializeField] GameObject m_PrimaryWeapon;
        [SerializeField] GameObject m_SecondaryWeapon;
        [SerializeField] GameObject m_Stratagem;
    #endregion

    #region Public Field
    public int PlayerID { get { return m_Control.PlayerID; } }
    public int PriWeaponID { private set; get; }
    public int SecWeaponID { private set; get; }
    public int CurStratagemPos { private set; get; }
    public ControlEvent Control { get { return m_Control; } }
    public bool m_bPrimary;
    public GameObject m_primary;
    public GameObject m_secondary;
    public GameObject[] Stratagems { get { return m_Stratagems; } }

    #endregion

    #region Private Field
    private Color m_Ready = new Color(1, 0.765f, 0, 1);
    private Color m_HighLight = new Color(0.788f, 0.635f, 0.133f, 1.0f);
    private Color m_BGColor = new Color(1, 1, 1, 0.286f);
    private GameObject m_CurrentObject;
    private GameObject m_UpObject;
    private GameObject m_DownObject;
    private string m_path = "Canvas/ARMOURY/Player";
    private GameObject[] m_Stratagems = new GameObject[4];
    #endregion

    #region MonoBehaviors

    void Start(){
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
        OnSelect();
    }

    private void OnDisable()
    {
        Control.AxisCancel -= ButtonCanael;
        UnsubscribeAxisEvent();
    }

    #endregion

    #region Init Function

    private void SetPlayer(int player)
    {
        m_tPlayerName.text = PlayerManager.Instance.Players[player].info.Username;
        m_tRank.text = "1";
        InitialWeapon(ref m_primary, "PlayerMenu/PrimaryWeapon", player, 1, true);
        InitialWeapon(ref m_secondary, "PlayerMenu/SecondaryWeapon", player, 0, false);
        InitialStratagems("PlayerMenu/Stratagems/", player, 0);
        InitialStratagems("PlayerMenu/Stratagems/", player, 1);
        InitialStratagems("PlayerMenu/Stratagems/", player, 2, true);
        InitialStratagems("PlayerMenu/Stratagems/", player, 3, true);
    }

    private void InitialWeapon(ref GameObject go,string s, int player, int serial, bool b)
    {
        go = Instantiate(m_WeaponUI, GameObject.Find(m_path + s).transform) as GameObject;
        LobbyUI_Weapon uI = go.GetComponent<LobbyUI_Weapon>();
        int id = PlayerManager.Instance.Players[player].info.Weapons[serial];
        uI.SetWeaponUI(id);
        if (b)  PriWeaponID = uI.ID;
        else SecWeaponID = uI.ID;
    }

    private void InitialStratagems(string s, int player, int serial, bool nothing = false , bool b = false)
    {
        GameObject go = Instantiate(m_StratagemeUI, GameObject.Find(m_path + s).transform) as GameObject;
        LobbyUI_Stratagems uI = go.GetComponent<LobbyUI_Stratagems>();
        int id =(!nothing)? PlayerManager.Instance.Players[player].info.Stratagems[serial] : 0 ;
        go.name = id.ToString();
        uI.SetStratagemUI(id, b);
        m_Stratagems[serial] = go;
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

    public void SelectStratagemUI(bool b, int id = 0)
    {
        string s = "Select_Stratagems_SinglePlayer";
        GameObject go = GameObject.Find(m_path + s) ?? GameObject.Find(m_path + s + "(Clone)");
        if (!go) { go = Instantiate(m_SelectStratagem, this.transform.parent) as GameObject; }
        go.SetActive(b);
        this.gameObject.SetActive(!b);
        if (b == false) { SetStratagems(id); }
    }

    public void SetPriWeaponID(int i) { PriWeaponID = i; }

    public void SetSecWeaponID(int i) { SecWeaponID = i; }
    
    #endregion

    #region Panel Click Event
    private void SetWeapon()
    {
        m_primary.GetComponent<LobbyUI_Weapon>().SetWeaponUI(PriWeaponID); 
        m_secondary.GetComponent<LobbyUI_Weapon>().SetWeaponUI(SecWeaponID);
    }
    private void SetStratagems(int id)
    {
        Stratagems[CurStratagemPos].GetComponent<LobbyUI_Stratagems>().SetStratagemUI(id);
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
        Control.AxisCancel -= ButtonCanael;
        SubsrtiptAxisEvent();
    }

    private void SubmitConfirm()
    {
        PlayerManager.Instance.RefreshEquipment(PlayerID, WeaponList());
        m_tConfirm.text = "READY";
        SetColor(ref m_Confirm, m_Ready);
        this.GetComponentInParent<PlayArmoury>().SetPlayerState(PlayerID);
        UnsubscribeAxisEvent();
    }

    #endregion

    #region Stratagems Method

    private void OnSelectStratagems()
    {
        m_Stratagems[CurStratagemPos].GetComponent<LobbyUI_Stratagems>().SetHighlightBG();
        Control.AxisLeft += NavLeft;
        Control.AxisRight += NavRight;
    }

    private void DisSelectStratagems()
    {
        m_Stratagems[CurStratagemPos].GetComponent<LobbyUI_Stratagems>().SetBG();
        Control.AxisLeft -= NavLeft;
        Control.AxisRight -= NavRight;
    }

    private void OnStratagems(bool right)
    {
        m_Stratagems[CurStratagemPos].GetComponent<LobbyUI_Stratagems>().SetBG();
        if (right) CurStratagemPos++;
        else CurStratagemPos--;
        m_Stratagems[CurStratagemPos].GetComponent<LobbyUI_Stratagems>().SetHighlightBG();
    }

    private void NavRight()
    {
        if (CurStratagemPos == 3) return;
        OnStratagems(true);
    }

    private void NavLeft()
    {
        if (CurStratagemPos == 0) return ;
        OnStratagems(false);
    }

    #endregion

    private List<int> WeaponList()
    {
        List<int> pList = new List<int>();
        pList.Add(PriWeaponID);
        pList.Add(SecWeaponID);
        return pList;
    }

    private void SubsrtiptAxisEvent()
    {
        Control.AxisUp += ButtonUp;
        Control.AxisDown += ButtonDown;
        Control.AxisSubmit += ButtonSumbit;
        Control.AxisCancel += ButtonCanael;
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
            SetObject(m_PrimaryWeapon, m_Stratagem);
            m_secondary.GetComponent<LobbyUI_Weapon>().SetHighlightBG();
        }
        else if(m_CurrentObject == m_Stratagem)
        {
            SetObject(m_SecondaryWeapon, m_Confirm.gameObject);
            OnSelectStratagems();
        }
        else if (m_CurrentObject == m_Confirm.gameObject)
        {
            SetObject(m_Stratagem, m_Confirm.gameObject);
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
        else if (m_CurrentObject == m_Stratagem)
        {
            DisSelectStratagems();
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
            m_bPrimary = true;
            SelectWeaponUI(true);
        }
        else if (m_CurrentObject == m_SecondaryWeapon)
        {
            m_bPrimary = false;
            SelectWeaponUI(true);
        }
        else if(m_CurrentObject == m_Stratagem)
        {
            SelectStratagemUI(true);
        }
        else if (m_CurrentObject == m_Confirm.gameObject)
        {
            SubmitConfirm();
        }
    }

    private void SetColor(ref Image target, Color color) { target.color = color; }

    private void SetObject(GameObject up, GameObject down)
    {
        m_UpObject = up;
        m_DownObject = down;
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
        DisSelect();
        OnClick();
    } 

    private void ButtonCanael()
    {
        DisSelect();
        CancelConfirm();
        m_CurrentObject = m_Confirm.gameObject;
        OnSelect();
    }
         
    #endregion
}
