using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStratagemCD : MonoBehaviour
{
    public Stratagem CurrentStratagem { get { return m_CurrentStratagem; } }
    private Stratagem m_CurrentStratagem;
    [SerializeField] private Image m_Icon;
    [SerializeField] private Image m_Fill;

    public void Initialize(Stratagem stratagem)
    {
        m_CurrentStratagem = stratagem;

        string fileName = string.Format("icon_{0}", m_CurrentStratagem.Info.ID);
        m_Fill.sprite = UIHelper.LoadSprite(UIHelper.StratagemIconFolder, fileName);

        fileName = string.Format("icon_{0}_gray", m_CurrentStratagem.Info.ID);
        m_Icon.sprite = UIHelper.LoadSprite(UIHelper.StratagemIconFolder, fileName);

        m_CurrentStratagem.OnStartCoolDown += DrawDisplay;
        m_CurrentStratagem.OnCoolDown += RefreshDisplay;
        m_CurrentStratagem.OnEndCoolDown += HideDisplay;
    }

    private void OnDestroy()
    {
        if (m_CurrentStratagem != null)
        {
            m_CurrentStratagem.OnStartCoolDown -= DrawDisplay;
            m_CurrentStratagem.OnCoolDown -= RefreshDisplay;
            m_CurrentStratagem.OnEndCoolDown -= HideDisplay;
        }
    }

    public void DrawDisplay()
    {
        this.transform.SetAsLastSibling();
        RefreshDisplay();
        this.gameObject.SetActive(true);
    }

    public void RefreshDisplay()
    {
        m_Fill.fillAmount = m_CurrentStratagem.CoolTimer / m_CurrentStratagem.Info.CoolDown;
    }

    public void HideDisplay()
    {
        this.gameObject.SetActive(false);
    }
}