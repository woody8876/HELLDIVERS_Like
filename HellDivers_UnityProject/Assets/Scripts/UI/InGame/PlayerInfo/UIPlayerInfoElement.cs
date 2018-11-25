using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIPlayerInfoElement : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }

        [SerializeField] private Image m_Header;
        [SerializeField] private Transform m_Panel_Title;
        [SerializeField] private Transform m_Panel_StratagemCD;
        [SerializeField] private GameObject m_PlayerTitle;
        [SerializeField] private GameObject m_Panel_WeaponAndGrenadePrefab;
        [SerializeField] private GameObject m_StratagemCDInfoPrefab;
        [SerializeField] private GameObject m_StratagemInfoPrefab;

        private UIPlayerTitle m_UIPlayerTitle;
        private UIPanelWeapons m_UIPanelWeapons;
        private Animator m_Panel_WeaponAnimator;
        private List<UIStratagemCDInfo> m_UIStratgemCDInfos = new List<UIStratagemCDInfo>();
        private List<UIStratagemInfo> m_UIStratagemInfos = new List<UIStratagemInfo>();

        private void Awake()
        {
            if (m_Panel_Title == null) m_Panel_Title = this.transform.Find("Panel_Title");
            if (m_Panel_StratagemCD == null) m_Panel_StratagemCD = this.transform.Find("Panel_StratagemsCD");
        }

        public void Init(Player player)
        {
            CurrentPlayer = player;

            m_Header.color = UIHelper.GetPlayerColor(player.SerialNumber);

            m_UIPlayerTitle = Instantiate(m_PlayerTitle, m_Panel_Title).GetComponent<UIPlayerTitle>();
            m_UIPlayerTitle.Init(CurrentPlayer);

            m_UIPanelWeapons = Instantiate(m_Panel_WeaponAndGrenadePrefab, this.transform).GetComponent<UIPanelWeapons>();
            m_UIPanelWeapons.Init(CurrentPlayer);

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
        }
    }
}