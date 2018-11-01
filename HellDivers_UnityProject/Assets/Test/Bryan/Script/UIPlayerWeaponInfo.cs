using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerWeaponInfo : MonoBehaviour
{
    private WeaponInfo m_CurrentWeapon;
    private Animator m_Animator;
    [SerializeField] private Image m_Icon;
    [SerializeField] private Image m_AmmoAmount;
    [SerializeField] private Text m_MagAmount;

    public void Initialize(WeaponInfo info)
    {
        m_CurrentWeapon = info;
        m_Icon.sprite = LoadWeaponIcon();
        UpdateAmmoDisplay();
    }

    private void Awake()
    {
        m_Animator = this.GetComponent<Animator>();
    }

    private Sprite LoadWeaponIcon()
    {
        Sprite iconImg = null;
        string imgName = string.Format("icon_{0}", m_CurrentWeapon.ID);
        string imgPath = "UI/Resource/Icons/Weapon";
        string fullPath = imgPath + "/" + imgName;

        if (AssetManager.m_Instance != null)
        {
            iconImg = AssetManager.m_Instance.GetAsset(typeof(Sprite), imgName, imgPath) as Sprite;
            if (iconImg == null)
            {
                iconImg = Resources.Load<Sprite>(fullPath);
                AssetManager.m_Instance.AddAsset(typeof(Sprite), imgName, imgPath, iconImg);
            }
        }
        else
        {
            iconImg = Resources.Load<Sprite>(fullPath);
        }
        return iconImg;
    }

    public void UpdateAmmoDisplay()
    {
        float ammoAmount = (float)m_CurrentWeapon.Ammo / m_CurrentWeapon.Capacity;
        m_AmmoAmount.fillAmount = Mathf.Clamp01(ammoAmount);

        m_MagAmount.text = string.Format("x{0}", m_CurrentWeapon.Mags);

        bool bOutOfAmmo = (m_AmmoAmount.fillAmount <= 0.2f);
        m_Animator.SetBool("OutOfAmmo", bOutOfAmmo);
    }
}