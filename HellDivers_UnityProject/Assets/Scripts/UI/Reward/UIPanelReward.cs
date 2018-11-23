using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HELLDIVERS.UI
{
    public class UIPanelReward : MonoBehaviour
    {
        [SerializeField] private TweenAlpha m_BlackCardTween;
        [SerializeField] private Transform m_PanelReward;
        [SerializeField] private UIPlayerReward m_PlayerRewardPrefab;
        [SerializeField] private Button m_BtnContinue;

        private Dictionary<int, UIPlayerReward> m_PlayerRewardMap;

        public void ClickContinueBtn()
        {
            if (SceneController.Instance != null) SceneController.Instance.ToLobby();
        }

        private void Awake()
        {
            m_PlayerRewardMap = new Dictionary<int, UIPlayerReward>();
        }

        // Use this for initialization
        private void Start()
        {
            if (m_BlackCardTween != null)
            {
                m_BlackCardTween.OnFinished += CreatePlayerRewardElement;
                m_BlackCardTween.PlayForward();
            }

            EventSystem.current.SetSelectedGameObject(m_BtnContinue.gameObject);
        }

        // Update is called once per frame
        private void Update()
        {
        }

        private void CreatePlayerRewardElement()
        {
            if (InGameRewardManager.Instance == null || InGameRewardManager.Instance.PlayerRewardMap.Count <= 0) return;

            foreach (KeyValuePair<int, PlayerRecord> record in InGameRewardManager.Instance.PlayerRewardMap)
            {
                UIPlayerReward playerReward = Instantiate(m_PlayerRewardPrefab, m_PanelReward);
                playerReward.Initialize(PlayerManager.Instance.Players[record.Key].info, record.Value);
            }
        }
    }
}