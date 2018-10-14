///2018.09.10
///Ivan.CC
///
/// Weapon's data.
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo
{
    #region Stauts for setting
    public float _Damage;
    public float _Center_Damage;
    public float _Explosion_Damage;
    public float _FireRate;
    public int _Capacity;
    public int _Start_Mags;
    public int _Max_Mags;
    public float _Tactical_Reload_Speed;
    public float _Empty_Reload_Speed;
    public float _Min_Spread;
    public float _Max_Spread;
    public float _Range;
    public float _Spread_Increase_per_shot;
    public float _FireMode;
    #endregion

    #region Status get only
    public float Damage { get { return _Damage; } }
    public float Center_Damage { get { return _Center_Damage; } }
    public float Explosion_Damage { get { return _Explosion_Damage; } }
    public float FireRate { get { return 1/(_FireRate * 0.017f); } }
    public int Capacity { get { return _Capacity; } }
    public int Start_Mags { get { return _Start_Mags; } }
    public int Max_Mags { get { return _Max_Mags; } }
    public float Empty_Reload_Speed { get { return _Empty_Reload_Speed; } }
    public float Tactical_Reload_Speed { get { return _Tactical_Reload_Speed; } }
    public float Min_Spread { get { return _Min_Spread; } }
    public float Max_Spread { get { return _Max_Spread; } }
    public float Spread_Increase_per_shot { get { return _Spread_Increase_per_shot; } }
    public float Range { get { return _Range; } }
    public float FireMode { get { return _FireMode; } }

    #endregion

    private int m_Ammo;
    public int Ammo
    {
        get { return m_Ammo; }
        set
        {
            if (value > Capacity) m_Ammo = Capacity;
            else if (value < 0) m_Ammo = 0;
            else m_Ammo = value;
        }
    }
    private int m_Mags;
    public int Mags
    {
        get { return m_Mags; }
        set
        {
            if (value > Max_Mags) m_Mags = Max_Mags;
            else if (value < 0) m_Mags = 0;
            else m_Mags = value;
        }
    }
}

[System.Serializable]
public class Weapon : IWeaponBehaviour{

    protected WeaponInfo _weaponInfo;

    #region Behaviours
    public virtual WeaponInfo weaponInfo() { return _weaponInfo; }
    public virtual void Init(int weaponID) { _weaponInfo = GameData.Instance.WeaponInfoTable[(int)weaponID]; }
    public virtual void Shot(Vector3 pos, Vector3 vec, float spread, ref float damage) {  }
    public virtual void Reload() { }
    #endregion


    protected bool CheckHit(Vector3 pos, Vector3 vec)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(pos, vec, out raycastHit, _weaponInfo.Range, 1 << LayerMask.NameToLayer("Enemy")))
        {
            Debug.Log("Hit");
            return true;
        }
        return false;
    }
}