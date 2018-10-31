using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackage : InteractiveItem
{
    public int WeaponID { get { return m_WeaponID; } }
    public int Ammo { get { return m_Ammo; } }
    [SerializeField] private int m_WeaponID;
    [SerializeField] private int m_Ammo;

    public void Initialize(int weaponId)
    {
        m_WeaponID = weaponId;
    }

    public override void OnInteract(Player player)
    {
        m_WeaponID = player.WaeponController._CurrentWeapon;

        bool bFillUp = player.WaeponController.AddMags(WeaponID, Ammo);
        if (bFillUp == false) return;

        Destroy(this.gameObject);
    }
}