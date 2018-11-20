using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    public class UIPlayerReward : MonoBehaviour
    {
        [SerializeField] private Image m_RankIcon;
        [SerializeField] private Text m_RankText;
        [SerializeField] private Text m_PlayerName;

        public void Initialize(PlayerInfo player)
        {
            m_PlayerName.text = player.Username;
            m_RankText.text = player.Rank.ToString();
            string fileName = string.Format("icon_rank_{0}", player.Rank.ToString("00"));
            m_RankIcon.sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.RankIconFolder, fileName);
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