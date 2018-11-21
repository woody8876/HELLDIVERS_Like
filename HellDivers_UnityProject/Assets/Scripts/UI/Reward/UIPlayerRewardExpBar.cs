using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    public class UIPlayerRewardExpBar : MonoBehaviour
    {
        [SerializeField] private Text m_textExp;
        [SerializeField] private Image m_imgFill;
        private RankData m_currentRank;

        public void Initialize(int startAmount, int targetAmount)
        {
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