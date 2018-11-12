using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI_Stratagems : MonoBehaviour {


    public int m_ID;

    [HideInInspector] public StratagemInfo m_StratagemInfo;
    [HideInInspector] public Sprite m_Sprite;
    [SerializeField] Image m_WeaponTexture;

    public void SetStratagemUI(bool b = false)
    {
        m_StratagemInfo = GameData.Instance.StratagemTable[m_ID];
        string s = (b)? "_gray" : "";
        m_Sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), HELLDIVERS.UI.UIHelper.StratagemIconFolder, "icon_" + m_ID + s, false);
        m_WeaponTexture.sprite = m_Sprite;
    }



}
