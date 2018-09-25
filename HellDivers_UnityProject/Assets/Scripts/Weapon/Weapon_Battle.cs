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
    //Weapons' types and numbers of the player equipment.

    #region Singleton
    private static Weapon_Battle m_Instance;
    public static Weapon_Battle Instance { get { return m_Instance; } }
    #endregion Singleton

    public Dictionary < eWeaponType,IWeaponBehaviour> weaponBehaviours = new Dictionary<eWeaponType, IWeaponBehaviour>();

    #region Private member
    [SerializeField]
    private List<eWeaponType> EWeapon;
    private List<int> m_CurMags = new List<int>();
    private WeaponFactory m_weaponFactory = new WeaponFactory();
    private delegate void ActiveState();
    private ActiveState m_ActiveState;
    private Animator m_effect;
    private Transform m_tGunPos;
    private Coroutine m_cCoolDown;
    private int m_WeaponNum = 0;
    private float m_fSpreadIncrease;
    private bool m_bAutoFire = true;
    #endregion
    //==================================================================================================
    private void Start()
    {
        m_Instance = this;
        m_effect = GameObject.Find("Gun_Effect").GetComponent<Animator>();
        m_tGunPos = GameObject.Find("LaunchPoint").GetComponent<Transform>();
        m_ActiveState = IdleState;
        for (int i = 0; i < EWeapon.Count; i++)
        {
            //Create weapon and add it into list
            weaponBehaviours.Add(EWeapon[i], m_weaponFactory.CreateWeapon(EWeapon[i]));
            //Load bullet and particle system
            WeaponLoader(EWeapon[i], weaponBehaviours[EWeapon[i]]);
            //Initial mags
            m_CurMags.Add(weaponBehaviours[EWeapon[i]].Start_Mags);
            Debug.Log(m_CurMags[i]);
            Debug.Log(weaponBehaviours[EWeapon[i]]);
        }
    }
    private void Update()
    {
        m_ActiveState();
    }
    //=============================================================================================================================================    
    #region WeaponBehaviours

    private void IdleState()
    {
        Debug.Log("Idle");

        if (Input.GetButton("Fire1") && m_cCoolDown == null)
        {
            m_fSpreadIncrease = 0;
            m_ActiveState = ShootState;
        }
        else if (Input.GetButton("Reload"))
        {
            m_ActiveState = ReloadState;
        }
        else if (Input.GetButtonDown("WeaponSwitch"))
        {
            m_ActiveState = SwitchWeaponState;
        }
    }
    private void ShootState()
    {
        if (weaponBehaviours[EWeapon[m_WeaponNum]].Ammo <= 0 || !Input.GetButton("Fire1"))
        {
            Debug.Log("Ammo");
            m_bAutoFire = true;
            m_ActiveState = IdleState;
            return;
        }
        if (!m_bAutoFire) { return; }
        m_bAutoFire = false;
        m_cCoolDown = StartCoroutine(WaitCooling());
    }
    IEnumerator WaitCooling()
    {
        m_effect.SetTrigger("startTrigger");
        yield return new WaitForSeconds(0.2f);
        weaponBehaviours[EWeapon[m_WeaponNum]].Shot(m_tGunPos.position, m_tGunPos.forward, m_fSpreadIncrease);
        Debug.Log(m_fSpreadIncrease);
        yield return new WaitForSeconds(weaponBehaviours[EWeapon[m_WeaponNum]].FireRate);
        Debug.Log("Shot");
        m_cCoolDown = null;
        m_bAutoFire = (weaponBehaviours[EWeapon[m_WeaponNum]].FireMode == 0) ? false : true;
        m_fSpreadIncrease += weaponBehaviours[EWeapon[m_WeaponNum]].Spread_Increase_per_shot;
        yield break;
    }
    private void ReloadState()
    {
        if (weaponBehaviours[EWeapon[m_WeaponNum]].Ammo >= weaponBehaviours[EWeapon[m_WeaponNum]].Capacity || m_CurMags[m_WeaponNum] <= 0)
        {
            m_ActiveState = IdleState;
            return;
        }
        if (m_cCoolDown != null) StopCoroutine(m_cCoolDown);
        Debug.Log("Reload");
        m_cCoolDown = StartCoroutine(WaitReloading());        
    }
    IEnumerator WaitReloading()
    {
        if (Input.GetButtonUp("Reload")) { m_ActiveState = IdleState; }

        if (weaponBehaviours[EWeapon[m_WeaponNum]].Ammo <= 0) { yield return new WaitForSeconds(weaponBehaviours[EWeapon[m_WeaponNum]].Empty_Reload_Speed); }
        else if (weaponBehaviours[EWeapon[m_WeaponNum]].Ammo > 0) { yield return new WaitForSeconds(weaponBehaviours[EWeapon[m_WeaponNum]].Tactical_Reload_Speed); }
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
    #endregion
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
        Object m_Weapon = rm.LoadData(typeof(GameObject), "Prefabs", m_sWeapon, false);

        ObjectPool.m_Instance.InitGameObjects(m_Weapon, weaponData.Capacity, (int)type + 100);
        if (ObjectPool.m_Instance == null)
        {
            ObjectPool OP = GetComponent<ObjectPool>();
            OP.InitGameObjects(m_Weapon, weaponData.Capacity, (int)type + 100);
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




