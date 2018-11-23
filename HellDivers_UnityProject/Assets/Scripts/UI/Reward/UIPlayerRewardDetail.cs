using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    [RequireComponent(typeof(UITweenCanvasGroup))]
    public class UIPlayerRewardDetail : MonoBehaviour
    {
        public UITweenCanvasGroup CanvasTween { get { return m_CanvasTween; } }

        [SerializeField] private Text m_textLabel;
        [SerializeField] private Text m_textNumber;
        private UITweenCanvasGroup m_CanvasTween;
        private int m_iNumber;
        private int m_iCurrentNum;

        public void Initialize(string label, int number)
        {
            m_textLabel.text = label;
            m_iNumber = number;
            m_textNumber.text = m_iNumber.ToString();
        }

        private void Awake()
        {
            m_CanvasTween = this.GetComponent<UITweenCanvasGroup>();
        }
    }
}