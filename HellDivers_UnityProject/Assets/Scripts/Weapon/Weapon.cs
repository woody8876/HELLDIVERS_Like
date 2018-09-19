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

    public float Power { set; get; }
    public float FireRate { set; get; }
    public float Stability { set; get; }
    public int Magazine { set; get; }
    public float Range { set; get; }
    public float ReloadTime { set; get; }

    List<GameObject> _currentActive = new List<GameObject>();
    public List<GameObject> m_Weapon_CurrentActives
    {
        set { _currentActive = value; }
        get { return _currentActive; }
    }

    public virtual void Shot(Vector3 pos, Vector3 vec) { }
    public virtual void Reload() { }


}
