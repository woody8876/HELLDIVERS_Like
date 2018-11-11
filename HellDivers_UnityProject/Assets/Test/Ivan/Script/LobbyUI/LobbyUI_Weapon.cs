using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbyUI_Weapon : MonoBehaviour{

    public int m_ID;
    /// <summary>
    /// if true than this weaponUI is primary weapon;
    /// </summary>
    public bool m_Primary;

    [HideInInspector] public WeaponInfo m_WeaponInfo;
    [HideInInspector] public Sprite m_Sprite;
    [SerializeField] Image m_WeaponTexture;
    [SerializeField] Text m_WeaponName;

    public void SetWeaponUI () {
        m_WeaponInfo = GameData.Instance.WeaponInfoTable[m_ID];
        m_Sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite),HELLDIVERS.UI.UIHelper.WeaponIconFolder, "icon_" + m_ID, false);
        m_WeaponName.text = m_WeaponInfo.Name;
        m_WeaponTexture.sprite = m_Sprite;
	}

}
