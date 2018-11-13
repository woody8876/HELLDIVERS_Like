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
    /// <summary>
    /// Add multi weapons into player
    /// </summary>
    /// <param name="WeaponsID">weapon's ID</param>
    /// <param name="pos">weapon's position</param>
    public void AddMultiWeapons(List<int> WeaponsID, Transform pos)
    {
        for (int i = 0; i < WeaponsID.Count; i++)
        {
            AddWeapon(WeaponsID[i], pos);
        }
    }
    /// <summary>
    /// Add single weapon into player
    /// </summary>
    /// <param name="weaponID">weapon's ID</param>
    /// <param name="pos">weapon's position</param>
    public void AddWeapon(int weaponID, Transform pos)
    {
        if (GameData.Instance.WeaponInfoTable.ContainsKey(weaponID) == false) { return; }
        if (!m_dActiveWeapon.ContainsKey(weaponID))
        {
            m_dActiveWeapon.Add(weaponID, m_weaponFactory.CreateWeapon(weaponID));
            m_dActiveWeapon[weaponID].WeaponLoader();
            m_GOEffect = ObjectPool.m_Instance.LoadGameObjectFromPool(weaponID * 10 + 1);
            m_GOEffect.transform.parent = pos;
            m_GOEffect.transform.localPosition = Vector3.zero;
            m_dActiveEffect.Add(weaponID, m_GOEffect);
            _CurrentWeapon = weaponID;
            m_tGunPos = pos;
        }
        SetWeaponInfo(weaponID);
    }
    /// <summary>
    /// Reset active weapons' ammo and mags to origine
    /// </summary>
    public void ResetWeaponInfo() { for (int i = 0; i < ActivedWeaponID.Length; i++)
        {
            SetWeaponInfo(ActivedWeaponID[i]); }
    }
    /// <summary>
    /// Reset the designate weapon to origine
    /// </summary>
    /// <param name="type"></param>
    public void SetWeaponInfo(int type)
    {
        m_dActiveWeapon[type].weaponInfo.Mags = GameData.Instance.WeaponInfoTable[type].Start_Mags;
        m_dActiveWeapon[type].weaponInfo.Ammo = GameData.Instance.WeaponInfoTable[type].Capacity;
    }
    /// <summary>
    /// Remove the designate weapon's info from dictionary and objectpool's dictionary
    /// </summary>
    /// <param name="weaponID"></param>
    public void RemoveWeapon(int weaponID)
    {
        if (GameData.Instance.WeaponInfoTable.ContainsKey(weaponID) == false) { return; }
        if (m_dActiveWeapon.ContainsKey(weaponID) == false) { return; }
        m_dActiveWeapon.Remove(weaponID);
        ObjectPool.m_Instance.RemoveObjectFromPool(weaponID);
    }
    /// <summary>
    /// Remove all the weapon from dictionary and objectpool
    /// </summary>
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
    /// <summary>
    /// Add the designate weapon's mags
    /// </summary>
    /// <param name="weaponID">weapon's ID</param>
    /// <param name="count">mags count</param>
    /// <returns></returns>
    public bool AddMags(int weaponID, int count)
    {
        if (m_dActiveWeapon.ContainsKey(weaponID) == false) { return false; }
        if (m_dActiveWeapon[weaponID].weaponInfo.Mags >= m_dActiveWeapon[weaponID].weaponInfo.Max_Mags) { return false; }
        m_dActiveWeapon[weaponID].weaponInfo.Mags += count;
        if (OnPickMags != null) OnPickMags();
        return true;
    }
    #endregion SetWeapon

    #region WeaponBehaviours
    /// <summary>
    /// Define if weapon is shooting, and the ammo's left; 
    /// </summary>
    /// <returns></returns>
    public bool ShootState()
    {
        if (CurrentWeaponInfo.Ammo <= 0 ) { return false; }
        if (!m_bShooting) { return false; }
        m_bShooting = false;
        StartCoroutine(WaitCooling());
        return true;
    }
    //Weapon's cooling after shooting, it's time decide by shooting rate
    private IEnumerator WaitCooling()
    {
        m_currentWeaponEffect.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        m_dActiveWeapon[_CurrentWeapon].Shot(m_tGunPos, m_fSpreadIncrease);
        if (OnFire != null) OnFire();
        yield return new WaitForSeconds(CurrentWeaponInfo.FireRate);
        m_bShooting = true;
        m_fSpreadIncrease += CurrentWeaponInfo.Spread_Increase_per_shot;
        if (_CurrentWeapon == 1601|| _CurrentWeapon == 1701)
            m_currentWeaponEffect.GetComponent<Animator>().SetTrigger("endTrigger");
        yield break;
    }
    /// <summary>
    /// Define weapon is reloading, and the ammo is full or not.
    /// </summary>
    /// <returns></returns>
    public bool ReloadState()
    {
        if (CurrentWeaponInfo.Ammo >= CurrentWeaponInfo.Capacity || CurrentWeaponInfo.Mags <= 0) { return false; }
        else if (m_bReloading) { return false; }
        m_bReloading = true;
        StartCoroutine(WaitReloading());
        return true;
    }
    //Weapon reloading speed decided by reloading speed.
    private IEnumerator WaitReloading()
    {
        if (OnReload != null) OnReload();
        yield return new WaitForSeconds(CurrentWeaponInfo.ReloadSpeed);
        m_dActiveWeapon[_CurrentWeapon].Reload();
        if (OnReloadEnd != null) OnReloadEnd();
        m_bReloading = false;
        yield break;
    }
    /// <summary>
    /// Switch weapon into next.
    /// </summary>
    /// <returns></returns>
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
    private Transform m_tGunPos;
    private Dictionary<int, IWeaponBehaviour> m_dActiveWeapon = new Dictionary<int, IWeaponBehaviour>();
    private Dictionary<int, GameObject> m_dActiveEffect = new Dictionary<int, GameObject>();
    private GameObject m_currentWeaponEffect { get { return m_dActiveEffect[_CurrentWeapon]; } }
    private bool m_bReloading;
    #endregion Private member
}