using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackage : InteractiveItem
{
    public override void OnInteract(Player player)
    {
        int[] ids = player.WeaponController.ActivedWeaponID;
        bool bFillUp = false;
        for (int i = 0; i < ids.Length; i++)
        {
            int currentWeaponId = player.WeaponController.ActivedWeaponID[i];
            int num = player.WeaponController.ActiveWeapon[currentWeaponId].weaponInfo.Start_Mags;
            bool bCurrentFillUp = player.WeaponController.AddMags(currentWeaponId, num);
            if (bCurrentFillUp) bFillUp = true;
        }
        if (bFillUp == false) return;

        if (ObjectPool.m_Instance != null)
        {
            int type = int.Parse(ID);
            ObjectPool.m_Instance.UnLoadObjectToPool(type, this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}