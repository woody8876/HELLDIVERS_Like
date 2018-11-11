using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponInfo : MonoBehaviour {

    [SerializeField] Text m_WeaponName;
    [SerializeField] Image m_IWeaponTexture;
    [SerializeField] RectTransform m_IPower;
    [SerializeField] RectTransform m_IFireRate;
    [SerializeField] RectTransform m_IStability;
    [SerializeField] RectTransform m_IMagazine;
    [SerializeField] RectTransform m_IRange;
    [SerializeField] Text m_FireMode;
    Vector2 m_Size = new Vector2();


    public void SetWeapon(LobbyUI_Weapon uI_Weapon)
    {
        m_Size.y = -2;
        SetCurrentUI(uI_Weapon);
        SetCurrent_FireMode(uI_Weapon);
        SetCurrent_FireRate(uI_Weapon);
        SetCurrent_Power(uI_Weapon);
        SetCurrent_Magazine(uI_Weapon);
        SetCurrent_Range(uI_Weapon);
        SetCurrent_Stability(uI_Weapon);
    }


    #region Private method
    private void SetCurrentUI(LobbyUI_Weapon uI_Weapon)
    {
        m_WeaponName.text = uI_Weapon.m_WeaponInfo.Name;
        m_IWeaponTexture.sprite = uI_Weapon.m_Sprite;
    }
    private void SetCurrent_Power(LobbyUI_Weapon uI_Weapon)
    {
        m_Size.x = (uI_Weapon.m_WeaponInfo.Damage / 1500) * 200;
        m_IPower.sizeDelta = m_Size;
    }
    private void SetCurrent_FireRate(LobbyUI_Weapon uI_Weapon)
    {
        m_Size.x = ((1 / (uI_Weapon.m_WeaponInfo.FireRate * 0.017f)) / 1500) * 200;
        m_IFireRate.sizeDelta = m_Size;
    }
    private void SetCurrent_Stability(LobbyUI_Weapon uI_Weapon)
    {
        m_Size.x = (uI_Weapon.m_WeaponInfo.Damage / 1500) * 200;
        m_IStability.sizeDelta = m_Size;
    }
    private void SetCurrent_Magazine(LobbyUI_Weapon uI_Weapon)
    {
        m_Size.x = (uI_Weapon.m_WeaponInfo.Capacity / 75) * 200;
        m_IMagazine.sizeDelta = m_Size;
    }
    private void SetCurrent_Range(LobbyUI_Weapon uI_Weapon)
    {
        m_Size.x = (uI_Weapon.m_WeaponInfo.Range / 50) * 200;
        m_IRange.sizeDelta = m_Size;
    }
    private void SetCurrent_FireMode(LobbyUI_Weapon uI_Weapon)
    {
        m_FireMode.text = (uI_Weapon.m_WeaponInfo.FireMode == 0) ? "SEMI - AUTO" : "FULL - AUTO";
    }
    #endregion




}
