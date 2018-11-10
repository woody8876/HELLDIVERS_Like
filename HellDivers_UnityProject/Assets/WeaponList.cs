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
    }

    #endregion
    
    #region WeaponUI - Private Method
    private IEnumerator SelectContinueButton()
    {
        yield return null;
        for (int i = 0; i < m_Buttons.Count; i++)
        {
            if (m_SetPlayer.m_iWeaponID == m_Buttons[i].gameObject.GetComponent<UI_Weapon>().m_ID)
            {
                EventSystem.current.SetSelectedGameObject(m_Buttons[i].gameObject, null);
                Debug.Log("Find one!!");
            }
        }
    }
    private void CreateWeaponUI()
    {
        for (int i = 0; i < GameData.Instance.WeaponInfoTable.Count; i++)
        {
            if (Keys[i] == 1901) { return; }
            GameObject go = Instantiate(m_WeaponUI, this.transform);
            go.name = Keys[i].ToString();
            UI_Weapon uI_Weapon = go.GetComponent<UI_Weapon>();
            uI_Weapon.m_ID = Keys[i];
            go.GetComponent<Button>();
            uI_Weapon.SetWeaponUI();
            m_Buttons.Add(go.GetComponent<Button>());
            AddEvent(go, uI_Weapon);
        }
    }
    private void AddEvent(GameObject go, UI_Weapon uI_Weapon)
    {
        EventTrigger trigger = go.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((eventData) => m_Info.SetWeapon(uI_Weapon));
        entry.callback.AddListener((eventData) => SetID(uI_Weapon));
        trigger.triggers.Add(entry);

    }
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
    private void SetID(UI_Weapon uI_Weapon)
    {
        m_CurrentID = uI_Weapon.m_ID;
    }
    #endregion

    #region Select Button - Private Method
    private void ClickSelect()
    {
        GameObject go = null;
        if (m_SetPlayer.main_Weapon.m_bPrimary)
        {
            if (m_CurrentID == m_SetPlayer.main_Weapon.m_iSecondaryWeaponID)
            {
                go = m_SetPlayer.main_Weapon.SecondaryWeapon.GetComponentInChildren<Button>().gameObject;
                SetWeaponUI(go, m_SetPlayer.main_Weapon.m_iPrimaryWeaponID, ref m_SetPlayer.main_Weapon.m_iSecondaryWeaponID);
            }
            go = m_SetPlayer.main_Weapon.PrimaryWeapon.GetComponentInChildren<Button>().gameObject;
            SetWeaponUI(go, m_CurrentID, ref m_SetPlayer.main_Weapon.m_iPrimaryWeaponID);
        }
        else
        {
            if (m_CurrentID == m_SetPlayer.main_Weapon.m_iPrimaryWeaponID)
            {
                go = m_SetPlayer.main_Weapon.PrimaryWeapon.GetComponentInChildren<Button>().gameObject;
                SetWeaponUI(go, m_SetPlayer.main_Weapon.m_iSecondaryWeaponID, ref m_SetPlayer.main_Weapon.m_iPrimaryWeaponID);
            }
            go = m_SetPlayer.main_Weapon.SecondaryWeapon.GetComponentInChildren<Button>().gameObject;
            SetWeaponUI(go, m_CurrentID, ref m_SetPlayer.main_Weapon.m_iSecondaryWeaponID);

        }
        EventSystem.current.SetSelectedGameObject(go, null);
        m_SetPlayer.SelectWeaponUI(false);

    }


    private void SetWeaponUI(GameObject go, int current, ref int target)
    {
        go.GetComponent<UI_Weapon>().m_ID = current;
        go.GetComponent<UI_Weapon>().SetWeaponUI();
        target = current;
    }
    #endregion
}
