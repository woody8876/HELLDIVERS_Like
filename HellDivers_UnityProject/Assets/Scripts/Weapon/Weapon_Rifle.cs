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

    public override void Shot(Vector3 pos, Vector3 vec, float fSpreadperShot)
    {
        float fCurSpread = Min_Spread + fSpreadperShot;
        if (fCurSpread > Max_Spread) fCurSpread = Max_Spread;
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)eWeaponType.Assault_Rifles + 100);
        if (go != null)
        {
            go.transform.position = pos;
            go.transform.forward = vec;
            go.transform.Rotate(0, Random.Range(-fCurSpread, fCurSpread), 0);
            go.SetActive(true);
            _iAmmo--;
        }
        else { Debug.Log("There is no ammo."); }
        
    }
    public override void Reload()
    {
        _iAmmo = Capacity;
    }
}
