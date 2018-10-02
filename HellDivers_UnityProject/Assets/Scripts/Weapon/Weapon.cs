///2018.09.10
///Ivan.CC
///
/// Weapon's data.
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : IWeaponBehaviour
{
    #region WeaponInfo

    public float Damage { set; get; }
    public float Center_Damage { set; get; }
    public float Explosion_Damage { set; get; }
    public float FireRate { set; get; }
    public int Capacity { set; get; }
    public int Start_Mags { set; get; }
    public int Max_Mags { set; get; }
    public float Empty_Reload_Speed { set; get; }
    public float Tactical_Reload_Speed { set; get; }
    public float Min_Spread { set; get; }
    public float Max_Spread { set; get; }
    public float Spread_Increase_per_shot { set; get; }
    public float RANGE { set; get; }
    public float FireMode { set; get; }

    #endregion WeaponInfo

    #region Bullet status

    private List<GameObject> _currentActive = new List<GameObject>();

    public List<GameObject> m_Weapon_CurrentActives
    {
        set { _currentActive = value; }
        get { return _currentActive; }
    }

    protected int _iAmmo;

    public int Ammo
    {
        set { _iAmmo = Capacity; }
        get { return _iAmmo; }
    }

    protected float _fSpread;

    public float Spread
    {
        set { _fSpread = Min_Spread; }
        get { return _fSpread; }
    }

    #endregion Bullet status

    #region Behaviours

    public virtual void Shot(Vector3 pos, Vector3 vec, float spread, ref float damage)
    {
    }

    public virtual void Reload()
    {
    }

    #endregion Behaviours

    protected bool CheckHit(Vector3 pos, Vector3 vec)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(pos, vec, out raycastHit, RANGE, 1 << LayerMask.NameToLayer("Enemy")))
        {
            Debug.Log("Hit");
            return true;
        }
        return false;
    }
}