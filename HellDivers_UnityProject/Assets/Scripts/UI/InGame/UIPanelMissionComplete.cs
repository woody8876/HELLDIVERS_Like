using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HELLDIVERS.UI;

[RequireComponent(typeof(UITweenCanvasGroup))]
public class UIPanelMissionComplete : MonoBehaviour
{
    private UITweenCanvasGroup m_PanelTweener;
    [SerializeField] private UITweenCanvasGroup m_TitleTweener;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        m_PanelTweener = this.GetComponent<UITweenCanvasGroup>();
    }

    public void StartUI()
    {
        StartCoroutine(OnStartUI());
    }

    private IEnumerator OnStartUI()
    {
        yield return null;
        m_PanelTweener.Play();
        yield return new WaitForSeconds(m_PanelTweener.TimeLenght);
        m_TitleTweener.Play();
        yield break;
    }
}