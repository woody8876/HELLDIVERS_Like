using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Weapon : MonoBehaviour {

    public int m_ID;
    [SerializeField] Text m_WeaponName;
    [SerializeField] Image m_WeaponTexture;

    [HideInInspector] public WeaponInfo m_WeaponInfo;
    [HideInInspector] public Sprite m_Sprite;
    // Use this for initialization
    public void Start()
    {
//        SetWeaponUI();
    }

    public void SetWeaponUI () {
        m_WeaponInfo = GameData.Instance.WeaponInfoTable[m_ID];
        m_Sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite),HELLDIVERS.UI.UIHelper.WeaponIconFolder, "icon_" + m_ID, false);
        m_WeaponName.text = m_WeaponInfo.Name;
        Debug.Log(m_WeaponInfo.Name);
        m_WeaponTexture.sprite = m_Sprite;
	}
	
    public void SetWeaponInfo(UI_WeaponInfo uI_WeaponInfo)
    {
        uI_WeaponInfo.SetWeapon(this);
    }
}
