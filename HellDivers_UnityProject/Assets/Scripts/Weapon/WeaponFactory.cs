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

    float Damage { set; get; }
    float Center_Damage { set; get; }
    float Explosion_Damage { set; get; }
    float FireRate { set; get; }
    int Capacity { set; get; }
    int Ammo { set; get; }
    int Start_Mags { set; get; }
    int Max_Mags { set; get; }
    float Empty_Reload_Speed { set; get; }
    float Tactical_Reload_Speed { set; get; }
    float Min_Spread { set; get; }
    float Max_Spread { set; get; }
    float Spread_Increase_per_shot { set; get; }
    float RANGE { set; get; }
    float FireMode { set; get; }
    List<GameObject> m_Weapon_CurrentActives { set; get; }
    void Shot(Vector3 pos, Vector3 vec);
    void Reload();
}


public class WeaponFactory {

    IWeaponBehaviour weaponActive;

    public IWeaponBehaviour CreateWeapon(eWeaponType eType)
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
                weaponBehaviour = null;
                break;
        }

        Vector3 pos = Vector3.zero;
        Vector3 vec = Vector3.forward;
        weaponBehaviour.Shot( pos, vec);
        weaponBehaviour.Reload();
        return weaponBehaviour;
    }
}



