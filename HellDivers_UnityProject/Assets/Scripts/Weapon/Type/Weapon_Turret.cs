using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Turret : Weapon {

    protected override int activeAmmo { get { return base.activeAmmo - 10; } }
    public override void WeaponLoader() { base.WeaponLoader(); }

    public override void Shot(Transform t, float spread)
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponInfo.ID);
        if (go != null)
        {
            go.transform.position = t.position;
            go.transform.forward = t.forward;
            go.transform.Rotate(0, Random.Range(-weaponInfo.Min_Spread, weaponInfo.Min_Spread), 0);
            go.SetActive(true);

        }
        else { Debug.Log("Something's wrong"); }
    }

}
