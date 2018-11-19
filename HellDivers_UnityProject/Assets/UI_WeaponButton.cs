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
        m_LevelUp.onClick.AddListener(() => ClickLevelUp(1));
        SetNav(m_LevelUp, m_Select, false);
        SetNav(m_Select, m_LevelUp, true);

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

    private void ClickLevelUp(int player)
    {
        int type = GameData.Instance.WeaponInfoTable[weaponDisplay.curWeaponID].Type;
        List<int> pList = weaponDisplay.Info.weapons[type];
        if (pList[pList.Count - 1] != weaponDisplay.curWeaponID)
        {
            int i = weaponDisplay.curWeaponID;
            PlayerManager.Instance.Players[player].LevelUpWeapon(ref i);
            weaponDisplay.ChangeCurID(i);
            weaponDisplay.Info.SetWeapon(type, weaponDisplay.curWeaponID);
            if (weaponDisplay.curWeaponID == pList[pList.Count - 1])
            {
                m_LevelUp.interactable = false;
                EventSystem.current.SetSelectedGameObject(Select.gameObject);
            }
        }
    }

    private void ClickSelect(int player)
    {
        GameObject go = null;
        int i = 0;
        if (weaponDisplay.SetPlayer.m_bPrimary)
        {
            if (weaponDisplay.curWeaponID == PlayerManager.Instance.Players[player].Weapons[0])
            {
                go = weaponDisplay.SetPlayer.m_secondary;
                SetWeaponUI(go, PlayerManager.Instance.Players[player].Weapons[1], ref i);
                PlayerManager.Instance.Players[player].Weapons[0] = i;
            }
            go = weaponDisplay.SetPlayer.m_primary;
            SetWeaponUI(go, weaponDisplay.curWeaponID, ref i);
            PlayerManager.Instance.Players[player].Weapons[1] = i;
        }
        else
        {
            if (weaponDisplay.curWeaponID == PlayerManager.Instance.Players[player].Weapons[1])
            {
                go = weaponDisplay.SetPlayer.m_primary;
                SetWeaponUI(go, PlayerManager.Instance.Players[player].Weapons[0], ref i);
                PlayerManager.Instance.Players[player].Weapons[1] = i;
            }
            go = weaponDisplay.SetPlayer.m_secondary;
            SetWeaponUI(go, weaponDisplay.curWeaponID, ref i);
            PlayerManager.Instance.Players[player].Weapons[0] = i;
        }
        weaponDisplay.SetPlayer.SelectWeaponUI(false);
        EventSystem.current.SetSelectedGameObject(go);

    }

    private void SetWeaponUI(GameObject go, int current, ref int target)
    {
        go.GetComponent<LobbyUI_Weapon>().m_ID = current;
        go.GetComponent<LobbyUI_Weapon>().SetWeaponUI();
        target = current;
    }

    #endregion

}
