///2018.09.10
///Ivan.CC
///
/// Weapon Rifle's behaviour.
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Rifle : Weapon
{
    public override WeaponInfo weaponInfo { get { return _weaponInfo; } }

    public override void Shot(Vector3 pos, Vector3 vec, float fSpreadperShot,ref float damage)
    {
        float fCurSpread = _weaponInfo.Min_Spread + fSpreadperShot;
        if (fCurSpread > _weaponInfo.Max_Spread) fCurSpread = _weaponInfo.Max_Spread;
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponInfo.ID);
        if (go != null)
        {
            go.transform.position = pos;
            go.transform.forward = vec;
            go.transform.Rotate(0, Random.Range(-fCurSpread, fCurSpread), 0);
            go.SetActive(true);
            weaponInfo.Ammo--;
            Debug.DrawRay(pos, go.transform.forward, Color.green, 10f);
            if (CheckHit(pos, go.transform.forward)) { damage = _weaponInfo.Damage; }
            else { damage = 0; }

        }
        else { Debug.Log("There is no ammo."); }
    }
    public override void Reload()
    {
        weaponInfo.Ammo = weaponInfo.Capacity;
        weaponInfo.Mags--;
    }
}
