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

        for (int i = 0; i < ActivedWeapon.Length; i++)
        {
            ObjectPool.m_Instance.RemoveObjectFromPool(ActivedWeapon[i]);
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

    #endregion
    
    #region WeaponBehaviours

    private void IdleState()
    {
        if (Input.GetButton("Fire1") && m_cCoolDown == null) { m_ActiveState = ShootState; }
        else if (Input.GetButton("Reload") && m_cCoolDown == null) { m_ActiveState = ReloadState; }
        else if (Input.GetButtonDown("WeaponSwitch")) { m_ActiveState = SwitchWeaponState; }
    }

    private void ShootState()
    {
        if (m_dActiveWeapon[_CurrentWeapon ].weaponInfo.Ammo <= 0 || !Input.GetButton("Fire1"))
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
        m_AnimEffect.SetTrigger("startTrigger");
        yield return new WaitForSeconds(0.2f);
        m_dActiveWeapon[_CurrentWeapon ].Shot(m_tGunPos.position, m_tGunPos.forward, m_fSpreadIncrease, ref m_fDamage);
        yield return new WaitForSeconds(m_dActiveWeapon[_CurrentWeapon ].weaponInfo.FireRate);
        Debug.Log((m_dActiveWeapon[_CurrentWeapon ].weaponInfo.Ammo));
        m_bAutoFire = (m_dActiveWeapon[_CurrentWeapon ].weaponInfo.FireMode == 0) ? false : true;
        m_fSpreadIncrease += m_dActiveWeapon[_CurrentWeapon ].weaponInfo.Spread_Increase_per_shot;
        m_cCoolDown = null;
//        yield break;
    }

    private void ReloadState()
    {
        Debug.Log("Reloading...");
        Debug.Log("Mags :" + m_dActiveWeapon[_CurrentWeapon ].weaponInfo.Mags);
        if (m_dActiveWeapon[_CurrentWeapon ].weaponInfo.Ammo >= m_dActiveWeapon[_CurrentWeapon ].weaponInfo.Capacity || m_dActiveWeapon[_CurrentWeapon ].weaponInfo.Mags <= 0)
        {
            m_ActiveState = IdleState;
            return;
        }else if (m_bReloading) { return; }
        //if (m_cCoolDown != null) StopCoroutine(m_cCoolDown);
        Debug.Log("Reload");
        m_bReloading = true;
        m_cCoolDown = StartCoroutine(WaitReloading());
    }

    private IEnumerator WaitReloading()
    {
        if (Input.GetButtonUp("Reload")) { m_ActiveState = IdleState; }

        if (m_dActiveWeapon[_CurrentWeapon ].weaponInfo.Ammo <= 0) { yield return new WaitForSeconds(m_dActiveWeapon[_CurrentWeapon ].weaponInfo.Empty_Reload_Speed); }
        else{ yield return new WaitForSeconds(m_dActiveWeapon[_CurrentWeapon ].weaponInfo.Tactical_Reload_Speed); }
        m_dActiveWeapon[_CurrentWeapon ].Reload();
        m_bReloading = false;
        m_cCoolDown = null;
    }

    private void SwitchWeaponState()
    {
        Debug.Log(_CurrentWeapon );
        for (int i = 0; i< ActivedWeapon.Length; i++)
        {
            if (i == ActivedWeapon.Length - 1)
            {
                _CurrentWeapon = ActivedWeapon[0];
                m_ActiveState = IdleState;
                return;
            }
            else if (ActivedWeapon[i] == _CurrentWeapon )
            {
                _CurrentWeapon = ActivedWeapon[i + 1];
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
        currentWeapon = _CurrentWeapon;
        activedWeapon = ActivedWeapon;
        currentAmmo = m_dActiveWeapon[currentWeapon].weaponInfo.Ammo;
        if (m_dActiveWeapon.Count < 1) { return; }
        m_ActiveState();
    }
    #endregion

    //For Debug
    [SerializeField] int currentWeapon;
    [SerializeField] int[] activedWeapon;
    [SerializeField] int currentAmmo;


    public int _CurrentWeapon { get; private set; }
    public int[] ActivedWeapon
    {
        get
        {
            int[] keys = new int[m_dActiveWeapon.Count];
            m_dActiveWeapon.Keys.CopyTo(keys, 0);
            return keys;
        }
    } 
    
    #region Private member
    private WeaponFactory m_weaponFactory = new WeaponFactory();
    private GameObject m_GOEffect;
    private Animator m_AnimEffect;
    private Transform m_tGunPos;
    private Coroutine m_cCoolDown;

    private delegate void ActiveState();
    private ActiveState m_ActiveState;

    private Dictionary<int, IWeaponBehaviour> m_dActiveWeapon = new Dictionary<int, IWeaponBehaviour>();

    private float m_fDamage;
    private float m_fSpreadIncrease;
    private bool m_bAutoFire = true;
    private bool m_bReloading;

    #endregion Private member
}