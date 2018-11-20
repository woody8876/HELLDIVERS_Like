using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    public class UIPanelReward : MonoBehaviour
    {
        [SerializeField] private TweenAlpha m_BlackCardTween;
        [SerializeField] private Transform m_PanelReward;
        [SerializeField] private UIPlayerReward m_PlayerRewardPrefab;

        private Dictionary<int, UIPlayerReward> m_PlayerRewardMap;

        private void Awake()
        {
            m_PlayerRewardMap = new Dictionary<int, UIPlayerReward>();
        }

        // Use this for initialization
        private void Start()
        {
            for (int i = 1; i <= InGameRewardManager.Instance.PlayerRewardMap.Count; i++)
            {
                UIPlayerReward playerReward = Instantiate(m_PlayerRewardPrefab, m_PanelReward);
                playerReward.Initialize(PlayerManager.Instance.Players[i].info);
            }

            if (m_BlackCardTween != null) m_BlackCardTween.PlayForward();
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}