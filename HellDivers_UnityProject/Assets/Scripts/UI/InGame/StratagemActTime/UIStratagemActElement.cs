using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIStratagemActElement : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }
        public Stratagem CurrentStratagem { get; private set; }

        [SerializeField] private Image m_Icon;
        [SerializeField] private Text m_Time;

        public void Init(Stratagem stratagem)
        {
            CurrentStratagem = stratagem;

            string fileName = string.Format("icon_{0}", stratagem.Info.ID);
            m_Icon.sprite = UIHelper.LoadSprite(UIHelper.StratagemIconFolder, fileName);

            SubscribePlayerEvent();
        }

        private void OnDestroy()
        {
            UnsubscribePlayerEvent();
        }

        private void SubscribePlayerEvent()
        {
            CurrentStratagem.OnStartActivation += RefreshUI;
            CurrentStratagem.OnActivation += RefreshUI;
            CurrentStratagem.OnEndActivation += RefreshUI;
        }

        private void UnsubscribePlayerEvent()
        {
            CurrentStratagem.OnStartActivation -= RefreshUI;
            CurrentStratagem.OnActivation -= RefreshUI;
            CurrentStratagem.OnEndActivation -= RefreshUI;
        }

        private void RefreshUI()
        {
            string minutes = Mathf.Floor(CurrentStratagem.ActTimeCountDown / 60).ToString("00");
            string seconds = (CurrentStratagem.ActTimeCountDown % 60).ToString("00");
            string minisec = Mathf.Floor((CurrentStratagem.ActTimeCountDown * 100) % 100).ToString("00");
            m_Time.text = string.Format("{0}:{1}:{2}", minutes, seconds, minisec);
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