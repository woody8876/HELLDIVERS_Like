using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UITweenCanvasGroup : MonoBehaviour
    {
        #region Property

        /// <summary>
        /// Tween progress time lenght.
        /// </summary>
        public float TimeLenght
        {
            get
            { return m_time; }
            set
            {
                if (value < 0) m_time = 0;
                else m_time = value;
            }
        }

        #endregion Property

        #region Serialize Field

        [SerializeField] private float m_time = 1;
        [SerializeField] private AnimationCurve m_AnimaCurve;

        #endregion Serialize Field

        #region Events

        /// <summary>
        /// Event on tween animation finished;
        /// </summary>
        public event UIEventHolder OnTweenFinished;

        /// <summary>
        /// Event on tween animation start.
        /// </summary>
        public event UIEventHolder OnTweenStart;

        #endregion Events

        #region MonoBehaviour

        private void Awake()
        {
            this.enabled = false;
            m_Canvas = this.GetComponent<CanvasGroup>();
        }

        #endregion MonoBehaviour

        #region Public Function

        /// <summary>
        /// Play forward, strat at 0 percent of animation curve.
        /// </summary>
        [ContextMenu("Play Forward")]
        public void PlayForward()
        {
            Play(0, 1);
        }

        /// <summary>
        /// Play backward, start at 100 percent of animation curve setting.
        /// </summary>
        [ContextMenu("Play Backward")]
        public void PlayBackward()
        {
            Play(1, 0);
        }

        /// <summary>
        /// Play tween animation by manual setting.
        /// </summary>
        /// <param name="startPercentage">Strat percentage of animation curve setting</param>
        /// <param name="finishPercentage">End percentage of animation curve setting</param>
        public void Play(float startPercentage, float finishPercentage)
        {
            this.enabled = true;
            StopAllCoroutines();
            StartCoroutine(DoAnimation(startPercentage, finishPercentage));
        }

        #endregion Public Function

        #region Progress

        private IEnumerator DoAnimation(float stratTime, float endTime)
        {
            float currentTime = stratTime;
            m_Canvas.alpha = m_AnimaCurve.Evaluate(currentTime);

            if (OnTweenStart != null) OnTweenStart();

            if (stratTime <= endTime)
            {
                while (currentTime <= endTime)
                {
                    currentTime += Time.deltaTime * (1 / m_time);
                    m_Canvas.alpha = m_AnimaCurve.Evaluate(currentTime);
                    yield return null;
                }
            }
            else
            {
                while (currentTime >= endTime)
                {
                    currentTime -= Time.deltaTime * (1 / m_time);
                    m_Canvas.alpha = m_AnimaCurve.Evaluate(currentTime);
                    yield return null;
                }
            }

            if (OnTweenFinished != null) OnTweenFinished();
            this.enabled = false;
        }

        private CanvasGroup m_Canvas;

        #endregion Progress
    }
}