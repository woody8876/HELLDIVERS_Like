using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIPlayerInfoElement : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }

        [SerializeField] private Transform m_Panel_Title;
        [SerializeField] private Transform m_Panel_StratagemCD;
        [SerializeField] private Transform m_Panel_WeaponAndGrenade;
        [SerializeField] private Transform m_Panel_Weapon;
        [SerializeField] private Transform m_Panel_Grenade;
        [SerializeField] private GameObject m_PlayerTitle;
        [SerializeField] private GameObject m_StratagemCDInfoPrefab;
        [SerializeField] private GameObject m_WeaponInfoPrefab;
        [SerializeField] private GameObject m_GrenadeInfoPrefab;
        [SerializeField] private GameObject m_StratagemInfoPrefab;

        private UIPlayerTitle m_UIPlayerTitle;
        private List<UIStratagemCDInfo> m_UIStratgemCDInfos = new List<UIStratagemCDInfo>();
        private List<UIWeaoponInfo> m_UIWeaponInfos = new List<UIWeaoponInfo>();
        private List<UIStratagemInfo> m_UIStratagemInfos = new List<UIStratagemInfo>();

        private void Awake()
        {
            if (m_Panel_Title == null) m_Panel_Title = this.transform.Find("Panel_Title");
            if (m_Panel_StratagemCD == null) m_Panel_StratagemCD = this.transform.Find("Panel_StratagemsCD");
            if (m_Panel_WeaponAndGrenade == null) m_Panel_WeaponAndGrenade = this.transform.Find("Panel_WeaponsAndGrenade");
            if (m_Panel_Weapon == null) m_Panel_Weapon = this.transform.Find("Panel_WeaponsAndGrenade/Weapons");
            if (m_Panel_Grenade == null) m_Panel_Grenade = this.transform.Find("Panel_WeaponsAndGrenade/Grenades");
        }

        public void Init(Player player)
        {
            CurrentPlayer = player;

            m_UIPlayerTitle = Instantiate(m_PlayerTitle, m_Panel_Title).GetComponent<UIPlayerTitle>();
            m_UIPlayerTitle.Init(player);

            if (m_StratagemCDInfoPrefab != null)
            {
                for (int i = 0; i < CurrentPlayer.StratagemController.Stratagems.Count; i++)
                {
                    UIStratagemCDInfo stratagemCDInfo = Instantiate(m_StratagemCDInfoPrefab, m_Panel_StratagemCD).GetComponent<UIStratagemCDInfo>();
                    stratagemCDInfo.Init(CurrentPlayer, CurrentPlayer.StratagemController.Stratagems[i]);
                    m_UIStratgemCDInfos.Add(stratagemCDInfo);
                    stratagemCDInfo.gameObject.SetActive(false);

                    UIStratagemInfo stratagemInfo = Instantiate(m_StratagemInfoPrefab, this.transform).GetComponent<UIStratagemInfo>();
                    stratagemInfo.Init(CurrentPlayer, CurrentPlayer.StratagemController.Stratagems[i]);
                    m_UIStratagemInfos.Add(stratagemInfo);
                    stratagemInfo.gameObject.SetActive(false);
                }
            }

            if (m_WeaponInfoPrefab != null)
            {
                foreach (KeyValuePair<int, IWeaponBehaviour> weapon in CurrentPlayer.WeaponController.ActiveWeapon)
                {
                    UIWeaoponInfo weaoponInfo = Instantiate(m_WeaponInfoPrefab, m_Panel_Weapon).GetComponent<UIWeaoponInfo>();
                    weaoponInfo.Init(CurrentPlayer, weapon.Value.weaponInfo);
                    m_UIWeaponInfos.Add(weaoponInfo);
                }
            }
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}