///2018.09.10
///Ivan.CC
///
/// Weapon ShotGun's behaviour.
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ShotGun : Weapon
{
    public override WeaponInfo weaponInfo { get { return _weaponInfo; } }
    protected override int activeAmmo { get { return base.activeAmmo * 10; } }
    public override GameObject WeaponLoader() { return base.WeaponLoader(); }

    public override void Shot(Vector3 pos, Vector3 vec, float fSpreadperShot , ref float damage)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponInfo.ID);
            if (go != null)
            {
                go.transform.position = pos;
                go.transform.forward = vec;
                go.transform.Rotate(
                    Random.Range(-weaponInfo.Min_Spread, weaponInfo.Min_Spread), 
                    Random.Range(-weaponInfo.Min_Spread, weaponInfo.Min_Spread), 
                    Random.Range(-weaponInfo.Min_Spread, weaponInfo.Min_Spread));
                go.SetActive(true);
                weaponInfo.Ammo--;
                Debug.DrawRay(pos, go.transform.forward * 10, Color.green, 1f);
                if (CheckHit(pos, go.transform.forward))
                {
                    damage = weaponInfo.Damage;
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
        int i = (int)((weaponInfo.Capacity - weaponInfo.Ammo) * 0.1f);
        weaponInfo.Ammo = weaponInfo.Capacity;
        weaponInfo.Mags -= i;
    }
}
