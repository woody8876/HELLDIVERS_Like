using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponList : MonoBehaviour {

    #region SerializeField Field
    [Header("== Current Weapon ID ==")]
        [SerializeField] private int m_CurrentID;
    [Header("== Script Setting ==")] 
        [SerializeField] SetPlayerWeapon m_SetPlayer;
    [Header("== GameObject Setting ==")]  
        [SerializeField] GameObject m_WeaponUI;
        [SerializeField] UI_WeaponInfo m_Info;
    [Header("== Button Setting ==")]  
        [SerializeField] Button m_LevelUP;
        [SerializeField] Button m_Select;
        [SerializeField] RectTransform rect;
    #endregion

    List<Button> m_Buttons = new List<Button>();
    int[] Keys
    {
        get
        {
            int[] i = new int[GameData.Instance.WeaponInfoTable.Count];
            GameData.Instance.WeaponInfoTable.Keys.CopyTo(i, 0);
            return i;
        }
    }
    int _posY;
    int posY
    {
        get { return _posY; }
        set
        {
            if (value < 0) _posY = 0;
            else if (value > 852) _posY = 852;
            else _posY = value;
        }
    }

    #region Monobehaviors
    void Start () {
        CreateWeaponUI();
        SetNav();
        m_Select.onClick.AddListener(() => ClickSelect());
    }
    private void OnEnable()
    {
        StartCoroutine(SelectContinueButton());
    }
    private void LateUpdate()
    {
        Vector3 pos = rect.localPosition ;
        pos.y = posY;
        rect.localPosition = Vector3.Lerp(rect.localPosition, pos, 0.05f);
    }
    #endregion

    #region WeaponUI - Private Method
    //Set the init highlight button when SelectWeapon is active
    private IEnumerator SelectContinueButton()
    {
        yield return null;
        for (int i = 0; i < m_Buttons.Count; i++)
        {
            if (m_SetPlayer.m_iWeaponID == m_Buttons[i].gameObject.GetComponent<LobbyUI_Weapon>().m_ID)
            {
                EventSystem.current.SetSelectedGameObject(m_Buttons[i].gameObject, null);
            }
        }
    }
    //Create all the weapons in the dictionary at the first time
    private void CreateWeaponUI()
    {
        for (int i = 0; i < GameData.Instance.WeaponInfoTable.Count; i++)
        {
            if (Keys[i] == 1901) { return; }
            GameObject go = Instantiate(m_WeaponUI, this.transform);
            go.name = Keys[i].ToString();
            LobbyUI_Weapon uI_Weapon = go.GetComponent<LobbyUI_Weapon>();
            uI_Weapon.m_ID = Keys[i];
            go.GetComponent<Button>();
            uI_Weapon.SetWeaponUI();
            m_Buttons.Add(go.GetComponent<Button>());
            AddEvent(go, uI_Weapon);
        }
    }
    //Add EventTrigger into each WeaponUI to Update the WeaponAttribute UI
    private void AddEvent(GameObject go, LobbyUI_Weapon uI_Weapon)
    {
        EventTrigger trigger = go.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((eventData) => m_Info.SetWeapon(uI_Weapon));
        entry.callback.AddListener((eventData) => SetID(uI_Weapon));
        entry.callback.AddListener((eventData) => OnValueChange());
        trigger.triggers.Add(entry);
    }
    //Set Navegation of WeaponUI buttons at the first time
    private void SetNav()
    {
        Navigation buttonNav;
        for (int i = 0; i < m_Buttons.Count; i++)
        {
            buttonNav = m_Buttons[i].navigation;
            buttonNav.mode = Navigation.Mode.Explicit;
            buttonNav.selectOnUp = (i != 0) ? m_Buttons[i - 1] : m_Buttons[i];
            buttonNav.selectOnDown = (i != m_Buttons.Count - 1) ? m_Buttons[i + 1] : m_Buttons[i];
            buttonNav.selectOnRight = m_LevelUP;
            m_Buttons[i].navigation = buttonNav;
        }
    }
    //EventTrigger function of each WeaponUI, everytime when WeaponUI was highlighted would change the current ID in this script.
    private void SetID(LobbyUI_Weapon uI_Weapon)
    {
        m_CurrentID = uI_Weapon.m_ID;
    }
    //EventTrigger function of each WeaponUI, everytime when WeaponUI was highlighted would change the rect positoin
    private void OnValueChange()
    {
        posY = -150 + 150 * (int)((m_CurrentID - 1000) * 0.01f);
    }
    
    #endregion

    #region Select Button - Private Method
    //Check and change the ID and sprite of the weapon in the PlayerMenu 
    private void ClickSelect()
    {
        GameObject go = null;
        int i = 0;
        if (m_SetPlayer.m_bPrimary)
        {
            if (m_CurrentID == PlayerManager.Instance.Players[0].Weapons[0])
            {
                go = m_SetPlayer.SecondaryWeapon.GetComponentInChildren<Button>().gameObject;
                SetWeaponUI(go, PlayerManager.Instance.Players[0].Weapons[1], ref i);
                PlayerManager.Instance.Players[0].Weapons[0] = i;
            }
            go = m_SetPlayer.PrimaryWeapon.GetComponentInChildren<Button>().gameObject;
            SetWeaponUI(go, m_CurrentID, ref i);
            PlayerManager.Instance.Players[0].Weapons[1] = i;
        }
        else
        {
            if (m_CurrentID == PlayerManager.Instance.Players[0].Weapons[1])
            {
                go = m_SetPlayer.PrimaryWeapon.GetComponentInChildren<Button>().gameObject;
                SetWeaponUI(go, PlayerManager.Instance.Players[0].Weapons[0], ref i);
                PlayerManager.Instance.Players[0].Weapons[1] = i;
            }
            go = m_SetPlayer.SecondaryWeapon.GetComponentInChildren<Button>().gameObject;
            SetWeaponUI(go, m_CurrentID, ref i);
            PlayerManager.Instance.Players[0].Weapons[0] = i;
        }
        EventSystem.current.SetSelectedGameObject(go, null);
        m_SetPlayer.SelectWeaponUI(false);

    }
    //Use to set selected weapon to PlayerMenu in this script
    private void SetWeaponUI(GameObject go, int current, ref int target)
    {
        go.GetComponent<LobbyUI_Weapon>().m_ID = current;
        go.GetComponent<LobbyUI_Weapon>().SetWeaponUI();
        target = current;
    }
    #endregion
}
