﻿using System.Collections;
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
        [SerializeField] private Text m_GameTime;
        [SerializeField] private Transform m_PanelMissionReward;
        [SerializeField] private UIMissionReward m_MissionRewardPrefab;
        [SerializeField] private UIMissionRewardEXP m_MissionRewardExp;
        [SerializeField] private Transform m_PanelReward;
        [SerializeField] private UIPlayerReward m_PlayerRewardPrefab;
        [SerializeField] private Button m_BtnContinue;

        #endregion SerializeField

        #region MonoBehaviour

        // Use this for initialization
        private void Start()
        {
            m_MissionRewardMap = new Dictionary<eMissionType, UIMissionReward>();
            m_PlayerRewardMap = new Dictionary<int, UIPlayerReward>();
            CreatePlayerRewardElement();
            CreateMissionRewardElement();

            m_BlackCardTween.OnFinished += StartDrawUI;
            m_BlackCardTween.PlayForward();

            m_BtnContinue.gameObject.SetActive(false);
        }

        private void StartDrawUI()
        {
            StartCoroutine(OnDarw());
        }

        private IEnumerator OnDarw()
        {
            if (m_MissionRewardMap.Count > 0)
            {
                float missionUITimeLenght = 0;
                foreach (var missionUI in m_MissionRewardMap)
                {
                    missionUI.Value.DrawUI();
                    missionUITimeLenght = missionUI.Value.CanvasTween.TimeLenght;
                }
                yield return new WaitForSeconds(missionUITimeLenght);

                m_MissionRewardExp.DrawUI();
                yield return new WaitForSeconds(m_MissionRewardExp.CanvasTween.TimeLenght);
            }

            PrintGameTime();

            foreach (var playerUI in m_PlayerRewardMap)
            {
                playerUI.Value.DrawUI();
            }
        }

        #endregion MonoBehaviour

        #region Public Function

        public void ClickContinueBtn()
        {
            MusicManager.Instance.FadeOut(1);
            m_BlackCardTween.OnFinished += SceneChangeToLobby;
            m_BlackCardTween.PlayeBackward();
        }

        #endregion Public Function

        #region Private Function

        private void PrintGameTime()
        {
            m_BlackCardTween.OnFinished -= PrintGameTime;
            float gameTime = InGameRewardManager.Instance.GameDurationTime;
            string minutes = Mathf.Floor(gameTime / 60).ToString("00");
            string seconds = (gameTime % 60).ToString("00");
            m_GameTime.text = string.Format("Mission Time - {0}:{1}", minutes, seconds);
            UITweenCanvasGroup tween = m_GameTime.GetComponent<UITweenCanvasGroup>();
            tween.Play();
        }

        private void CreatePlayerRewardElement()
        {
            m_BlackCardTween.OnFinished -= CreatePlayerRewardElement;
            if (InGameRewardManager.Instance == null || InGameRewardManager.Instance.PlayerRewardMap.Count <= 0) return;

            MissionReward missionReward = InGameRewardManager.Instance.CaculateAllMissionReward();

            foreach (var record in InGameRewardManager.Instance.PlayerRewardMap)
            {
                UIPlayerReward playerReward = Instantiate(m_PlayerRewardPrefab, m_PanelReward);
                playerReward.Initialize(PlayerManager.Instance.Players[record.Key].info, record.Value, missionReward, record.Key);
                playerReward.OnDrawUIFinished += RefreshCountinueBtn;
                m_DrawFinishedCount++;
                m_PlayerRewardMap.Add(record.Key, playerReward);
            }
        }

        private void CreateMissionRewardElement()
        {
            if (InGameRewardManager.Instance == null || InGameRewardManager.Instance.MissionRewardMap.Count <= 0) return;

            foreach (var record in InGameRewardManager.Instance.MissionRewardMap)
            {
                UIMissionReward missionReward = Instantiate(m_MissionRewardPrefab, m_PanelMissionReward);
                missionReward.Initialize(record.Key);
                missionReward.OnTweenFinished += AddMissionEXP;

                m_MissionRewardMap.Add(record.Key, missionReward);
            }
        }

        private void AddMissionEXP(eMissionType type)
        {
            if (InGameRewardManager.Instance == null) return;
            if (InGameRewardManager.Instance.MissionRewardMap.ContainsKey(type) == false) return;

            List<MissionReward> pList = InGameRewardManager.Instance.MissionRewardMap[type];
            int totalExp = 0;
            for (int i = 0; i < pList.Count; i++)
            {
                totalExp += pList[i].EXP;
            }

            m_MissionRewardExp.AddExp(totalExp);
        }

        private void RefreshCountinueBtn()
        {
            m_DrawFinishedCount--;
            if (m_DrawFinishedCount <= 0)
            {
                m_BtnContinue.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(m_BtnContinue.gameObject);
            }
        }

        private void SceneChangeToLobby()
        {
            m_BlackCardTween.OnFinished -= SceneChangeToLobby;
            if (InGameRewardManager.Instance != null) InGameRewardManager.Instance.ApplyRewardToPlayers();
            if (SceneController.Instance != null) SceneController.Instance.ToLobby();
        }

        #endregion Private Function

        private int m_DrawFinishedCount;
        private Dictionary<eMissionType, UIMissionReward> m_MissionRewardMap;
        private Dictionary<int, UIPlayerReward> m_PlayerRewardMap;
    }
}