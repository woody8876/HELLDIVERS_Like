using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStratagemInfo : MonoBehaviour
{
    public Stratagem CurrentStratagem { get { return m_CurrentStratagem; } }
    private Stratagem m_CurrentStratagem;
    private List<Image> m_CodeImgs;
    [SerializeField] private Image m_Icon;
    [SerializeField] private Text m_Uses;
    [SerializeField] private Text m_Title;
    [SerializeField] private Transform m_ArrowRoot;
    [SerializeField] private Image m_Arrow;
    [SerializeField] private Color m_LightColor;
    [SerializeField] private Color m_DarkColor;

    public void Initialize(Stratagem stratagem)
    {
        m_CurrentStratagem = stratagem;
        m_Title.text = stratagem.Info.Title;
        m_Icon.sprite = LoadIcon();
        CreateCodesDisplaye();
        UpdateUses();
    }

    public void StartUI()
    {
        UpdateUses();
        this.gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        this.gameObject.SetActive(false);
        m_Title.gameObject.SetActive(false);
        m_ArrowRoot.gameObject.SetActive(true);
        StopCheckCodes();
    }

    public void UpdateUses()
    {
        int count = m_CurrentStratagem.Info.Uses - m_CurrentStratagem.UsesCount;
        m_Uses.text = count.ToString();
    }

    public void StartCheckCodes()
    {
        m_Icon.color = m_LightColor;
        foreach (Image codeImg in m_CodeImgs)
        {
            codeImg.color = m_DarkColor;
        }
    }

    public void CheckingCode(int step)
    {
        m_CodeImgs[step - 1].color = m_LightColor;
    }

    public void StopCheckCodes()
    {
        m_Icon.color = m_DarkColor;
        foreach (Image codeImg in m_CodeImgs)
        {
            codeImg.color = m_DarkColor;
        }
    }

    public void GetReady()
    {
        m_ArrowRoot.gameObject.SetActive(false);
        m_Title.gameObject.SetActive(true);
    }

    private Sprite LoadIcon()
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
        return iconImg;
    }

    private void CreateCodesDisplaye()
    {
        m_CodeImgs = new List<Image>();

        for (int i = 0; i < m_CurrentStratagem.Info.Codes.Length; i++)
        {
            Image codeArrow = Instantiate(m_Arrow, m_ArrowRoot);

            switch (m_CurrentStratagem.Info.Codes[i])
            {
                case StratagemInfo.eCode.Up:
                    codeArrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                    break;

                case StratagemInfo.eCode.Down:
                    codeArrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                    break;

                case StratagemInfo.eCode.Right:
                    codeArrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    break;

                case StratagemInfo.eCode.Left:
                    codeArrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                    break;
            }

            codeArrow.gameObject.SetActive(true);
            m_CodeImgs.Add(codeArrow);
        }
    }
}