using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    public class UIPlayerReward : MonoBehaviour
    {
        private PlayerInfo m_CurrentPlayer;
        private PlayerRecord m_CurrentRecord;
        private Animator m_Animator;
        [SerializeField] private Image m_RankIcon;
        [SerializeField] private Text m_RankText;
        [SerializeField] private Text m_PlayerName;
        [SerializeField] private UIPlayerRewardExpBar m_ExpBar;
        [SerializeField] private UIPlayerRewardDetail m_RewardDetailPrefab;
        private List<UIPlayerRewardDetail> m_Details = new List<UIPlayerRewardDetail>();

        public void Initialize(PlayerInfo player, PlayerRecord record)
        {
            m_CurrentPlayer = player;
            m_CurrentRecord = record;
            m_PlayerName.text = player.Username;
            m_RankText.text = player.Rank.ToString();
            string fileName = string.Format("icon_rank_{0}", player.Rank.ToString("00"));
            m_RankIcon.sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.RankIconFolder, fileName);

            m_ExpBar = Instantiate(m_ExpBar, this.transform);
            m_ExpBar.OnRankUpdate += RefreshRankText;
            m_ExpBar.Initialize(player.Exp, player.Exp + record.Exp, player.Rank);
            CreateDetail("DEATH", record.TimesOfDeath);
            CreateDetail("KILLS", record.NumOfKills);
            CreateDetail("SHOTS", record.Shots);
            CreateDetail("MONEY", record.Money);

            DrawUI();
        }

        private void CreateDetail(string label, int number)
        {
            UIPlayerRewardDetail detail = Instantiate(m_RewardDetailPrefab, this.transform);
            detail.Initialize(label, number);
            m_Details.Add(detail);
        }

        public void DrawUI()
        {
            StartCoroutine(DoDraw());
        }

        private IEnumerator DoDraw()
        {
            yield return new WaitUntil
                (() =>
                    {
                        if (m_Animator.IsInTransition(0)) return false;

                        var state = m_Animator.GetCurrentAnimatorStateInfo(0);
                        if (state.normalizedTime >= 0.98f) return true;
                        return false;
                    }
                );

            m_ExpBar.CanvasTween.Play();
            yield return new WaitForSeconds(m_ExpBar.CanvasTween.TimeLenght);

            m_ExpBar.DoEvaluate();

            for (int i = 0; i < m_Details.Count; i++)
            {
                UITweenCanvasGroup tween = m_Details[i].CanvasTween;
                tween.Play();
                yield return new WaitForSeconds(tween.TimeLenght);
            }
        }

        private void RefreshRankText()
        {
            m_RankText.text = m_ExpBar.CurrentRank.ToString();
        }

        private void Awake()
        {
            m_Animator = this.GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            m_ExpBar.OnRankUpdate -= RefreshRankText;
        }
    }
}