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
        m_Fill.sprite = LoadIcon(fileName);

        fileName = string.Format("icon_{0}_gray", m_CurrentStratagem.Info.ID);
        m_Icon.sprite = LoadIcon(fileName);

        stratagem.OnCoolDown += RefreshDisplay;
    }

    public void RefreshDisplay()
    {
        this.gameObject.SetActive(m_CurrentStratagem.IsCooling);
        m_Fill.fillAmount = m_CurrentStratagem.CoolTimer / m_CurrentStratagem.Info.CoolDown;
    }

    private Sprite LoadIcon(string fileName)
    {
        Sprite iconImg = null;
        string imgPath = "UI/Resource/Icons/Stratagem";
        string fullPath = imgPath + "/" + fileName;

        if (AssetManager.m_Instance != null)
        {
            iconImg = AssetManager.m_Instance.GetAsset(typeof(Sprite), fileName, imgPath) as Sprite;
            if (iconImg == null)
            {
                iconImg = Resources.Load<Sprite>(fullPath);
                AssetManager.m_Instance.AddAsset(typeof(Sprite), fileName, imgPath, iconImg);
            }
        }
        else
        {
            iconImg = Resources.Load<Sprite>(fullPath);
        }

        return iconImg;
    }
}