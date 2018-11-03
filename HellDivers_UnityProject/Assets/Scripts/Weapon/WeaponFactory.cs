///2018.09.10
///Ivan.CC
///
/// Weapon type, Weapon behaviour, Weapon Factory.
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//步槍，留彈槍，散彈槍，雷社，火箭筒，治療槍
public enum eWeaponType { FirstOne = -1,
    Sidearms,
    Assault_Rifles,
    LMGs,
    Shotguns,
    SMGs,
    Precision,
    FireGun,
    Laser,
    Anti_Tank,
    Turret,
    LastOne };

public interface IWeaponBehaviour
{
    WeaponInfo weaponInfo { get; }
    GameObject WeaponLoader();
    void Init(int weaponType);
    void Shot(Vector3 pos, Vector3 vec, float spread, ref float damage);
    void Reload();

}


public class WeaponFactory {

    public IWeaponBehaviour CreateWeapon(int weaponID)
    {
        int iType = GameData.Instance.WeaponInfoTable[weaponID].Type;
        IWeaponBehaviour weaponBehaviour;
        switch ((eWeaponType)iType)
        {
            case eWeaponType.Sidearms:
                weaponBehaviour = new Weapon_Sidearms();
                break;
            case eWeaponType.Assault_Rifles:
                weaponBehaviour = new Weapon_Rifle();
                break;
            case eWeaponType.LMGs:
                weaponBehaviour = new Weapon_LMGs();
                break;
            case eWeaponType.Shotguns:
                weaponBehaviour = new Weapon_ShotGun();
                break;
            case eWeaponType.SMGs:
                weaponBehaviour = new Weapon_SMGs();
                break;
            case eWeaponType.Precision:
                weaponBehaviour = new Weapon_Precision();
                break;
            case eWeaponType.FireGun:
                weaponBehaviour = new Weapon_FireGun();
                break;
            case eWeaponType.Laser:
                weaponBehaviour = new Weapon_Laser();
                break;
            case eWeaponType.Anti_Tank:
                weaponBehaviour = new Weapon_Anti_Tank();
                break;
            case eWeaponType.Turret:
                weaponBehaviour = new Weapon_Turret();
                break;
            default:
                weaponBehaviour = new Weapon_Rifle();
                break;
        }

        #region WeaponBehaviour 
        float damage = 0f;
        weaponBehaviour.Init(weaponID);
        weaponBehaviour.Shot(Vector3.zero, Vector3.forward, 0f, ref damage);
        weaponBehaviour.Reload();
        #endregion

        return weaponBehaviour;
    }
}



