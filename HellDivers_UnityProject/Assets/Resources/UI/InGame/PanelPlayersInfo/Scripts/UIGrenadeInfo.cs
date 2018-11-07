using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIGrenadeInfo : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }
        public int CurrentGrenadeID { get; private set; }

        [SerializeField] private Image m_Icon;
        [SerializeField] private Color m_BrightColor;
        [SerializeField] private Color m_DarkColor;
        private int m_Ammo;
        private List<Image> m_Ammos = new List<Image>();

        public void Init(Player player, int grenadeID)
        {
            CurrentPlayer = player;
            CurrentGrenadeID = grenadeID;
            m_Ammo = CurrentPlayer.GrenadesController.ActiveGrenades[CurrentGrenadeID];

            string fileName = string.Format("icon_{0}", grenadeID);
            m_Icon.sprite = UIHelper.LoadSprite(UIHelper.GrenadeIconFolder, fileName);
            m_Icon.gameObject.SetActive(false);

            for (int i = 0; i < m_Ammo; i++)
            {
                Image ammo = Instantiate(m_Icon, this.transform);
                ammo.color = m_BrightColor;
                ammo.gameObject.SetActive(true);
                m_Ammos.Add(ammo);
            }

            SubscribePlayerEvent();
            DoSwitchStartUI();
        }

        private void OnDestroy()
        {
            UnsubscribePlayerEvent();
        }

        private void SubscribePlayerEvent()
        {
            CurrentPlayer.GrenadesController.OnCount += RefreshInfo;
            CurrentPlayer.GrenadesController.OnChangeID += DoSwitchStartUI;
        }

        private void UnsubscribePlayerEvent()
        {
            CurrentPlayer.GrenadesController.OnCount -= RefreshInfo;
            CurrentPlayer.GrenadesController.OnChangeID -= DoSwitchStartUI;
        }

        private void DoSwitchStartUI()
        {
            if (CurrentPlayer.GrenadesController.CurrentID == CurrentGrenadeID)
            {
                this.gameObject.SetActive(true);
                RefreshInfo();
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        private void RefreshInfo()
        {
            m_Ammo = CurrentPlayer.GrenadesController.GrenadeCounter;

            foreach (Image img in m_Ammos)
            {
                img.color = m_DarkColor;
            }

            for (int i = 0; i < m_Ammo; i++)
            {
                m_Ammos[i].color = m_BrightColor;
            }
        }
    }
}