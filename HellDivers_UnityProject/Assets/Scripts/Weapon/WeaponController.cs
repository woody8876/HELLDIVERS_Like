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
    public void InitWeapon(Transform GunPos)
    {
        m_tGunPos = GunPos;
        m_Effect.transform.parent = GunPos;
        m_effect = m_Effect.GetComponent<Animator>();
    }
    public void AddMultiWeapons(List<int> WeaponsID)
    {
        for (int i = 0; i < WeaponsID.Count; i++)
        {
            AddWeapon(WeaponsID[i]);
        }
    }
    public void AddWeapon(int weaponID)
    {
        if (GameData.Instance.WeaponInfoTable.ContainsKey(weaponID) == false) { return; }
        if (m_dActiveWeapon.ContainsKey(weaponID) == true) { return; }
        m_dActiveWeapon.Add(weaponID, m_weaponFactory.CreateWeapon(weaponID));
        m_dActiveWeapon[weaponID].weaponInfo.Mags = GameData.Instance.WeaponInfoTable[weaponID].Start_Mags;
        WeaponLoader(m_dActiveWeapon[weaponID]);
        CurrentWeapon = weaponID;
    }
    public void RemoveWeapon(int weaponID)
    {
        if (GameData.Instance.WeaponInfoTable.ContainsKey(weaponID) == false) { return; }
        if (m_dActiveWeapon.ContainsKey(weaponID) == false) { return; }
        m_dActiveWeapon.Remove(weaponID);
        ObjectPool.m_Instance.RemoveObjectFromPool(weaponID);
    }
    #endregion
    
    #region WeaponBehaviours

    private void IdleState()
    {
        if (Input.GetButton("Fire1") && m_cCoolDown == null) { m_ActiveState = ShootState; }
        else if (Input.GetButton("Reload")) { m_ActiveState = ReloadState; }
        else if (Input.GetButtonDown("WeaponSwitch")) { m_ActiveState = SwitchWeaponState; }
    }

    private void ShootState()
    {
        if (m_dActiveWeapon[CurrentWeapon].weaponInfo.Ammo <= 0 || !Input.GetButton("Fire1"))
        {
            Debug.Log("Ammo");
            m_bAutoFire = true;
            m_fSpreadIncrease = 0;
            m_ActiveState = IdleState;
            return;
        }
        else if (!m_bAutoFire) { return; }
        m_bAutoFire = false;
        m_cCoolDown = StartCoroutine(WaitCooling());
    }

    private IEnumerator WaitCooling()
    {
        m_effect.SetTrigger("startTrigger");
        yield return new WaitForSeconds(0.2f);
        m_dActiveWeapon[CurrentWeapon].Shot(m_tGunPos.position, m_tGunPos.forward, m_fSpreadIncrease, ref m_fDamage);
        yield return new WaitForSeconds(m_dActiveWeapon[CurrentWeapon].weaponInfo.FireRate);
        Debug.Log((m_dActiveWeapon[CurrentWeapon].weaponInfo.Ammo));
        m_bAutoFire = (m_dActiveWeapon[CurrentWeapon].weaponInfo.FireMode == 0) ? false : true;
        m_fSpreadIncrease += m_dActiveWeapon[CurrentWeapon].weaponInfo.Spread_Increase_per_shot;
        m_cCoolDown = null;
        yield break;
    }

    private void ReloadState()
    {
        Debug.Log("Reloading...");
        Debug.Log("Mags :" + m_dActiveWeapon[CurrentWeapon].weaponInfo.Mags);
        if (m_dActiveWeapon[CurrentWeapon].weaponInfo.Ammo >= m_dActiveWeapon[CurrentWeapon].weaponInfo.Capacity || m_dActiveWeapon[CurrentWeapon].weaponInfo.Mags <= 0)
        {
            m_ActiveState = IdleState;
            return;
        }
        if (m_cCoolDown != null) StopCoroutine(m_cCoolDown);
        Debug.Log("Reload");
        m_cCoolDown = StartCoroutine(WaitReloading());
    }

    private IEnumerator WaitReloading()
    {
        if (Input.GetButtonUp("Reload")) { m_ActiveState = IdleState; }

        if (m_dActiveWeapon[CurrentWeapon].weaponInfo.Ammo <= 0) { yield return new WaitForSeconds(m_dActiveWeapon[CurrentWeapon].weaponInfo.Empty_Reload_Speed); }
        else if (m_dActiveWeapon[CurrentWeapon].weaponInfo.Ammo > 0) { yield return new WaitForSeconds(m_dActiveWeapon[CurrentWeapon].weaponInfo.Tactical_Reload_Speed); }
        m_dActiveWeapon[CurrentWeapon].Reload();
        m_cCoolDown = null;
    }

    private void SwitchWeaponState()
    {
        int[] keys = new int[m_dActiveWeapon.Count];
        m_dActiveWeapon.Keys.CopyTo(keys, 0);
        for (int i = 0; i< keys.Length; i++)
        {
            if (i == keys.Length - 1)
            {
                CurrentWeapon = keys[0];
                m_ActiveState = IdleState;
                return;
            }
            else if (keys[i] == CurrentWeapon)
            {
                CurrentWeapon = keys[i + 1];
                m_ActiveState = IdleState;
                return;
            }
        }
    }
    #endregion WeaponBehaviours
    
    #region MonoBehaviors
    private void Start()
    {
        m_ActiveState = IdleState;
        m_dActiveWeapon.Clear();

    }

    private void Update()
    {
        if (m_dActiveWeapon.Count < 1) { return; }
        m_ActiveState();
    }
    #endregion

    /// <summary>
    /// Load weapon from Objectpool.
    /// </summary>
    /// <param name="type">Weapon type</param>
    /// <param name="weaponData">Data in calss "Weapon".</param>
    private void WeaponLoader(IWeaponBehaviour weaponData)
    {
        string m_sWeapon = "Bullet_" + weaponData.weaponInfo.Title;
        string m_sEffect = "Effect_" + weaponData.weaponInfo.Title;
        int activeAmmo = (int)((weaponData.weaponInfo.Range * 0.01f)/ weaponData.weaponInfo.FireRate) + 1;
        Object m_Weapon;
        if (ResourceManager.m_Instance != null)
        {
            m_Weapon = ResourceManager.m_Instance.LoadData(typeof(GameObject), "WeaponStorage", m_sWeapon, false);
            m_Effect = ResourceManager.m_Instance.LoadData(typeof(GameObject), "WeaponStorage", m_sEffect, true) as GameObject;
        }
        else
        {
            Debug.LogWarning("No ResourceManager.");
            m_Weapon = Resources.Load(m_sWeapon);
            m_Effect = Resources.Load(m_sEffect) as GameObject;
        }
        if (ObjectPool.m_Instance != null)
        {
            ObjectPool.m_Instance.InitGameObjects(m_Weapon, activeAmmo, weaponData.weaponInfo.ID);
        }
        else
        {
            ObjectPool OP = GetComponent<ObjectPool>();
            OP.InitGameObjects(m_Weapon, activeAmmo, weaponData.weaponInfo.ID);
        }
    }

    //public void PickUpMgas(int ItemID)
    //{
    //    eWeaponType weaponType = eWeaponType.FirstOne;
    //    if (m_dActiveWeapon.ContainsKey(weaponType) == false) { return; }
    //    if (m_dActiveWeapon[weaponType].weaponInfo.Mags >= m_dActiveWeapon[weaponType].weaponInfo.Max_Mags) { return; }
    //    m_dActiveWeapon[weaponType].weaponInfo.Mags++;
    //}

    public int CurrentWeapon { get; private set; } 
    
    #region Private member
    private WeaponFactory m_weaponFactory = new WeaponFactory();
    private GameObject m_Effect;
    private Animator m_effect;
    private Transform m_tGunPos;
    private Coroutine m_cCoolDown;

    private delegate void ActiveState();
    private ActiveState m_ActiveState;

    private Dictionary<int, IWeaponBehaviour> m_dActiveWeapon = new Dictionary<int, IWeaponBehaviour>();

    private float m_fDamage;
    private float m_fSpreadIncrease;
    private bool m_bAutoFire = true;

    #endregion Private member
}