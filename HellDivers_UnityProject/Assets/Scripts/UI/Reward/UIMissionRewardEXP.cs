using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    [RequireComponent(typeof(UITweenCanvasGroup))]
    public class UIMissionRewardEXP : MonoBehaviour
    {
        public UITweenCanvasGroup CanvasTween { get { return m_CanvasTween; } }

        [SerializeField] private Text m_EXPText;
        private UITweenCanvasGroup m_CanvasTween;
        private int m_iNumber;
        private int m_iCurrentNum;
        private int m_iEvulateRate;

        private void Awake()
        {
            m_CanvasTween = this.GetComponent<UITweenCanvasGroup>();
            m_EXPText.text = string.Format("Exp {0}", m_iCurrentNum);
        }

        public void DrawUI()
        {
            m_CanvasTween.Play();
        }

        public void AddExp(int num)
        {
            StopAllCoroutines();
            m_iNumber += num;
            m_iEvulateRate = Mathf.FloorToInt((num - m_iCurrentNum) * 0.1f);
            if (m_iEvulateRate < 1) m_iEvulateRate = 1;
            m_EXPText.text = string.Format("Exp {0}", m_iCurrentNum);

            StartCoroutine(EvulateNum());
        }

        private IEnumerator EvulateNum()
        {
            while (m_iCurrentNum < m_iNumber)
            {
                m_iCurrentNum += m_iEvulateRate;
                if (m_iCurrentNum > m_iNumber) m_iCurrentNum = m_iNumber;
                m_EXPText.text = string.Format("Exp {0}", m_iCurrentNum);
                yield return null;
            }
        }
    }
}