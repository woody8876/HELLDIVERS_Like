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
    // Use this for initialization
    void Start()
    {
        CreateWeaponUI(weaponDisplay.SetPlayer.PlayerID);
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
            if (Determine() == m_weapons[i].gameObject.name)
            {
                EventSystem.current.SetSelectedGameObject(m_weapons[i].gameObject);
                yield break;
            }
        }
    }

    public void LevelUp(int id)
    {
        for (int i = 0; i < m_weapons.Count; i++)
        {
            if ((id-1).ToString() == m_weapons[i].gameObject.name)
            {
                m_weapons[i].gameObject.name = id.ToString();
                weaponDisplay.SetWeaponUI(m_weapons[i].gameObject, id);
                return;
            }
        }
    }

    private void CreateWeaponUI(int player)
    {
        GameObject go;
        Button btn;
        List<int> unlockWeapons = PlayerManager.Instance.Players[player].UnLockWeapons;
        for (int i = 0; i < unlockWeapons.Count; i++)
        {
            go = Instantiate(m_WeaponUI, m_Content.transform);
            btn = go.GetComponent<Button>();
            int id = unlockWeapons[i];
            go.name = id.ToString();
            btn.onClick.AddListener(() => OnClickEvent(go.name ,weaponDisplay.SelectButton));
            weaponDisplay.SetWeaponUI(go, id);
            OnSelectEvent(go);
            m_weapons.Add(btn);
        }
    }

    private string Determine()
    {
        if (weaponDisplay.SetPlayer.m_bPrimary) return weaponDisplay.SetPlayer.PriWeaponID.ToString();
        else return weaponDisplay.SetPlayer.SecWeaponID.ToString();
    }

    #region Set Button

    private void SetInfo(GameObject go)
    {
        int i = int.Parse(go.name);
        weaponDisplay.Info.SetID(i);
        weaponDisplay.Info.SetType(GameData.Instance.WeaponInfoTable[i].Type);
        weaponDisplay.Info.SetWeapon();
    }

    private void OnValueChange(GameObject go)
    {
        
        for (int j = 0; j < m_weapons.Count; j++)
        {
            if (m_weapons[j].name == go.name)
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

    private void OnSelectEvent(GameObject go)
    {
        EventTrigger trigger = go.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((eventData) => SetInfo(go));
        entry.callback.AddListener((eventData) => OnValueChange(go));
        trigger.triggers.Add(entry);
    }

    private void OnClickEvent(string s, GameObject next) 
    {
        weaponDisplay.SetCurID(weaponDisplay.Info.ID);
        EventSystem.current.SetSelectedGameObject(next);
    }

    #endregion
}
