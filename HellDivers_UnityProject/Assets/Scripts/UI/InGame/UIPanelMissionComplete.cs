using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HELLDIVERS.UI;

public class UIPanelMissionComplete : MonoBehaviour
{
    public float TimeLenght { get { return m_BackgroundTweener.TimeLenght + m_TitleDelay + m_TitleTweener.TimeLenght + m_OverlayDelay + m_OverlayTweener.TimeLenght; } }

    [SerializeField] private UITweenCanvasGroup m_BackgroundTweener;
    [SerializeField] private float m_TitleDelay;
    [SerializeField] private UITweenCanvasGroup m_TitleTweener;
    [SerializeField] private float m_OverlayDelay;
    [SerializeField] private UITweenCanvasGroup m_OverlayTweener;

    public void StartUI()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(OnStartUI());
    }

    private IEnumerator OnStartUI()
    {
        m_BackgroundTweener.Play();

        yield return new WaitForSeconds(m_BackgroundTweener.TimeLenght * m_TitleDelay);

        m_TitleTweener.Play();

        yield return new WaitForSeconds(m_TitleTweener.TimeLenght + m_OverlayDelay);

        m_OverlayTweener.Play();

        yield break;
    }
}