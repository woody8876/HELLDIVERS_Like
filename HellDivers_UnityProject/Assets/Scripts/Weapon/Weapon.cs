///2018.09.10
///Ivan.CC
///
/// Weapon's data.
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : IWeaponBehaviour{
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
    #endregion


    List<GameObject> _currentActive = new List<GameObject>();
    public List<GameObject> m_Weapon_CurrentActives
    {
        set { _currentActive = value; }
        get { return _currentActive; }
    }

    #region Behaviours
    public virtual void Shot(Vector3 pos, Vector3 vec) { }
    public virtual void Reload() { }
    #endregion



}
