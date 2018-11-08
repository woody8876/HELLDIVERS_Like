using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Weapon : MonoBehaviour {

    public int m_ID;
    [SerializeField] Text m_WeaponName;
    [SerializeField] Image m_WeaponTexture;

    WeaponInfo m_WeaponInfo;
    Sprite m_Sprite;
	// Use this for initialization
	void Start () {
        m_WeaponInfo = new WeaponInfo();
        m_Sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite),HELLDIVERS.UI.UIHelper.WeaponIconFolder, "icon_" + m_ID, false);
        //m_Sprite
        //m_WeaponName.text = m_WeaponInfo.Name;
        //m_WeaponTexture.sprite = 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
