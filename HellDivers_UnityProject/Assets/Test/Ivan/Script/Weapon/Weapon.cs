///2018.09.10
///Ivan.CC
///
/// Weapon's data.
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon  {

    public int m_iAmmo;
    public float m_fDamage;
    public float m_fCoolTime;
    public float m_fReloadTime;

    public List<GameObject> m_Weapon_CurrentActives = new List<GameObject>();

}
