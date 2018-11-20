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
    float m_Timer;
    float m_TCountDown = 0.2f;
    ControllerSetting controller;
    // Use this for initialization
    void Start()
    {
        CreateWeaponUI(weaponDisplay.SetPlayer.PlayerID);
        int PlayerID = GetComponentInParent<ControlEvent>().PlayerID;
        controller = PlayerManager.Instance.Players[PlayerID].controllerSetting;
    }
    private void OnEnable()
    {
        StartCoroutine(ChangeWeapon());
    }


    private void LateUpdate()
    {
        if (m_Timer < 0)
        {
            if (Input.GetAxis(controller.Vertical) > 0)
            {
                ButtonNavUP();
                m_Timer = m_TCountDown;
            }
            if (Input.GetAxis(controller.Vertical) < 0)
            {
                ButtonNavDown();
                m_Timer = m_TCountDown;
            }
        }
        Vector3 pos = m_Content.transform.localPosition;
        pos.y = posY;
        m_Content.transform.localPosition = Vector3.Lerp(m_Content.transform.localPosition, pos, 0.05f);
        m_Timer -= Time.fixedDeltaTime;
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
        List<int> unlockWeapons = PlayerManager.Instance.Players[player].info.UnLockWeapons;
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

    private void ButtonNavDown()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        if (go == m_weapons[m_weapons.Count - 1]) return;
        for (int i = 0; i < m_weapons.Count-1; i++)
        {
            if (go == m_weapons[i].gameObject)
            {
                EventSystem.current.SetSelectedGameObject(m_weapons[i + 1].gameObject);
                return;
            }
        }
    }
    private void ButtonNavUP()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        if (go == m_weapons[0]) return;
        for (int i = 1; i < m_weapons.Count; i++)
        {
            if (go == m_weapons[i].gameObject)
            {
                EventSystem.current.SetSelectedGameObject(m_weapons[i - 1].gameObject);
                return;
            }
        }
    }

    private void OnSelectEvent(GameObject go)
    {
        SetInfo(go);
        OnValueChange(go);
    }

    private void OnClickEvent(string s, GameObject next) 
    {
        weaponDisplay.SetCurID(weaponDisplay.Info.ID);
        EventSystem.current.SetSelectedGameObject(next);
    }

    private void SetSelectedGameObject() { }
    #endregion
}
