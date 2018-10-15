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
    Explosive,
    Laser,
    Anti_Tank,
    LastOne };

public interface IWeaponBehaviour
{
    WeaponInfo weaponInfo { get; }
    void Init(int weaponType);
    void Shot(Vector3 pos, Vector3 vec, float spread, ref float damage);
    void Reload();
}


public class WeaponFactory {

    public IWeaponBehaviour CreateWeapon(eWeaponType eType, int weaponID)
    {
        IWeaponBehaviour weaponBehaviour;
        switch (eType)
        {
            case eWeaponType.Assault_Rifles:
                weaponBehaviour = new Weapon_Rifle();
                break;
            case eWeaponType.Shotguns:
                weaponBehaviour = new Weapon_ShotGun();
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



