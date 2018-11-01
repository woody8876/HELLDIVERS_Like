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
        if (m_dActiveWeapon.ContainsKey(weaponID) == true) { return; }
        m_dActiveWeapon.Add(weaponID, m_weaponFactory.CreateWeapon(weaponID));
        Debug.Log(m_dActiveWeapon.Count);
        m_dActiveWeapon[weaponID].weaponInfo.Mags = GameData.Instance.WeaponInfoTable[weaponID].Start_Mags;
        m_dActiveWeapon[weaponID].weaponInfo.Ammo = GameData.Instance.WeaponInfoTable[weaponID].Capacity;
        m_GOEffect = m_dActiveWeapon[weaponID].WeaponLoader();
        m_GOEffect.transform.parent = pos;
        m_GOEffect.transform.localPosition = Vector3.zero;
        m_AnimEffect = m_GOEffect.GetComponent<Animator>();
        _CurrentWeapon = weaponID;

        m_tGunPos = pos;
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
        for (int i = 0; i < ActivedWeaponID.Length; i++)
        {
            ObjectPool.m_Instance.RemoveObjectFromPool(ActivedWeaponID[i]);
        }
        m_dActiveWeapon.Clear();
    }

    public bool AddMags(int weaponID, int quantity)
    {
        if (m_dActiveWeapon.ContainsKey(weaponID) == false) { return false; }
        if (m_dActiveWeapon[weaponID].weaponInfo.Mags >= m_dActiveWeapon[weaponID].weaponInfo.Max_Mags) { return false; }
        m_dActiveWeapon[weaponID].weaponInfo.Mags += quantity;
        return true;
    }

    #endregion SetWeapon

    #region WeaponBehaviours
    public bool ShootState()
    {
        if (m_dActiveWeapon[_CurrentWeapon].weaponInfo.Ammo <= 0 )
        {
            Debug.Log("Ammo");
            return false;
        }
        if (!m_bShooting) { return false; }
        m_bShooting = false;
        m_cCoolDown = StartCoroutine(WaitCooling());
        return true;
    }

    private IEnumerator WaitCooling()
    {
        m_AnimEffect.SetTrigger("startTrigger");
        yield return new WaitForSeconds(0.2f);
        m_dActiveWeapon[_CurrentWeapon].Shot(m_tGunPos.position, m_tGunPos.forward, m_fSpreadIncrease, ref m_fDamage);
        OnFire();
        yield return new WaitForSeconds(m_dActiveWeapon[_CurrentWeapon].weaponInfo.FireRate);
        m_bShooting = true;
        m_fSpreadIncrease += m_dActiveWeapon[_CurrentWeapon].weaponInfo.Spread_Increase_per_shot;
        m_cCoolDown = null;
        //        yield break;
    }

    public bool ReloadState()
    {
        if (m_dActiveWeapon[_CurrentWeapon].weaponInfo.Ammo >= m_dActiveWeapon[_CurrentWeapon].weaponInfo.Capacity ||
            m_dActiveWeapon[_CurrentWeapon].weaponInfo.Mags <= 0) { return false; }
        else if (m_bReloading) { return false; }
        m_bReloading = true;
        m_cCoolDown = StartCoroutine(WaitReloading());
        return true;
    }

    private IEnumerator WaitReloading()
    {
        OnReload();
        if (m_dActiveWeapon[_CurrentWeapon].weaponInfo.Ammo <= 0) {
            yield return new WaitForSeconds(m_dActiveWeapon[_CurrentWeapon].weaponInfo.Empty_Reload_Speed); }
        else {
            yield return new WaitForSeconds(m_dActiveWeapon[_CurrentWeapon].weaponInfo.Tactical_Reload_Speed); }
        m_dActiveWeapon[_CurrentWeapon].Reload();
        OnReloadEnd();
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
                OnSwitch();
                return true; 
            }
            else if (ActivedWeaponID[i] == _CurrentWeapon)
            {
                _CurrentWeapon = ActivedWeaponID[i + 1];
                OnSwitch();
                return true;
            }
        }
        return false;
    }
    #endregion WeaponBehaviours

    //For Debug
    [SerializeField] private int currentWeapon;
    [SerializeField] private int[] activedWeapon;
    [SerializeField] private int currentAmmo;

    public Dictionary<int, IWeaponBehaviour> ActiveWeapon { get { return m_dActiveWeapon; } }
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

    #region Delegate
    public delegate void Shoot();
    public event Shoot OnFire;
    public delegate void StartReload();
    public event StartReload OnReload;
    public delegate void EndReload();
    public event EndReload OnReloadEnd;
    public delegate void SwitchWeapon();
    public event SwitchWeapon OnSwitch;
    #endregion

    #region Private member

    private WeaponFactory m_weaponFactory = new WeaponFactory();
    private GameObject m_GOEffect;
    private Animator m_AnimEffect;
    private Transform m_tGunPos;
    private Coroutine m_cCoolDown;

    public delegate void ActiveState();

    public ActiveState m_ActiveState;

    private Dictionary<int, IWeaponBehaviour> m_dActiveWeapon = new Dictionary<int, IWeaponBehaviour>();

    public bool m_bShooting = true;
    private float m_fDamage;
    private bool m_bReloading;

    #endregion Private member
}