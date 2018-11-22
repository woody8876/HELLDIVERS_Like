using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    [RequireComponent(typeof(UITweenCanvasGroup))]
    public class UIPlayerRewardExpBar : MonoBehaviour
    {
        public UITweenCanvasGroup CanvasTween { get { return m_CanvasTween; } }

        [SerializeField] private Text m_textExp;
        [SerializeField] private Image m_imgFill;
        private RankData m_currentRank;

        public void Initialize(int startAmount, int targetAmount)
        {
        }

        private void Awake()
        {
            m_CanvasTween = this.GetComponent<UITweenCanvasGroup>();
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }

        private UITweenCanvasGroup m_CanvasTween;
    }
}