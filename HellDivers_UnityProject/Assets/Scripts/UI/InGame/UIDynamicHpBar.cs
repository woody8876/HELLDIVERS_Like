using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIDynamicHpBar : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }

        [SerializeField] private Image m_SerialIcon;
        [SerializeField] private Text m_NameText;
        [SerializeField] private Image m_BarBg;
        [SerializeField] private Image m_BarFill;
        [SerializeField] private Vector2 m_Position;

        public void Init(Player player)
        {
            CurrentPlayer = player;

            m_SerialIcon.color = UIHelper.GetPlayerColor(player.SerialNumber);

            m_NameText.text = CurrentPlayer.Info.Username;
            m_BarBg.enabled = false;
            m_BarFill.enabled = false;
            SubscribePlayerEvent();
        }

        private void Update()
        {
            if (CurrentPlayer != null) FollowPlayer();
        }

        private void OnDestroy()
        {
            UnsubscribePlayerEvent();
        }

        private void SubscribePlayerEvent()
        {
            CurrentPlayer.OnSpawnBegin += RefreshUI;
            CurrentPlayer.OnSpawnFinish += RefreshUI;
            CurrentPlayer.OnDamaged += RefreshUI;
            CurrentPlayer.OnTakeHealth += RefreshUI;
            CurrentPlayer.OnDeathBegin += RefreshUI;
        }

        private void UnsubscribePlayerEvent()
        {
            CurrentPlayer.OnSpawnBegin -= RefreshUI;
            CurrentPlayer.OnSpawnFinish -= RefreshUI;
            CurrentPlayer.OnDamaged -= RefreshUI;
            CurrentPlayer.OnTakeHealth -= RefreshUI;
            CurrentPlayer.OnDeathBegin -= RefreshUI;
        }

        private void RefreshUI()
        {
            this.gameObject.SetActive(!CurrentPlayer.IsDead);
            m_BarFill.fillAmount = CurrentPlayer.CurrentHpPercent;
            m_BarFill.enabled = (m_BarFill.fillAmount < 1);
            m_BarBg.enabled = (m_BarFill.fillAmount < 1);
        }

        private void FollowPlayer()
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(CurrentPlayer.transform.position);
            pos.x += m_Position.x;
            pos.y += m_Position.y;
            this.transform.position = pos;
        }
    }
}