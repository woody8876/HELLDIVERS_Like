using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HELLDIVERS.UI
{
    [RequireComponent(typeof(TweenAlpha))]
    public class UIPanelReward : MonoBehaviour
    {
        #region SerializeField

        [SerializeField] private TweenAlpha m_BlackCardTween;
        [SerializeField] private Transform m_PanelReward;
        [SerializeField] private UIPlayerReward m_PlayerRewardPrefab;
        [SerializeField] private Button m_BtnContinue;

        #endregion SerializeField

        #region MonoBehaviour

        // Use this for initialization
        private void Start()
        {
            m_PlayerRewardMap = new Dictionary<int, UIPlayerReward>();
            m_BlackCardTween.OnFinished += CreatePlayerRewardElement;
            m_BlackCardTween.PlayForward();

            EventSystem.current.SetSelectedGameObject(m_BtnContinue.gameObject);
        }

        #endregion MonoBehaviour

        #region Public Function

        public void ClickContinueBtn()
        {
            m_BtnContinue.gameObject.SetActive(false);
            m_BlackCardTween.OnFinished += SceneChangeToLobby;
            m_BlackCardTween.PlayeBackward();
        }

        #endregion Public Function

        #region Private Function

        private void CreatePlayerRewardElement()
        {
            m_BlackCardTween.OnFinished -= CreatePlayerRewardElement;
            if (InGameRewardManager.Instance == null || InGameRewardManager.Instance.PlayerRewardMap.Count <= 0) return;

            foreach (var record in InGameRewardManager.Instance.PlayerRewardMap)
            {
                UIPlayerReward playerReward = Instantiate(m_PlayerRewardPrefab, m_PanelReward);
                playerReward.Initialize(PlayerManager.Instance.Players[record.Key].info, record.Value);
                m_PlayerRewardMap.Add(record.Key, playerReward);
            }
        }

        private void SceneChangeToLobby()
        {
            m_BlackCardTween.OnFinished -= SceneChangeToLobby;
            if (InGameRewardManager.Instance != null) InGameRewardManager.Instance.ApplyRewardToPlayers();
            if (SceneController.Instance != null) SceneController.Instance.ToLobby();
        }

        #endregion Private Function

        private Dictionary<int, UIPlayerReward> m_PlayerRewardMap;
    }
}