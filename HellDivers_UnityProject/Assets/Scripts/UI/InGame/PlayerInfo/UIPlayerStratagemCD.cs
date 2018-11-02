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
        LoadIcon();
    }

    public void RefreshDisplay()
    {
        this.gameObject.SetActive(m_CurrentStratagem.IsCooling);
        m_Fill.fillAmount = m_CurrentStratagem.CoolTimer / m_CurrentStratagem.Info.CoolDown;
    }

    private void LoadIcon()
    {
        Sprite iconImg = null;
        string imgName = string.Format("icon_{0}", m_CurrentStratagem.Info.ID);
        string imgPath = "UI/Resource/Icons/Stratagem";
        string fullPath = imgPath + "/" + imgName;

        if (AssetManager.m_Instance != null)
        {
            iconImg = AssetManager.m_Instance.GetAsset(typeof(Sprite), imgName, imgPath) as Sprite;
            if (iconImg == null)
            {
                iconImg = Resources.Load<Sprite>(fullPath);
                AssetManager.m_Instance.AddAsset(typeof(Sprite), imgName, imgPath, iconImg);
            }
        }
        else
        {
            iconImg = Resources.Load<Sprite>(fullPath);
        }

        m_Icon.sprite = iconImg;
        m_Fill.sprite = iconImg;
    }
}