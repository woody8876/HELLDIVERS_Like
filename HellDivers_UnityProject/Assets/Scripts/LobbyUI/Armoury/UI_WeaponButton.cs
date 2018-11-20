using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_WeaponButton : MonoBehaviour {

    [SerializeField] UIWeaponDisplay weaponDisplay;
    [SerializeField] Button m_LevelUp;
    [SerializeField] Button m_Select;
    
    public GameObject LevelUp { get { return m_LevelUp.gameObject; } }
    public GameObject Select  { get { return m_Select.gameObject; } }

    // Use this for initialization
    void Start () {
        m_Select.onClick.AddListener(() => ClickSelect(weaponDisplay.SetPlayer.PlayerID));
        m_LevelUp.onClick.AddListener(() => ClickLevelUp(weaponDisplay.SetPlayer.PlayerID));
        SetNav(m_LevelUp, m_Select, false);
        SetNav(m_Select, m_LevelUp, true);
        SelectEvent();
    }

    // Update is called once per frame
    void Update () {
		
	}
    #region Button Method
    private void SetNav(Button a, Button b, bool left)
    {
        Navigation buttonNav;
        buttonNav = a.navigation;
        buttonNav.mode = Navigation.Mode.Explicit;
        if (left) { buttonNav.selectOnLeft = b; }
        else { buttonNav.selectOnRight = b; }
        a.navigation = buttonNav;
    }

    private void SelectEvent()
    {
        EventTrigger trigger = Select.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((eventdata) => m_LevelUp.interactable = CheckLevelUp());
        trigger.triggers.Add(entry);
    }

    private bool CheckLevelUp()
    {
        return true;
    }

    private void ClickLevelUp(int player)
    {
        int type = GameData.Instance.WeaponInfoTable[weaponDisplay.CurWeaponID].Type;
        List<int> pList = weaponDisplay.Info.weapons[type];
        if (pList[pList.Count - 1] != weaponDisplay.CurWeaponID)
        {
            int i = weaponDisplay.CurWeaponID;
            PlayerManager.Instance.Players[player].info.LevelUpWeapon(ref i);
            weaponDisplay.SetCurID(i);
            weaponDisplay.WeaponList.LevelUp(i);
            weaponDisplay.Info.SetID(i);
            weaponDisplay.Info.SetType(type);
            weaponDisplay.Info.SetWeapon();
            if (weaponDisplay.CurWeaponID == pList[pList.Count - 1])
            {
                m_LevelUp.interactable = false;
                EventSystem.current.SetSelectedGameObject(Select.gameObject);
            }   
        }
    }

    private void ClickSelect(int player)
    {
        GameObject go = null;
        SetPlayerWeapon setPlayer = weaponDisplay.SetPlayer;
        if (setPlayer.m_bPrimary)
        {
            if (GameData.Instance.WeaponInfoTable[weaponDisplay.CurWeaponID].Type == GameData.Instance.WeaponInfoTable[setPlayer.SecWeaponID].Type)
            {
                go = setPlayer.m_secondary;
                weaponDisplay.SetWeaponUI(go, setPlayer.PriWeaponID, false);
                setPlayer.SetSecWeaponID(setPlayer.PriWeaponID);
            }
            go = setPlayer.m_primary;
            weaponDisplay.SetWeaponUI(go, weaponDisplay.CurWeaponID);
            setPlayer.SetPriWeaponID(weaponDisplay.CurWeaponID);
        }
        else
        {
            if (GameData.Instance.WeaponInfoTable[weaponDisplay.CurWeaponID].Type == GameData.Instance.WeaponInfoTable[setPlayer.PriWeaponID].Type)
            {
                go = setPlayer.m_primary;
                weaponDisplay.SetWeaponUI(go, setPlayer.SecWeaponID, false);
                setPlayer.SetPriWeaponID(setPlayer.SecWeaponID);
            }
            go = setPlayer.m_secondary;
            weaponDisplay.SetWeaponUI(go, weaponDisplay.CurWeaponID);
            setPlayer.SetSecWeaponID(weaponDisplay.CurWeaponID);
        }
        setPlayer.SelectWeaponUI(false);
        EventSystem.current.SetSelectedGameObject(go);

    }

    #endregion

}
