using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_FireGun : Weapon
{
    public override void Shot(Transform t, float fSpreadperShot, Player player)
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponInfo.ID);
        if (go == null) return;
        float fCurSpread = weaponInfo.Min_Spread + fSpreadperShot;
        if (fCurSpread > weaponInfo.Max_Spread) fCurSpread = weaponInfo.Max_Spread;
        go.transform.position = t.position;
        go.transform.forward = t.forward;
        go.GetComponent<Bullet_Ray>().SetParent(t);
        go.GetComponent<Bullet_Ray>().SetPlayer(player);
        go.GetComponent<Bullet_Ray>().SetBool(true);
        go.transform.Rotate(0, Random.Range(-fCurSpread, fCurSpread), 0);
        go.SetActive(true);
        weaponInfo.Ammo--;

    }
    public override void Reload()
    {
        weaponInfo.Ammo = weaponInfo.Capacity;
        weaponInfo.Mags--;
    }

}
