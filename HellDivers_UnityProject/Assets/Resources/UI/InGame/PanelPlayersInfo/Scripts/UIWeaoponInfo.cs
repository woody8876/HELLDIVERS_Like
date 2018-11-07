using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIWeaoponInfo : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }
        public WeaponInfo CurrentWeapon { get; private set; }
        private bool IsNearOutOfAmmo { get { return (m_AmmoFill.fillAmount <= 0.2f); } }

        [SerializeField] private Image m_IconBG;
        [SerializeField] private Image m_IconFill;
        [SerializeField] private Image m_AmmoFill;
        [SerializeField] private Text m_Mags;
        private Animator m_Animator;

        public void Init(Player player, WeaponInfo weapon)
        {
            CurrentPlayer = player;
            CurrentWeapon = weapon;

            string fileName = string.Format("icon_{0}", CurrentWeapon.ID);
            m_IconBG.sprite = UIHelper.LoadSprite(UIHelper.WeaponIconFolder, fileName);
            m_IconFill.sprite = m_IconBG.sprite;

            DoSwithStartUI();
            SubscribePlayerEvent();
        }

        private void Awake()
        {
            m_Animator = this.GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            UnsubscribePlayerEvent();
        }

        private void SubscribePlayerEvent()
        {
            CurrentPlayer.WeaponController.OnSwitch += DoSwithStartUI;
            CurrentPlayer.WeaponController.OnFire += RefreshInfo;
            CurrentPlayer.WeaponController.OnReload += DoReload;
            CurrentPlayer.WeaponController.OnReloadEnd += RefreshInfo;
            CurrentPlayer.WeaponController.OnPickMags += RefreshInfo;
        }

        private void UnsubscribePlayerEvent()
        {
            CurrentPlayer.WeaponController.OnSwitch -= DoSwithStartUI;
            CurrentPlayer.WeaponController.OnFire -= RefreshInfo;
            CurrentPlayer.WeaponController.OnReload -= DoReload;
            CurrentPlayer.WeaponController.OnReloadEnd -= RefreshInfo;
            CurrentPlayer.WeaponController.OnPickMags -= RefreshInfo;
        }

        private void DoSwithStartUI()
        {
            if (CurrentPlayer.WeaponController._CurrentWeapon == CurrentWeapon.ID)
            {
                StartUI();
            }
            else
            {
                StopUI();
            }
        }

        private void StartUI()
        {
            this.gameObject.SetActive(true);
            RefreshInfo();
        }

        private void StopUI()
        {
            this.gameObject.SetActive(false);
        }

        private void DoReload()
        {
            float speed = 1 / CurrentWeapon.ReloadSpeed;
            m_Animator.SetFloat("ReloadSpeed", speed);
            m_Animator.SetTrigger("Reload");
        }

        private void RefreshInfo()
        {
            m_AmmoFill.fillAmount = (float)CurrentWeapon.Ammo / CurrentWeapon.Capacity;
            m_Mags.text = string.Format("x{0}", CurrentWeapon.Mags);
            m_Animator.SetBool("IsOutOfAmmo", IsNearOutOfAmmo);
        }
    }
}