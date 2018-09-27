///2018.09.10
///Ivan.CC
///
/// Weapon ShotGun's behaviour.
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ShotGun : Weapon, IWeaponBehaviour
{

    public override void Shot(Vector3 pos, Vector3 vec, float fSpreadperShot , ref float damage)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)eWeaponType.Shotguns + 100);
            if (go != null)
            {
                go.transform.position = pos;
                go.transform.forward = vec;
                go.transform.Rotate(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                go.SetActive(true);
                _iAmmo--;
                Debug.DrawRay(pos, go.transform.forward, Color.green, 10f);
                if (CheckHit(pos, go.transform.forward))
                {
                    damage = Damage;
                }

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
        _iAmmo++;
    }
}
