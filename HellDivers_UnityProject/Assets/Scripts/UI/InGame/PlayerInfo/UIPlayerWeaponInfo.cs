using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerWeaponInfo : MonoBehaviour
{
    public WeaponInfo CurrentWeaponInfo { get { return m_CurrentWeaponInfo; } }
    private WeaponInfo m_CurrentWeaponInfo;

    private Animator m_Animator;
    [SerializeField] private Image m_Icon;
    [SerializeField] private Image m_IconFill;
    [SerializeField] private Image m_AmmoAmount;
    [SerializeField] private Text m_MagAmount;

    public void Initialize(WeaponInfo info)
    {
        m_CurrentWeaponInfo = info;

        string fileName = string.Format("icon_{0}", m_CurrentWeaponInfo.ID);
        m_Icon.sprite = UIHelper.LoadSprite(UIHelper.WeaponIconFolder, fileName);
        m_IconFill.sprite = m_Icon.sprite;

        UpdateAmmo();
    }

    private void Awake()
    {
        m_Animator = this.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_Animator.SetTrigger("Show");
        m_IconFill.fillAmount = 1.0f;
    }

    public void StartReload()
    {
        float reloadSpeed = 1 / m_CurrentWeaponInfo.ReloadSpeed;
        m_Animator.SetFloat("ReloadTime", reloadSpeed);
        m_Animator.SetTrigger("Reload");
    }

    public void UpdateAmmo()
    {
        float ammoAmount = (float)m_CurrentWeaponInfo.Ammo / m_CurrentWeaponInfo.Capacity;
        m_AmmoAmount.fillAmount = Mathf.Clamp01(ammoAmount);

        m_MagAmount.text = string.Format("x{0}", m_CurrentWeaponInfo.Mags);

        bool bOutOfAmmo = (m_AmmoAmount.fillAmount <= 0.2f);
        m_Animator.SetBool("OutOfAmmo", bOutOfAmmo);
    }
}