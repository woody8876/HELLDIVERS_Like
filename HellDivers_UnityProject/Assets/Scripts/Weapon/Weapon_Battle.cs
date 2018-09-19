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
    //The serial number of the play's equipment
    [SerializeField] int m_WeaponNum = 0;

    int[] m_Ammo;

    //Weapons' types and numbers of the player equipment.
    [SerializeField] List<eWeaponType> EWeapon;
    WeaponFactory m_weaponFactory = new WeaponFactory();
    List<IWeaponBehaviour> weaponBehaviours = new List<IWeaponBehaviour>();
    Transform m_tGunPos;
    Coroutine m_cCoolDown;
    //==================================================================================================
    // Use this for initialization

    private void Start()
    {
        m_tGunPos = GameObject.Find("LaunchPoint").GetComponent<Transform>();
        for (int i = 0; i < EWeapon.Count; i++)
        {
            weaponBehaviours.Add(m_weaponFactory.CreateWeapon(EWeapon[i]));
            WeaponLoader(EWeapon[i], weaponBehaviours[i]);
            m_Ammo[i] = weaponBehaviours[i].Capacity;
        }
        Debug.Log(weaponBehaviours[0]);
        Debug.Log(weaponBehaviours[1]);
    }

    private void Update()
    {
        Reloaded();
        Shoot();
        SwitchWeapon();
    }

    //==============================================================================================================================================

    private void Shoot()
    {
        if (Input.GetButton("Fire1") && m_cCoolDown == null) {
            m_cCoolDown = StartCoroutine(WaitCooling()); }
        //RaycastHit rh;
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out rh, 10, LayerMask.NameToLayer("Enemy")))
    }
    IEnumerator WaitCooling()
    {
        weaponBehaviours[m_WeaponNum].Shot(m_tGunPos.position, m_tGunPos.forward);
        yield return new WaitForSeconds(weaponBehaviours[m_WeaponNum].FireRate);
        m_cCoolDown = null;
        yield break;
    }

    /// <summary>
    /// Reload the weapon which being used.
    /// </summary>
    private void Reloaded()
    {
        if (Input.GetButtonDown("Reload")) {
            StopCoroutine(m_cCoolDown);
            m_cCoolDown = StartCoroutine(WaitReloading()); }
    }
    IEnumerator WaitReloading()
    {
        weaponBehaviours[m_WeaponNum].Reload();
        if (m_Ammo[m_WeaponNum] == 0)
        {
            yield return new WaitForSeconds(weaponBehaviours[m_WeaponNum].Empty_Reload_Speed);
        }
        m_cCoolDown = null;

    }

    /// <summary>
    /// Switch weapons in the battle. 
    /// </summary>
    private void SwitchWeapon()
    {
        if (Input.GetButtonDown("WeaponSwitch"))
        {
            if (m_WeaponNum == EWeapon.Count - 1) { m_WeaponNum = 0; }
            else if (m_WeaponNum >= 0 || m_WeaponNum < EWeapon.Count - 1) { m_WeaponNum++; }
            else
            {
                Debug.Log("Weapon's number is out of range");
                m_WeaponNum = 0;
            }
            Debug.Log(m_WeaponNum);
        }
    }

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

        string m_sFirstWeapon = "Bullet_" + type.ToString();
        Object m_FirstWeapon = rm.LoadData(typeof(GameObject), "Prefabs", m_sFirstWeapon, false);

        ObjectPool.m_Instance.InitGameObjects(m_FirstWeapon, weaponData.Capacity, (int)type + 100);
        if (ObjectPool.m_Instance == null)
        {
            ObjectPool OP = GetComponent<ObjectPool>();
            OP.InitGameObjects(m_FirstWeapon, weaponData.Capacity, (int)type + 100);
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




