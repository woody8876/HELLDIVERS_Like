using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_FireGun : Weapon
{
    public override void Shot(Transform t, float fSpreadperShot)
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponInfo.ID) ?? GameObject.Find("Bullet/Bullet_FireGun(Clone)");

        float fCurSpread = weaponInfo.Min_Spread + fSpreadperShot;
        if (fCurSpread > weaponInfo.Max_Spread) fCurSpread = weaponInfo.Max_Spread;
        if (go != null)
        {
            go.transform.position = t.position;
            go.transform.forward = t.forward;
            go.GetComponent<Bullet_Ray>().StartPos = t;
            go.GetComponent<Bullet_Ray>().m_bActive = true;
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
