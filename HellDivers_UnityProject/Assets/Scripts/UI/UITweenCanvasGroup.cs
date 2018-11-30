using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UITweenCanvasGroup : MonoBehaviour
    {
        public float TimeLenght { get { return m_time; } }

        [SerializeField] private float m_time = 1;
        [SerializeField] private AnimationCurve m_AnimaCurve;

        public event UIEventHolder OnTweenFinished;

        private void Awake()
        {
            m_Canvas = this.GetComponent<CanvasGroup>();
        }

        [ContextMenu("Play")]
        public void Play()
        {
            StopAllCoroutines();
            StartCoroutine(DoAnimation());
        }

        private IEnumerator DoAnimation()
        {
            float currentTime = 0;
            m_Canvas.alpha = m_AnimaCurve.Evaluate(currentTime);

            while (currentTime < 1.0f)
            {
                currentTime += Time.deltaTime * (1 / m_time);
                m_Canvas.alpha = m_AnimaCurve.Evaluate(currentTime);
                yield return null;
            }

            if (OnTweenFinished != null) OnTweenFinished();
        }

        private CanvasGroup m_Canvas;
    }
}