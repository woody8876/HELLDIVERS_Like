using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    public class UIPlayerReward : MonoBehaviour
    {
        private PlayerInfo currentPlayer;
        private PlayerRecord currentRecord;
        [SerializeField] private Image m_RankIcon;
        [SerializeField] private Text m_RankText;
        [SerializeField] private Text m_PlayerName;
        [SerializeField] private UIPlayerRewardDetail m_RewardDetailPrefab;
        private List<UIPlayerRewardDetail> m_Details = new List<UIPlayerRewardDetail>();

        public void Initialize(PlayerInfo player, PlayerRecord record)
        {
            currentPlayer = player;
            currentRecord = record;
            m_PlayerName.text = player.Username;
            m_RankText.text = player.Rank.ToString();
            string fileName = string.Format("icon_rank_{0}", player.Rank.ToString("00"));
            m_RankIcon.sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.RankIconFolder, fileName);

            CreateDetail("DEATH", record.TimesOfDeath);
            CreateDetail("KILLS", record.NumOfKills);
            CreateDetail("SHOTS", record.Shots);
            CreateDetail("MONEY", record.Money);
        }

        private void CreateDetail(string label, int number)
        {
            UIPlayerRewardDetail detail = Instantiate(m_RewardDetailPrefab, this.transform);
            detail.Initialize(label, number);
            m_Details.Add(detail);
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