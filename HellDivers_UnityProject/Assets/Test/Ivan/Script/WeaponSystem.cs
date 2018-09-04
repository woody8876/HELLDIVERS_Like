///2018.09.02
/// Ivan.C
///
/// Define weapons' type and their attributes (Ammo, Damage, RefillTime, CoolTime, Level)
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//步槍，留彈槍，散彈槍，雷社，火箭筒，治療槍
public enum eWeaponType { FirstOne = -1, Rifle, Grenade, ShotGun, Laser, Rocket, Heal, LastOne };


//傷害，子彈，冷卻(緩衝)，填彈葯時間，
public class WeaponSystem {

    //Attribute
    protected float m_iAmmo;
    protected float m_fDamage;
    protected float m_fRefillTime;
    protected float m_fCoolTime;

    //Weight of LevelUp
    protected float m_iAmmoWt = 0.3f;
    protected float m_fDamageWt = 8.7f;
    protected float m_fRefillTimeWt = 0.01f;
    protected float m_fCoolTimeWt = 0.003f;
    protected float m_fLevelWt = 1;

    //Bullet's worked region    
    protected float mr_fXRatio;
    protected float mr_fYRatio;

    //Storage Weapons' types and attributes 
    protected Dictionary<eWeaponType, List<float>> m_dWeaponInfo;

    //Storage the attributes of weapons
    private List<float> m_lWeaponInfo;

    //=====================================================================================================
    /// <summary>
    /// Initalized the WeaponSystem script.
    /// </summary>
    protected void Initalized()
    {      
        m_dWeaponInfo = new Dictionary<eWeaponType, List<float>>();
        CreatDictionary();
    }
    
    //Set Weapon's Info into  List
    private void GetWeapon(int itype)
    {
        eWeaponType type = (eWeaponType)itype;        
        switch (type)        
        {
            case eWeaponType.Rifle:                
                m_iAmmo = 30;
                m_fDamage = 10.0f;
                m_fRefillTime = 0.8f;
                m_fCoolTime = 0.3f;
                mr_fXRatio = 0;
                mr_fYRatio = 0;
                break;

            case eWeaponType.Grenade:
                m_iAmmo = 15;
                m_fDamage = 50.0f;
                m_fRefillTime = 3.0f;
                m_fCoolTime = 1.2f;
                mr_fXRatio = 0;
                mr_fYRatio = 0;
                break;

            case eWeaponType.ShotGun:
                m_iAmmo = 22;
                m_fDamage = 30.0f;
                m_fRefillTime = 1.5f;
                m_fCoolTime = 1.0f;
                mr_fXRatio = 1.5f;
                mr_fYRatio = 1.5f;
                break;

            case eWeaponType.Laser:
                m_iAmmo = 100;
                m_fDamage = 3.0f;
                m_fRefillTime = 3.0f;
                m_fCoolTime = 0.0f;
                mr_fXRatio = 0;
                mr_fYRatio = 0;
                break;

            case eWeaponType.Rocket:
                m_iAmmo = 4;
                m_fDamage = 150.0f;
                m_fRefillTime = 5.0f;
                m_fCoolTime = 2.0f;
                mr_fXRatio = 0;
                mr_fYRatio = 0;
                break;

            case eWeaponType.Heal:
                m_iAmmo = 8;
                m_fDamage = -10.0f;
                m_fRefillTime = 1.0f;
                m_fCoolTime = 0.5f;
                mr_fXRatio = 0;
                mr_fYRatio = 0;
                break;
            //Out of Range
            default:
                Debug.Log("Type is out of Range!");
                m_iAmmo = 30;
                m_fDamage = 10.0f;
                m_fRefillTime = 0.8f;
                m_fCoolTime = 0.3f;
                mr_fXRatio = 0;
                mr_fYRatio = 0;
                break;
        }
    }

    //Creat weapon dictionary
    private void CreatDictionary()
    {
        for (int i = 0; i < (int)eWeaponType.LastOne; i++)
        {
            GetWeapon(i);
            m_lWeaponInfo = new List<float>() { m_iAmmo, m_fDamage, m_fRefillTime, m_fCoolTime, mr_fXRatio, mr_fXRatio };          
            m_dWeaponInfo.Add((eWeaponType)i, m_lWeaponInfo);
        }
    }
}
