using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIStratagemCDInfo : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }
        public Stratagem CurrentStratagem { get; private set; }
        [SerializeField] private Image m_Background;
        [SerializeField] private Image m_Fill;
        private Sprite m_FillIcon;
        private Sprite m_BGIcon;

        public void Init(Player player, Stratagem stratagem)
        {
            CurrentPlayer = player;
            CurrentStratagem = stratagem;

            string fileName = string.Format("icon_{0}", CurrentStratagem.Info.ID);
            m_FillIcon = UIHelper.LoadSprite(UIHelper.StratagemIconFolder, fileName);
            m_Fill.sprite = m_FillIcon;

            fileName = string.Format("icon_{0}_gray", CurrentStratagem.Info.ID);
            m_BGIcon = UIHelper.LoadSprite(UIHelper.StratagemIconFolder, fileName);
            m_Background.sprite = m_BGIcon;

            SubscribePlayerEvent();
        }

        private void OnDestroy()
        {
            UnsubscribePlayerEvent();
        }

        private void SubscribePlayerEvent()
        {
            CurrentPlayer.OnStartSpawnNotify += RefreshInfo;
            CurrentPlayer.OnStartDeathNotify += StopUI;

            CurrentStratagem.OnStartCoolDown += RefreshInfo;
            CurrentStratagem.OnCoolDown += RefreshInfo;
            CurrentStratagem.OnEndCoolDown += RefreshInfo;
        }

        private void UnsubscribePlayerEvent()
        {
            CurrentPlayer.OnStartSpawnNotify -= RefreshInfo;
            CurrentPlayer.OnStartDeathNotify -= StopUI;

            CurrentStratagem.OnStartCoolDown -= RefreshInfo;
            CurrentStratagem.OnCoolDown -= RefreshInfo;
            CurrentStratagem.OnEndCoolDown -= RefreshInfo;
        }

        public void RefreshInfo()
        {
            this.gameObject.SetActive(CurrentStratagem.IsCooling);
            if (CurrentStratagem.IsCooling) m_Fill.fillAmount = CurrentStratagem.CoolTimer / CurrentStratagem.Info.CoolDown;
        }

        private void StopUI()
        {
            this.gameObject.SetActive(false);
            m_Fill.fillAmount = 0.0f;
        }
    }
}