using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenAlpha : MonoBehaviour
{
    public Image m_Image;
    public float m_StratAlpha = 0;
    public float m_TargetAlpha = 1;
    public float m_DeadZone = 0.01f;
    public float m_Lerp = 0.025f;

    private float m_CurrentAlpha;
    private float m_origin;
    private float m_target;

    public delegate void TweenAlphaEventHolder();

    public event TweenAlphaEventHolder OnBegining;

    public event TweenAlphaEventHolder OnFinished;

    [ContextMenu("Play Forward")]
    public void PlayForward()
    {
        Play(m_StratAlpha, m_TargetAlpha);
    }

    [ContextMenu("Play Backward")]
    public void PlayeBackward()
    {
        Play(m_TargetAlpha, m_StratAlpha);
    }

    public void Play(float start, float end)
    {
        this.gameObject.SetActive(true);
        m_CurrentAlpha = start;
        m_origin = start;
        m_target = end;
        this.enabled = true;
        if (OnBegining != null) OnBegining();
    }

    private void Awake()
    {
        if (m_Image == null) m_Image = this.GetComponent<Image>();
        this.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        DoFade();
    }

    private void DoFade()
    {
        m_CurrentAlpha = Mathf.Lerp(m_CurrentAlpha, m_target, m_Lerp);
        SetAlpha(m_CurrentAlpha);

        if (m_origin > m_target && m_CurrentAlpha <= m_target + m_DeadZone)
        {
            SetAlpha(m_target);
            if (OnFinished != null) OnFinished();
            this.enabled = false;
        }
        else if (m_origin < m_target && m_CurrentAlpha >= m_target - m_DeadZone)
        {
            SetAlpha(m_target);
            if (OnFinished != null) OnFinished();
            this.enabled = false;
        }
    }

    private void SetAlpha(float alpha)
    {
        Color currentColor = m_Image.color;
        currentColor.a = alpha;
        m_Image.color = currentColor;
        if (currentColor.a <= 0.0f) this.gameObject.SetActive(false);
    }
}