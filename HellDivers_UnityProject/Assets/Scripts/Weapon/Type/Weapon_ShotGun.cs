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
    protected override int activeAmmo { get { return base.activeAmmo * 20; } }
    public override void WeaponLoader() {  base.WeaponLoader(); }
    
    public override void Shot(Transform t, float fSpreadperShot, Player player)
    {
        float angle = (weaponInfo.Min_Spread * 2) / weaponInfo.Explosion_Damage;
        for (int i = 0; i < weaponInfo.Explosion_Damage; i++)
        {
            GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponInfo.ID);
            if (go != null)
            {
                go.transform.position = t.position;
                go.transform.forward = t.forward;
                go.transform.Rotate(0, -weaponInfo.Min_Spread + i * angle, 0);
                go.GetComponent<Bullet>().SetPlayer(player);
                go.GetComponent<Bullet>().SetID(weaponInfo.ID);
                go.SetActive(true);
                weaponInfo.Ammo--;
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
        weaponInfo.Ammo += (int)weaponInfo.Explosion_Damage;
        weaponInfo.Mags -= 1;
    }
}
