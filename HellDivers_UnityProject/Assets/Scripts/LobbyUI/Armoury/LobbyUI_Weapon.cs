using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbyUI_Weapon : MonoBehaviour{

    public int m_ID;
    public int Type { get { return GameData.Instance.WeaponInfoTable[m_ID].Type; } }
    /// <summary>
    /// if true than this weaponUI is primary weapon;
    /// </summary>
    public bool m_Primary;

//    [HideInInspector] public WeaponInfo m_WeaponInfo = new WeaponInfo();
    [SerializeField] Image m_WeaponTexture;
    [SerializeField] Text m_WeaponName;

    public void SetWeaponUI () {
        WeaponInfo info = GameData.Instance.WeaponInfoTable[m_ID];
        Sprite sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite),HELLDIVERS.UI.UIHelper.WeaponIconFolder, "icon_" + info.Image, false);
        m_WeaponName.text = info.Name;
        m_WeaponTexture.sprite = sprite;
	}

}
