using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStratagemAct : MonoBehaviour
{
    public Stratagem CurrentStratagem { get { return m_CurrentStratagem; } }
    private Stratagem m_CurrentStratagem;

    [SerializeField] private Image m_Icon;
    [SerializeField] private Text m_TimeText;

    public void Initialize(Stratagem stratagem)
    {
        m_CurrentStratagem = stratagem;

        string fileName = string.Format("icon_{0}", m_CurrentStratagem.Info.ID);
        m_Icon.sprite = UIHelper.LoadSprite(UIHelper.StratagemIconFolder, fileName);

        m_CurrentStratagem.OnStartActivation += Draw;
        m_CurrentStratagem.OnActivation += UpdateTimer;
        m_CurrentStratagem.OnEndActivation += Hide;
    }

    private void OnDestroy()
    {
        m_CurrentStratagem.OnStartActivation -= Draw;
        m_CurrentStratagem.OnActivation -= UpdateTimer;
        m_CurrentStratagem.OnEndActivation -= Hide;
    }

    public void Draw()
    {
        this.transform.SetAsLastSibling();
        this.gameObject.SetActive(true);
    }

    public void UpdateTimer()
    {
        string minutes = Mathf.Floor(m_CurrentStratagem.ActTimer / 60).ToString("00");
        string seconds = (m_CurrentStratagem.ActTimer % 60).ToString("00");
        string minisec = Mathf.Floor((m_CurrentStratagem.ActTimer * 100) % 100).ToString("00");

        m_TimeText.text = string.Format("{0}:{1}:{2}", minutes, seconds, minisec);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}