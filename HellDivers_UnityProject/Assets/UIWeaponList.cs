using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIWeaponList : MonoBehaviour {

    [SerializeField] UIWeaponDisplay weaponDisplay;
    [SerializeField] GameObject m_WeaponUI;
    [SerializeField] GameObject m_Content;

    List<Button> m_weapons = new List<Button>();
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
    int _posY;
    int m_curID;
    // Use this for initialization
    void Start()
    {
        CreateWeaponUI();
        SetButtonNav();
    }
    private void OnEnable()
    {
        StartCoroutine(ChangeWeapon());
    }


    private void LateUpdate()
    {
        Vector3 pos = m_Content.transform.localPosition;
        pos.y = posY;
        m_Content.transform.localPosition = Vector3.Lerp(m_Content.transform.localPosition, pos, 0.05f);
    }

    private IEnumerator ChangeWeapon()
    {
        yield return null;
            for (int i = 0; i < m_weapons.Count; i++)
            {
                if (weaponDisplay.SetPlayer.PriWeaponID.ToString() == m_weapons[i].gameObject.name)
                {
                    EventSystem.current.SetSelectedGameObject(m_weapons[i].gameObject, null);
                }
            }
    }

    private void CreateWeaponUI()
    {
        GameObject go;
        Button btn;
        List<int> unlockWeapons = PlayerManager.Instance.Players[1].UnLockWeapons;
        for (int i = 0; i < unlockWeapons.Count; i++)
        {
            go = Instantiate(m_WeaponUI, m_Content.transform);
            btn = go.GetComponent<Button>();
            int id = unlockWeapons[i];
            go.name = id.ToString();
            btn.onClick.AddListener(() => EventSystem.current.SetSelectedGameObject(weaponDisplay.SelectButton));
            SetWeaponUI(go, id, ref m_curID);
            AddEvent(go, id);
            m_weapons.Add(btn);
        }
    }

    private void AddEvent(GameObject go, int i)
    {
        int type = GameData.Instance.WeaponInfoTable[i].Type;
        EventTrigger trigger = go.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        //entry.eventID = EventTriggerType.;
        entry.callback.AddListener((eventData) => m_curID = i);
        entry.callback.AddListener((eventData) => weaponDisplay.Info.SetWeapon(type, i));
        entry.callback.AddListener((eventData) => OnValueChange(i));
        trigger.triggers.Add(entry);
    }

    private void OnValueChange(int i)
    {
        for (int j = 0; j < m_weapons.Count; j++)
        {
            if (m_weapons[j].name == i.ToString())
            {
                posY = -150 + 150 * j;
            }
        }
    }

    private void SetButtonNav()
    {
        Navigation buttonNav;
        for (int i = 0; i < m_weapons.Count; i++)
        {
            buttonNav = m_weapons[i].navigation;
            buttonNav.mode = Navigation.Mode.Explicit;
            buttonNav.selectOnUp = (i != 0) ? m_weapons[i - 1] : m_weapons[i];
            buttonNav.selectOnDown = (i != m_weapons.Count - 1) ? m_weapons[i + 1] : m_weapons[i];
            m_weapons[i].navigation = buttonNav;
        }
    }


    private void SetWeaponUI(GameObject go, int current, ref int target)
    {
        go.GetComponent<LobbyUI_Weapon>().m_ID = current;
        go.GetComponent<LobbyUI_Weapon>().SetWeaponUI();
        target = current;
    }

}
