using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Turret : Weapon {

    // Use this for initialization
    public override WeaponInfo weaponInfo { get { return _weaponInfo;  } }
    protected override int activeAmmo　{　get　{　return base.activeAmmo - 5;　}　}

    public override void Shot(Vector3 pos, Vector3 vec, float spread, ref float damage)
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponInfo.ID);
        if (go != null)
        {
            go.transform.position = pos;
            go.transform.forward = vec;
            go.transform.Rotate(0, Random.Range(-weaponInfo.Min_Spread, weaponInfo.Min_Spread), 0);
            go.SetActive(true);

        }
        else { Debug.Log("Something's wrong"); }
    }

}
