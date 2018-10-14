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
        m_effect = m_Effect.GetComponent<Animator>();
    }
    public void AddWeapon(eWeaponType weaponType)
    {
        if (m_dActiveWeapon.ContainsKey(weaponType) == true) { return; }
        m_dActiveWeapon.Add(weaponType, m_weaponFactory.CreateWeapon(weaponType));
        m_dActiveWeapon[weaponType].weaponInfo().Mags = GameData.Instance.WeaponInfoTable[(int)weaponType].Start_Mags;
        WeaponLoader(weaponType, m_dActiveWeapon[weaponType]);
        CurrentWeapon = weaponType;
    }
    public void RemoveWeapon(eWeaponType weaponType)
    {
        if (m_dActiveWeapon.ContainsKey(weaponType) == false) { return; }
        m_dActiveWeapon.Remove(weaponType);
        ObjectPool.m_Instance.RemoveObjectFromPool((int)weaponType + 100);
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
        if (m_dActiveWeapon[CurrentWeapon].weaponInfo().Ammo <= 0 || !Input.GetButton("Fire1"))
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
        m_Effect.transform.position = m_tGunPos.position;
        m_effect.SetTrigger("startTrigger");
        yield return new WaitForSeconds(0.2f);
        m_dActiveWeapon[CurrentWeapon].Shot(m_tGunPos.position, m_tGunPos.forward, m_fSpreadIncrease, ref m_fDamage);
        yield return new WaitForSeconds(m_dActiveWeapon[CurrentWeapon].weaponInfo().FireRate);
        Debug.Log((m_dActiveWeapon[CurrentWeapon].weaponInfo().Ammo));
        m_bAutoFire = (m_dActiveWeapon[CurrentWeapon].weaponInfo().FireMode == 0) ? false : true;
        m_fSpreadIncrease += m_dActiveWeapon[CurrentWeapon].weaponInfo().Spread_Increase_per_shot;
        m_cCoolDown = null;
        yield break;
    }

    private void ReloadState()
    {
        Debug.Log("Reloading...");
        Debug.Log("Mags :" + m_dActiveWeapon[CurrentWeapon].weaponInfo().Mags);
        if (m_dActiveWeapon[CurrentWeapon].weaponInfo().Ammo >= m_dActiveWeapon[CurrentWeapon].weaponInfo().Capacity || m_dActiveWeapon[CurrentWeapon].weaponInfo().Mags <= 0)
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

        if (m_dActiveWeapon[CurrentWeapon].weaponInfo().Ammo <= 0) { yield return new WaitForSeconds(m_dActiveWeapon[CurrentWeapon].weaponInfo().Empty_Reload_Speed); }
        else if (m_dActiveWeapon[CurrentWeapon].weaponInfo().Ammo > 0) { yield return new WaitForSeconds(m_dActiveWeapon[CurrentWeapon].weaponInfo().Tactical_Reload_Speed); }
        m_dActiveWeapon[CurrentWeapon].Reload();
        m_cCoolDown = null;
    }

    private void SwitchWeaponState()
    {
        int currentWeapon = (int)CurrentWeapon;
        for (int i = currentWeapon + 1; i < (int)eWeaponType.LastOne; i++)
        {
            if (m_dActiveWeapon.ContainsKey((eWeaponType)i))
            {
                CurrentWeapon = (eWeaponType)i;
                m_ActiveState = IdleState;
                return;
            }
        }
        for (int i = 0; i < currentWeapon; i++)
        {
            if (m_dActiveWeapon.ContainsKey((eWeaponType)i))
            {
                CurrentWeapon = (eWeaponType)i;
                m_ActiveState = IdleState;
                return;
            }
        }
        m_ActiveState = IdleState;
    }
    #endregion WeaponBehaviours
    
    #region MonoBehaviors
    private void Start()
    {
        m_dActiveWeapon.Clear();
        m_ActiveState = IdleState;
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
    private void WeaponLoader(eWeaponType type, IWeaponBehaviour weaponData)
    {
        string m_sWeapon = "Bullet_" + type.ToString();
        string m_sEffect = "Effect_" + type.ToString();
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
            ObjectPool.m_Instance.InitGameObjects(m_Weapon, weaponData.weaponInfo().Capacity, (int)type + 100);
        }
        else
        {
            ObjectPool OP = GetComponent<ObjectPool>();
            OP.InitGameObjects(m_Weapon, weaponData.weaponInfo().Capacity, (int)type + 100);
        }
    }

    public void PickUpMgas(int ItemID)
    {
        eWeaponType weaponType = eWeaponType.FirstOne;
        
        if (m_dActiveWeapon.ContainsKey(weaponType) == false) { return; }
        if (m_dActiveWeapon[weaponType].weaponInfo().Mags >= m_dActiveWeapon[weaponType].weaponInfo().Max_Mags) { return; }
        m_dActiveWeapon[weaponType].weaponInfo().Mags++;
    }

    public eWeaponType CurrentWeapon { get; private set; } 
    
    #region Private member
    private WeaponFactory m_weaponFactory = new WeaponFactory();
    private GameObject m_Effect;
    private Animator m_effect;
    private Transform m_tGunPos;
    private Coroutine m_cCoolDown;

    private delegate void ActiveState();
    private ActiveState m_ActiveState;

    private Dictionary<eWeaponType, IWeaponBehaviour> m_dActiveWeapon = new Dictionary<eWeaponType, IWeaponBehaviour>();

    private float m_fDamage;
    private float m_fSpreadIncrease;
    private bool m_bAutoFire = true;

    #endregion Private member
}