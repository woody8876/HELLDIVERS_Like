using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIPanelWeapons : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }

        [SerializeField] private Transform m_Panel_Weapon;
        [SerializeField] private Transform m_Panel_Grenade;
        [SerializeField] private GameObject m_WeaponInfoPrefab;
        [SerializeField] private GameObject m_GrenadeInfoPrefab;
        private Animator m_Animator;
        private List<UIWeaoponInfo> m_UIWeaponInfos = new List<UIWeaoponInfo>();
        private List<UIGrenadeInfo> m_UIGrenadeInfos = new List<UIGrenadeInfo>();

        public void Init(Player player)
        {
            CurrentPlayer = player;

            if (m_WeaponInfoPrefab != null)
            {
                foreach (KeyValuePair<int, IWeaponBehaviour> weapon in CurrentPlayer.WeaponController.ActiveWeapon)
                {
                    UIWeaoponInfo weaoponInfo = Instantiate(m_WeaponInfoPrefab, m_Panel_Weapon).GetComponent<UIWeaoponInfo>();
                    weaoponInfo.Init(CurrentPlayer, weapon.Value.weaponInfo);
                    m_UIWeaponInfos.Add(weaoponInfo);
                }
            }

            if (m_GrenadeInfoPrefab != null)
            {
                foreach (KeyValuePair<int, int> grenade in CurrentPlayer.GrenadesController.ActiveGrenades)
                {
                    UIGrenadeInfo grenadeInfo = Instantiate(m_GrenadeInfoPrefab, m_Panel_Grenade).GetComponent<UIGrenadeInfo>();
                    grenadeInfo.Init(CurrentPlayer, grenade.Key);
                    m_UIGrenadeInfos.Add(grenadeInfo);
                }
            }

            StartWeaponPanel();
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
            CurrentPlayer.WeaponController.OnSwitch += StartWeaponPanel;
            CurrentPlayer.StratagemController.CheckCodesMechine.OnStart += StopWeaponPanel;
            CurrentPlayer.StratagemController.CheckCodesMechine.OnFaild += StartWeaponPanel;
            CurrentPlayer.StratagemController.OnThrow += StartWeaponPanel;
            CurrentPlayer.OnStartSpawnNotify += StartWeaponPanel;
            CurrentPlayer.OnStartDeathNotify += StopWeaponPanel;
        }

        private void UnsubscribePlayerEvent()
        {
            CurrentPlayer.WeaponController.OnSwitch -= StartWeaponPanel;
            CurrentPlayer.StratagemController.CheckCodesMechine.OnStart -= StopWeaponPanel;
            CurrentPlayer.StratagemController.CheckCodesMechine.OnFaild -= StartWeaponPanel;
            CurrentPlayer.StratagemController.OnThrow -= StartWeaponPanel;
            CurrentPlayer.OnStartSpawnNotify -= StartWeaponPanel;
            CurrentPlayer.OnStartDeathNotify -= StopWeaponPanel;
        }

        private void StartWeaponPanel()
        {
            this.gameObject.SetActive(true);
            m_Animator.SetTrigger("Open");
        }

        private void StopWeaponPanel()
        {
            m_Animator.gameObject.SetActive(false);
        }
    }
}