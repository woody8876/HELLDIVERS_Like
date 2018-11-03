using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Precision : Weapon
{
    public override void Shot(Vector3 pos, Vector3 vec, float fSpreadperShot, ref float damage)
    {
        float fCurSpread = weaponInfo.Min_Spread + fSpreadperShot;
        if (fCurSpread > weaponInfo.Max_Spread) fCurSpread = weaponInfo.Max_Spread;
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponInfo.ID);
        if (go != null)
        {
            go.transform.position = pos;
            go.transform.forward = vec;
            go.transform.Rotate(0, Random.Range(-fCurSpread, fCurSpread), 0);
            go.SetActive(true);
            weaponInfo.Ammo--;
        }
        else { Debug.Log("There is no ammo."); }
    }
    public override void Reload()
    {
        weaponInfo.Ammo = weaponInfo.Capacity;
        weaponInfo.Mags--;
    }

}
