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
    Rifle,
    Grenade,
    ShotGun,
    Laser,
    Rocket,
    Heal,
    LastOne };

public interface IWeaponBehaviour
{
    float Power { set; get; }
    float FireRate { set; get; }
    float Stability { set; get; }
    int Magazine { set; get; }
    float Range { set; get; }
    float ReloadTime { set; get; }
    List<GameObject> m_Weapon_CurrentActives { set; get; }
    void Shot(Vector3 pos, Vector3 vec);
    void Reload();
}


public class WeaponFactory {

    IWeaponBehaviour weaponActive;

    public IWeaponBehaviour CreateWeapon(eWeaponType eType)
    {
        IWeaponBehaviour weaponBehaviour;
        if (eWeaponType.ShotGun == eType) { weaponBehaviour = new Weapon_ShotGun(); }
        else if (eWeaponType.Rifle == eType) { weaponBehaviour = new Weapon_Rifle(); }
        else { weaponBehaviour = null; }

        Vector3 pos = Vector3.zero;
        Vector3 vec = Vector3.forward;
        weaponBehaviour.Shot( pos, vec);
        weaponBehaviour.Reload();
        return weaponBehaviour;
    }
}



