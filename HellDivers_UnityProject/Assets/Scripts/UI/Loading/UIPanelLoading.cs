using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanelLoading : MonoBehaviour
{
    [SerializeField] private TweenAlpha m_DlackCardTween;
    [SerializeField] private Image m_Background;
    [SerializeField] private Image m_LoadingBar;
    [SerializeField] private Image m_LoadingFill;
    [SerializeField] private Text m_TipText;
    private CanvasGroup m_CanvasGroup;
    private float m_targetAlpha;
    private float m_alphaLerp = 0.05f;
    private float m_targetFillAmount;

    private delegate void DelegateDoState();

    private DelegateDoState DoFade;

    private void Awake()
    {
        m_CanvasGroup = this.GetComponent<CanvasGroup>();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (DoFade != null) DoFade();
    }

    [ContextMenu("Fade In")]
    public void FadeIn()
    {
        this.gameObject.SetActive(true);
        m_CanvasGroup.alpha = 1;
        m_DlackCardTween.Play(1, 0);
        DoFade = null;
    }

    [ContextMenu("Fade Out")]
    public void FadeOut()
    {
        this.gameObject.SetActive(true);
        m_DlackCardTween.Play(0, 1);
        m_targetAlpha = 0;
        DoFade = DoPanelFade;
    }

    private void DoPanelFade()
    {
        m_CanvasGroup.alpha = Mathf.Lerp(m_CanvasGroup.alpha, m_targetAlpha, m_alphaLerp);
        if (m_CanvasGroup.alpha < m_targetAlpha + 0.01f)
        {
            m_CanvasGroup.alpha = m_targetAlpha;
            this.gameObject.SetActive(false);
            DoFade = null;
        }
    }

    public void SetLoadingBarProcess(float amount)
    {
        m_LoadingFill.fillAmount = Mathf.Clamp01(amount);
    }
}