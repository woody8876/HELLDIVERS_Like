using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Laser : Weapon
{
    public override void Shot(Transform t, float fSpreadperShot, Player player)
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponInfo.ID) ?? GameObject.Find("Bullet/Bullet_Laser(Clone)");

        float fCurSpread = weaponInfo.Min_Spread + fSpreadperShot;
        if (fCurSpread > weaponInfo.Max_Spread) fCurSpread = weaponInfo.Max_Spread;
        if (go != null)
        {
            go.transform.position = t.position;
            go.transform.forward = t.forward;
            go.GetComponent<Bullet_Ray>().SetParent(t);
            go.GetComponent<Bullet_Ray>().SetBool(true);
            go.GetComponent<Bullet_Ray>().SetPlayer(player);
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
