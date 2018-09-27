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
    void Shot(Vector3 pos, Vector3 vec, float spread, ref float damage);
    void Reload();
}


public class WeaponFactory {

    public IWeaponBehaviour CreateWeapon(eWeaponType eType)
    {
        IWeaponBehaviour weaponBehaviour;
        Weapon m_weaponData = GameData.Instance.WeaponInfoTable[(int)eType];
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

        weaponBehaviour.Damage = m_weaponData.Damage;
        weaponBehaviour.Center_Damage = m_weaponData.Center_Damage;
        weaponBehaviour.Explosion_Damage = m_weaponData.Explosion_Damage;
        weaponBehaviour.FireRate = 1/(m_weaponData.FireRate * 0.017f);
        weaponBehaviour.Capacity = m_weaponData.Capacity;
        weaponBehaviour.Start_Mags = m_weaponData.Start_Mags;
        weaponBehaviour.Max_Mags = m_weaponData.Max_Mags;
        weaponBehaviour.Empty_Reload_Speed = m_weaponData.Empty_Reload_Speed;
        weaponBehaviour.Tactical_Reload_Speed = m_weaponData.Tactical_Reload_Speed;
        weaponBehaviour.Min_Spread = m_weaponData.Min_Spread;
        weaponBehaviour.Max_Spread = m_weaponData.Max_Spread;
        weaponBehaviour.Spread_Increase_per_shot = m_weaponData.Spread_Increase_per_shot;
        weaponBehaviour.RANGE = m_weaponData.RANGE;
        weaponBehaviour.FireMode = m_weaponData.FireMode;

        Vector3 pos = Vector3.zero;
        Vector3 vec = Vector3.forward;
        float damage = 0f;
        weaponBehaviour.Shot( pos, vec, 0f, ref damage);
        weaponBehaviour.Reload();
        return weaponBehaviour;
    }
}



