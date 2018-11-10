using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponList : MonoBehaviour {

    public int m_ID;
    [SerializeField] SetPlayerWeapon m_SetPlayer;
    [SerializeField] GameObject m_WeaponUI;
    [SerializeField] GameObject m_PrimaryWeapon;
    [SerializeField] GameObject m_SecondaryWeapon;
    [SerializeField] Button m_LevelUP;
    [SerializeField] Button m_Select;
    

    List<Button> m_Buttons = new List<Button>();
    GameObject m_First;
    int[] Keys
    {
        get
        {
            int[] i = new int[GameData.Instance.WeaponInfoTable.Count];
            GameData.Instance.WeaponInfoTable.Keys.CopyTo(i, 0);
            return i;
        }
    }


    // Use this for initialization
    void Start () {
        CreateWeaponUI();
        SetNav();
        m_Select.onClick.AddListener(() => ClickSelect());
    }
    private void OnEnable()
    {
        StartCoroutine(SelectContinueButton());
    }
    private IEnumerator SelectContinueButton()
    {
        yield return null;
        for (int i = 0; i < m_Buttons.Count; i++)
        {
            if (m_SetPlayer.m_WeaponID == m_Buttons[i].gameObject.GetComponent<UI_Weapon>().m_ID)
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
            uI_Weapon.SetWeaponUI();
            m_Buttons.Add(go.GetComponent<Button>());
            if (i == 0) m_First = go;
        }
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

    public void ClickSelect()
    {
        GameObject go = (m_SetPlayer.m_bPrimary) ? m_PrimaryWeapon : m_SecondaryWeapon;
        EventSystem.current.SetSelectedGameObject(go.GetComponentInChildren<Button>().gameObject, null);
        m_SetPlayer.SelectWeaponUI(false);
    }
}
