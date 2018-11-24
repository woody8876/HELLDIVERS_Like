using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI_Stratagems : MonoBehaviour {

    [SerializeField] Image m_BG;
    [SerializeField] Image m_StratagemTexture;
    [Header("== Private Field ==")]
    [SerializeField] int m_ID;

    public int StratagemID { get { return m_ID; } }

    private Color m_HighLight = new Color(0.788f, 0.635f, 0.133f, 1.0f);
    private Color m_BGColor = new Color(1, 1, 1, 0.286f);
    private Color m_SpritNull = new Color(0,0,0,0);
    private Color m_SpritBright = new Color(1,1,1,1);


    public void SetStratagemUI(int id , bool b = false)
    {
        m_ID = id;
        if (m_ID == 0)
        {
            SetNull();
            this.name = m_ID.ToString();
            return;
        }
        StratagemInfo m_StratagemInfo = GameData.Instance.StratagemTable[m_ID];
        string s = (b)? "_gray" : "";
        Sprite sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), HELLDIVERS.UI.UIHelper.StratagemIconFolder, "icon_" + m_ID + s, false);
        m_StratagemTexture.sprite = sprite;
        m_StratagemTexture.color = m_SpritBright;
        this.name = m_ID.ToString();
    }

    public void SetBG() { m_BG.color = m_BGColor; }

    public void SetHighlightBG() { m_BG.color = m_HighLight; }

    private void SetNull()
    {
        m_StratagemTexture.sprite = null;
        m_StratagemTexture.color = m_SpritNull;
    }
}
