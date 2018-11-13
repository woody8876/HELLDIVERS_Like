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
        [SerializeField] private Image m_Fill;
        [SerializeField] private Vector2 m_Position;

        public void Init(Player player)
        {
            CurrentPlayer = player;
            m_NameText.text = CurrentPlayer.Info.Username;

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
            CurrentPlayer.OnDeathBegin += RefreshUI;
        }

        private void UnsubscribePlayerEvent()
        {
            CurrentPlayer.OnSpawnBegin -= RefreshUI;
            CurrentPlayer.OnSpawnFinish -= RefreshUI;
            CurrentPlayer.OnDamaged -= RefreshUI;
            CurrentPlayer.OnDeathBegin -= RefreshUI;
        }

        private void RefreshUI()
        {
            this.gameObject.SetActive(!CurrentPlayer.IsDead);
            m_Fill.fillAmount = CurrentPlayer.CurrentHpPercent;
            m_Fill.enabled = (m_Fill.fillAmount < 1);
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