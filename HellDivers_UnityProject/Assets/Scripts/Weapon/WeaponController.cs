///2018.09.10
///Ivan.CC
///
/// Weapon's behaviour for player.
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    #region SetWeapon

    public void AddMultiWeapons(List<int> WeaponsID, Transform pos)
    {
        for (int i = 0; i < WeaponsID.Count; i++)
        {
            AddWeapon(WeaponsID[i], pos);
        }
    }

    public void AddWeapon(int weaponID, Transform pos)
    {
        if (GameData.Instance.WeaponInfoTable.ContainsKey(weaponID) == false) { return; }
        if (!m_dActiveWeapon.ContainsKey(weaponID))
        {
            m_dActiveWeapon.Add(weaponID, m_weaponFactory.CreateWeapon(weaponID));
            m_GOEffect = m_dActiveWeapon[weaponID].WeaponLoader();
            m_GOEffect.transform.parent = pos;
            m_GOEffect.transform.localPosition = Vector3.zero;
            m_AnimEffect = m_GOEffect.GetComponent<Animator>();
            m_dActiveEffect.Add(weaponID, m_AnimEffect);
            _CurrentWeapon = weaponID;
            m_tGunPos = pos;
        }
        SetWeaponInfo(weaponID);
    }
    
    public void ResetWeaponInfo() { for (int i = 0; i < ActivedWeaponID.Length; i++) { SetWeaponInfo(ActivedWeaponID[i]); } }

    public void SetWeaponInfo(int type)
    {
        m_dActiveWeapon[type].weaponInfo.Mags = GameData.Instance.WeaponInfoTable[type].Start_Mags;
        m_dActiveWeapon[type].weaponInfo.Ammo = GameData.Instance.WeaponInfoTable[type].Capacity;
    }

    public void RemoveWeapon(int weaponID)
    {
        if (GameData.Instance.WeaponInfoTable.ContainsKey(weaponID) == false) { return; }
        if (m_dActiveWeapon.ContainsKey(weaponID) == false) { return; }
        m_dActiveWeapon.Remove(weaponID);
        ObjectPool.m_Instance.RemoveObjectFromPool(weaponID);
    }

    public void ClearWeapon()
    {
        if (ActivedWeaponID.Length == 0) { return; }
        for (int i = 0; i < ActivedWeaponID.Length; i++) { ObjectPool.m_Instance.RemoveObjectFromPool(ActivedWeaponID[i]); }
        Transform t = GameObject.Find("Bullet").transform;
        Debug.Log(t.childCount);
        for (int i = 0; i < t.childCount; i++) { Destroy(t.GetChild(i).gameObject); }
        for (int i = 0; i < ActivedWeaponID.Length; i++) { Destroy(m_dActiveEffect[ActivedWeaponID[i]].gameObject); }
        m_dActiveWeapon.Clear();
        m_dActiveEffect.Clear();
    }

    public bool AddMags(int weaponID, int quantity)
    {
        if (m_dActiveWeapon.ContainsKey(weaponID) == false) { return false; }
        if (m_dActiveWeapon[weaponID].weaponInfo.Mags >= m_dActiveWeapon[weaponID].weaponInfo.Max_Mags) { return false; }
        m_dActiveWeapon[weaponID].weaponInfo.Mags += quantity;
        if (OnPickMags != null) OnPickMags();
        return true;
    }

    #endregion SetWeapon

    #region WeaponBehaviours
    public bool ShootState()
    {
        if (CurrentWeaponInfo.Ammo <= 0 ) { return false; }
        if (!m_bShooting) { return false; }
        m_bShooting = false;
        m_cCoolDown = StartCoroutine(WaitCooling());
        return true;
    }

    private IEnumerator WaitCooling()
    {
        m_currentWeaponEffect.SetTrigger("startTrigger");
        yield return new WaitForSeconds(0.2f);
        m_dActiveWeapon[_CurrentWeapon].Shot(m_tGunPos, m_fSpreadIncrease);
        if (OnFire != null) OnFire();
        yield return new WaitForSeconds(CurrentWeaponInfo.FireRate);
        m_bShooting = true;
        m_fSpreadIncrease += CurrentWeaponInfo.Spread_Increase_per_shot;
        if (_CurrentWeapon == 1401|| _CurrentWeapon == 1601|| _CurrentWeapon == 1701)
             m_currentWeaponEffect.SetTrigger("endTrigger");
        m_cCoolDown = null;
    }

    public bool ReloadState()
    {
        if (CurrentWeaponInfo.Ammo >= CurrentWeaponInfo.Capacity || CurrentWeaponInfo.Mags <= 0) { return false; }
        else if (m_bReloading) { return false; }
        m_bReloading = true;
        m_cCoolDown = StartCoroutine(WaitReloading());
        return true;
    }

    private IEnumerator WaitReloading()
    {
        if (OnReload != null) OnReload();
        yield return new WaitForSeconds(CurrentWeaponInfo.ReloadSpeed);
        m_dActiveWeapon[_CurrentWeapon].Reload();
        if (OnReloadEnd != null) OnReloadEnd();
        m_bReloading = false;
        m_cCoolDown = null;
    }

    public bool SwitchWeaponState()
    {
        for (int i = 0; i < ActivedWeaponID.Length; i++)
        {
            if (i == ActivedWeaponID.Length - 1)
            {
                _CurrentWeapon = ActivedWeaponID[0];
                if (OnSwitch != null) OnSwitch();
                return true; 
            }
            else if (ActivedWeaponID[i] == _CurrentWeapon)
            {
                _CurrentWeapon = ActivedWeaponID[i + 1];
                if (OnSwitch != null) OnSwitch();
                return true;
            }
        }
        return false;
    }
    #endregion WeaponBehaviours

    #region Delegate
    public delegate void EventHolder();
    public event EventHolder OnFire;
    public event EventHolder OnReload;
    public event EventHolder OnReloadEnd;
    public event EventHolder OnSwitch;
    public event EventHolder OnPickMags;
    #endregion

    #region Public Field
    public Dictionary<int, IWeaponBehaviour> ActiveWeapon { get { return m_dActiveWeapon; } }
    public WeaponInfo CurrentWeaponInfo { get { return m_dActiveWeapon[_CurrentWeapon].weaponInfo; } }
    public int _CurrentWeapon { get; private set; }
    public int[] ActivedWeaponID
    {
        get
        {
            int[] keys = new int[m_dActiveWeapon.Count];
            m_dActiveWeapon.Keys.CopyTo(keys, 0);
            return keys;
        }
    }
    
    public float m_fSpreadIncrease;
    public bool m_bAutoFire = true;
    public bool m_bShooting = true;
    #endregion

    #region Private member
    private WeaponFactory m_weaponFactory = new WeaponFactory();
    private GameObject m_GOEffect;
    private Animator m_AnimEffect;
    private Transform m_tGunPos;
    private Coroutine m_cCoolDown;
    private Dictionary<int, IWeaponBehaviour> m_dActiveWeapon = new Dictionary<int, IWeaponBehaviour>();
    private Dictionary<int, Animator> m_dActiveEffect = new Dictionary<int, Animator>();
    private Animator m_currentWeaponEffect { get { return m_dActiveEffect[_CurrentWeapon]; } }
    private bool m_bReloading;
    #endregion Private member
}