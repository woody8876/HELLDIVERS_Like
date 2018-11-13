using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIPlayerTitle : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }
        [SerializeField] private Image m_RankIcon;
        [SerializeField] private Image m_DeadIcon;
        [SerializeField] private Text m_RankText;
        [SerializeField] private Text m_NameText;

        public void Init(Player player)
        {
            CurrentPlayer = player;

            int rank = CurrentPlayer.Info.Rank;
            if (rank > 40) rank = 40;
            else if (rank < 0) rank = 0;
            string fileName = string.Format("icon_rank_{0}", rank.ToString("00"));
            Sprite loadedSprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.RankIconFolder, fileName);
            if (loadedSprite != null) m_RankIcon.sprite = loadedSprite;

            RefershInfo();
            SubscribePlayerEvent();
        }

        private void OnDestroy()
        {
            UnsubcribePlayerEvent();
        }

        private void SubscribePlayerEvent()
        {
            CurrentPlayer.OnSpawnBegin += OnAliveView;
            CurrentPlayer.OnDeathBegin += OnDeathView;
        }

        private void UnsubcribePlayerEvent()
        {
            CurrentPlayer.OnSpawnBegin -= OnAliveView;
            CurrentPlayer.OnDeathBegin -= OnDeathView;
        }

        private void OnAliveView()
        {
            m_DeadIcon.gameObject.SetActive(false);

            RefershInfo();
        }

        private void OnDeathView()
        {
            m_DeadIcon.gameObject.SetActive(true);

            RefershInfo();
        }

        public void RefershInfo()
        {
            m_RankText.text = CurrentPlayer.Info.Rank.ToString();
            m_NameText.text = CurrentPlayer.Info.Username;
        }
    }
}