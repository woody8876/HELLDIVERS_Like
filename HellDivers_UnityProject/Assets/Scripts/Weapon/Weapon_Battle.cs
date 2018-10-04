///2018.09.10
///Ivan.CC
///
/// Weapon's behaviour for player.
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Battle : MonoBehaviour
{

    public static Dictionary<eWeaponType, IWeaponBehaviour> weaponBehaviours = new Dictionary<eWeaponType, IWeaponBehaviour>();

    #region Private member
    private WeaponFactory m_weaponFactory = new WeaponFactory();
    private Player m_playerInfo;
    private GameObject m_Effect;
    private Animator m_effect;
    private Transform m_tGunPos;
    private Coroutine m_cCoolDown;

    private delegate void ActiveState();
    private ActiveState m_ActiveState;

    private List<eWeaponType> EWeapon = new List<eWeaponType>();
    private List<int> m_CurMags = new List<int>();
    private List<int> m_CurAmmo = new List<int>();

    private int m_WeaponNum = 0;
    private float m_fDamage;
    private float m_fSpreadIncrease;
    private bool m_bAutoFire = true;

    #endregion Private member

    //==================================================================================================
    private void Start()
    {
        m_ActiveState = IdleState;
        m_playerInfo = GetComponent<Player>() ;
        if (m_playerInfo == null) return;
        m_tGunPos = m_playerInfo.Parts.LaunchPoint;
        if (m_playerInfo.Info.WeaponId.Count > 0)
        {
            for (int i = 0; i < m_playerInfo.Info.WeaponId.Count; i++)
            {
                EWeapon.Add((eWeaponType)m_playerInfo.Info.WeaponId[i]);
            }
        }
        else
        {
            EWeapon.Add((eWeaponType)1);
        }
        for (int i = 0; i < EWeapon.Count; i++)
        {
            weaponBehaviours.Add(EWeapon[i], m_weaponFactory.CreateWeapon(EWeapon[i]));
            WeaponLoader(EWeapon[i], weaponBehaviours[EWeapon[i]]);
            //Initial ammo and mags
            m_CurAmmo.Add(weaponBehaviours[EWeapon[i]].weaponInfo().Capacity);
            m_CurMags.Add(weaponBehaviours[EWeapon[i]].weaponInfo().Start_Mags);
            Debug.Log(m_CurMags[i]);
            Debug.Log(weaponBehaviours[EWeapon[i]]);
        }
        m_effect = m_Effect.GetComponent<Animator>();
    }

    private void Update()
    {
        m_ActiveState();
    }

    //=============================================================================================================================================

    #region WeaponBehaviours

    private void IdleState()
    {
        if (Input.GetButton("Fire1") && m_cCoolDown == null) { m_ActiveState = ShootState; }
        else if (Input.GetButton("Reload")) { m_ActiveState = ReloadState; }
        else if (Input.GetButtonDown("WeaponSwitch")) { m_ActiveState = SwitchWeaponState; }
    }

    private void ShootState()
    {
        if (m_CurAmmo[m_WeaponNum]<= 0 || !Input.GetButton("Fire1"))
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
        weaponBehaviours[EWeapon[m_WeaponNum]].Shot(m_tGunPos.position, m_tGunPos.forward, m_fSpreadIncrease, ref m_fDamage);
        yield return new WaitForSeconds(weaponBehaviours[EWeapon[m_WeaponNum]].weaponInfo().FireRate);
        Debug.Log("Shoot");
        m_bAutoFire = (weaponBehaviours[EWeapon[m_WeaponNum]].weaponInfo().FireMode == 0) ? false : true;
        m_fSpreadIncrease += weaponBehaviours[EWeapon[m_WeaponNum]].weaponInfo().Spread_Increase_per_shot;
        m_cCoolDown = null;
        yield break;
    }

    private void ReloadState()
    {
        if (m_CurAmmo[m_WeaponNum] >= weaponBehaviours[EWeapon[m_WeaponNum]].weaponInfo().Capacity || m_CurMags[m_WeaponNum] <= 0)
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

        if (m_CurAmmo[m_WeaponNum] <= 0) { yield return new WaitForSeconds(weaponBehaviours[EWeapon[m_WeaponNum]].weaponInfo().Empty_Reload_Speed); }
        else if (m_CurAmmo[m_WeaponNum] > 0) { yield return new WaitForSeconds(weaponBehaviours[EWeapon[m_WeaponNum]].weaponInfo().Tactical_Reload_Speed); }
        weaponBehaviours[EWeapon[m_WeaponNum]].Reload();
        m_CurMags[m_WeaponNum]--;
        Debug.Log(m_CurMags[m_WeaponNum]);
        m_cCoolDown = null;
    }

    private void SwitchWeaponState()
    {
        if (m_WeaponNum == EWeapon.Count - 1) { m_WeaponNum = 0; }
        else if (m_WeaponNum >= 0 || m_WeaponNum < EWeapon.Count - 1) { m_WeaponNum++; }
        else
        {
            Debug.Log("Weapon's number is out of range");
            m_WeaponNum = 0;
        }
        Debug.Log(m_WeaponNum);
        m_ActiveState = IdleState;
    }

    private void PickUpMgas(GameObject item)
    {
        if (Input.GetButtonDown("Pick"))
        {
        }
    }

    #endregion WeaponBehaviours

    //=============================================================================================================================================
    /// <summary>
    /// Load weapon from Objectpool.
    /// </summary>
    /// <param name="type">Weapon type</param>
    /// <param name="weaponData">Data in calss "Weapon".</param>
    public void WeaponLoader(eWeaponType type, IWeaponBehaviour weaponData)
    {
        AssetManager AssetManager = new AssetManager();
        AssetManager.Init();
        ResourceManager rm = new ResourceManager();
        rm.Init();

        string m_sWeapon = "Bullet_" + type.ToString();
        string m_sEffect = "Effect_" + type.ToString();
        Object m_Weapon = rm.LoadData(typeof(GameObject), "Prefabs", m_sWeapon, false);
        m_Effect = rm.LoadData(typeof(GameObject), "Prefabs", m_sEffect, true) as GameObject;

        ObjectPool.m_Instance.InitGameObjects(m_Weapon, weaponData.weaponInfo().Capacity, (int)type + 100);
        if (ObjectPool.m_Instance == null)
        {
            ObjectPool OP = GetComponent<ObjectPool>();
            OP.InitGameObjects(m_Weapon, weaponData.weaponInfo().Capacity, (int)type + 100);
        }
    }

    /// <summary>
    /// For player equip weapons
    /// </summary>
    /// <param name="weaponType">weapon type</param>
    /// <returns></returns>
    public IWeaponBehaviour SelectWeapon(eWeaponType weaponType)
    {
        IWeaponBehaviour weaponBehaviour = m_weaponFactory.CreateWeapon(weaponType);
        return weaponBehaviour;
    }

}