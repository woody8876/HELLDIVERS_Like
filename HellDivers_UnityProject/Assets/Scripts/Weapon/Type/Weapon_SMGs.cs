using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SMGs : Weapon
{
    public override void Shot(Transform t, float fSpreadperShot, Player player)
    {
        float fCurSpread = weaponInfo.Min_Spread + fSpreadperShot;
        if (fCurSpread > weaponInfo.Max_Spread) fCurSpread = weaponInfo.Max_Spread;
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponInfo.ID);
        if (go != null)
        {
            go.transform.position = t.position;
            go.transform.forward = t.forward;
            go.transform.Rotate(0, Random.Range(-fCurSpread, fCurSpread), 0);
            go.GetComponent<Bullet>().m_BulletPlayer = player;
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
