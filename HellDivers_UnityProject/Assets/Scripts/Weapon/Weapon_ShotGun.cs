///2018.09.10
///Ivan.CC
///
/// Weapon ShotGun's behaviour.
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ShotGun : Weapon, IWeaponBehaviour
{
    public override WeaponInfo weaponInfo()
    {
        _weaponInfo = GameData.Instance.WeaponInfoTable[(int)eWeaponType.Shotguns];
        return _weaponInfo;
    }

    public override void Shot(Vector3 pos, Vector3 vec, float fSpreadperShot , ref float damage)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)eWeaponType.Shotguns + 100);
            if (go != null)
            {
                go.transform.position = pos;
                go.transform.forward = vec;
                go.transform.Rotate(
                    Random.Range(-weaponInfo().Min_Spread, weaponInfo().Min_Spread), 
                    Random.Range(-weaponInfo().Min_Spread, weaponInfo().Min_Spread), 
                    Random.Range(-weaponInfo().Min_Spread, weaponInfo().Min_Spread));
                go.SetActive(true);
                weaponInfo().Ammo--;
                Debug.DrawRay(pos, go.transform.forward, Color.green, 10f);
                if (CheckHit(pos, go.transform.forward))
                {
                    damage = weaponInfo().Damage;
                }

            }
            else
            {
                Debug.Log("There is no ammo.");
                break;
            }
        }
    }

    public override void Reload()
    {
        weaponInfo().Mags--;
    }
}
