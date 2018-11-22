using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIWeaponList : MonoBehaviour {

    [SerializeField] UIWeaponDisplay weaponDisplay;
    [SerializeField] GameObject m_WeaponUI;
    [SerializeField] GameObject m_Content;

    List<GameObject> m_weapons = new List<GameObject>();
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
    GameObject m_currentSelectObject;
    // Use this for initialization
    void Start()
    {
        int PlayerID = weaponDisplay.SetPlayer.PlayerID;
        CreateWeaponUI(PlayerID);
        SubscriptAxisEvent();
        ChangeWeapon();
    }
    private void OnEnable()
    {
        if (m_weapons.Count < 1) return;
        SubscriptAxisEvent();
        ChangeWeapon();
    }
    private void OnDisable()
    {
        UnsubscribeAxisEvent();
    }

    private void LateUpdate()
    {
        Vector3 pos = m_Content.transform.localPosition;
        pos.y = posY;
        m_Content.transform.localPosition = Vector3.Lerp(m_Content.transform.localPosition, pos, 0.05f);
    }

    private void SubscriptAxisEvent()
    {
        weaponDisplay.SetPlayer.Control.AxisUp += ButtonUp;
        weaponDisplay.SetPlayer.Control.AxisDown += ButtonDown;
        weaponDisplay.SetPlayer.Control.AxisSubmit += ButtonSubmit;
        weaponDisplay.SetPlayer.Control.AxisCancel += ButtonCancel;
    }

    private void UnsubscribeAxisEvent()
    {
        weaponDisplay.SetPlayer.Control.AxisUp -= ButtonUp;
        weaponDisplay.SetPlayer.Control.AxisDown -= ButtonDown;
        weaponDisplay.SetPlayer.Control.AxisSubmit -= ButtonSubmit;
        weaponDisplay.SetPlayer.Control.AxisCancel -= ButtonCancel;
    }

    private void ChangeWeapon()
    {
        for (int i = 0; i < m_weapons.Count; i++)
        {
            if (Determine() == m_weapons[i].gameObject.name)
            {
                m_currentSelectObject = m_weapons[i].gameObject;
                OnSelectEvent(m_currentSelectObject);
                return;
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
        List<int> unlockWeapons = PlayerManager.Instance.Players[player].info.UnLockWeapons;
        for (int i = 0; i < unlockWeapons.Count; i++)
        {
            go = Instantiate(m_WeaponUI, m_Content.transform);
            int id = unlockWeapons[i];
            go.name = id.ToString();
            weaponDisplay.SetWeaponUI(go, id);
            m_weapons.Add(go);
        }
    }

    private string Determine()
    {
        if (weaponDisplay.SetPlayer.m_bPrimary) return weaponDisplay.SetPlayer.PriWeaponID.ToString();
        else return weaponDisplay.SetPlayer.SecWeaponID.ToString();
    }

    #region Button Behaviors

    private void ButtonUp()
    {
        DisSelectEvent(m_currentSelectObject);
        ButtonNavUP();
        OnSelectEvent(m_currentSelectObject);

    }

    private void ButtonDown()
    {
        DisSelectEvent(m_currentSelectObject);
        ButtonNavDown();
        OnSelectEvent(m_currentSelectObject);
    }

    private void ButtonSubmit()
    {
        OnClickEvent();
    }

    private void ButtonCancel()
    {
        DisSelectEvent(m_currentSelectObject);
        weaponDisplay.SetPlayer.SelectWeaponUI(false);
    }

    #endregion


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
        GameObject go = m_currentSelectObject;
        if (go == m_weapons[m_weapons.Count - 1]) return;
        for (int i = 0; i < m_weapons.Count-1; i++)
        {
            if (go == m_weapons[i].gameObject)
            {
                m_currentSelectObject = m_weapons[i + 1].gameObject;
                return;
            }
        }
    }

    private void ButtonNavUP()
    {
        GameObject go = m_currentSelectObject;
        if (go == m_weapons[0]) return;
        for (int i = 1; i < m_weapons.Count; i++)
        {
            if (go == m_weapons[i].gameObject)
            {
                m_currentSelectObject = m_weapons[i - 1].gameObject;
                return;
            }
        }
    }

    private void OnSelectEvent(GameObject go)
    {
        SetInfo(go);
        OnValueChange(go);
        go.GetComponent<LobbyUI_Weapon>().SetHighlightBG();
    }

    private void DisSelectEvent(GameObject go)
    {
        go.GetComponent<LobbyUI_Weapon>().SetBG();
    }
    
    private void OnClickEvent() 
    {
        weaponDisplay.SetCurID(weaponDisplay.Info.ID);
        DisSelectEvent(m_currentSelectObject);
        UnsubscribeAxisEvent();
        weaponDisplay.Button.SetCancel();
        weaponDisplay.Button.OnSelectButton();
    }

    public void Return()
    {
        weaponDisplay.Button.SetCancel(false);
        SubscriptAxisEvent();
        OnSelectEvent(m_currentSelectObject);
    }
    #endregion
}
