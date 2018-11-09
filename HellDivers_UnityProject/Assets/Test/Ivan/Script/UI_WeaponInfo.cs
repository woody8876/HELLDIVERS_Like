using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponInfo : MonoBehaviour {

    [SerializeField] UI_Weapon m_UI_Weapon;
    [SerializeField] Text m_WeaponName;
    [SerializeField] Image m_IWeaponTexture;
    [SerializeField] RectTransform m_IPower;
    [SerializeField] RectTransform m_IFireRate;
    [SerializeField] RectTransform m_IStability;
    [SerializeField] RectTransform m_IMagazine;
    [SerializeField] RectTransform m_IRange;
    [SerializeField] Text m_FireMode;

    Vector2 m_Size = new Vector2();
    // Use this for initialization
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_WeaponName.text = m_UI_Weapon.m_WeaponInfo.Name;
            m_IWeaponTexture.sprite = m_UI_Weapon.m_Sprite;
            m_Size.y = -2;
        }
    }
    public void SetWeapon()
    {

    }
    public void SetCurrent_Power()
    {
        m_Size.x = (m_UI_Weapon.m_WeaponInfo.Damage / 1500) * 200;
        m_IPower.sizeDelta = m_Size;
    }
    public void SetCurrent_FireRate()
    {
        m_Size.x = ((1 / (m_UI_Weapon.m_WeaponInfo.FireRate * 0.017f)) / 1500) * 200;
        m_IFireRate.sizeDelta = m_Size;
    }
    public void SetCurrent_Stability()
    {
        m_Size.x = (m_UI_Weapon.m_WeaponInfo.Damage / 1500) * 200;
        m_IStability.sizeDelta = m_Size;
    }
    public void SetCurrent_Magazine()
    {
        m_Size.x = (m_UI_Weapon.m_WeaponInfo.Capacity / 75) * 200;
        m_IMagazine.sizeDelta = m_Size;
    }
    public void SetCurrent_Range()
    {
        m_Size.x = (m_UI_Weapon.m_WeaponInfo.Range / 50) * 200;
        m_IRange.sizeDelta = m_Size;
    }
    public void SetCurrent_FireMode()
    {
        m_FireMode.text = (m_UI_Weapon.m_WeaponInfo.FireMode == 0) ? "SEMI - AUTO" : "FULL - AUTO";
    }





}
