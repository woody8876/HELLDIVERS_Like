using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//射擊模式，Load weapon，
public class Weapon : MonoBehaviour
{
    int m_WeaponNum = 1;
    bool m_bCooling = true;
    bool m_bRefilling = false;

    public List<eWeaponType> Weapons;
    
    WeaponController m_WC;
    Dictionary<int, List<float>> m_WeapponDats = new Dictionary<int, List<float>>();

    //Bullet initial state
    public Transform m_tGunPos;

    //Load Asset
    ObjectPool m_ObjectPool;
    AssetManager m_AssetManager;
    List<GameObject> m_CurrentActives = new List<GameObject>();

//==================================================================================================

    // Use this for initialization
    void Start()
    {
        m_WC = new WeaponController();
        m_WC.Init();
        m_WeapponDats.Add(1, m_WC.WeaponInfo(Weapons[0]));
        m_WeapponDats.Add(2, m_WC.WeaponInfo(Weapons[1]));

        //Load Asset
        m_AssetManager = new AssetManager();
        m_AssetManager.Init();
        ResourceManager rm = new ResourceManager();
        rm.Init();
        m_ObjectPool = new ObjectPool();
        m_ObjectPool.Init();

        string m_sFirstWeapon = "Bullet_" + Weapons[0].ToString();
        string m_sSecondWeapon = "Bullet_" + Weapons[1].ToString();

        Object o1 = rm.LoadData(typeof(GameObject), "Ivan/Prefabs", m_sFirstWeapon, false); 
        m_ObjectPool.InitGameObjects(o1, (int)m_WeapponDats[1][0], 1);
        Object o2 = rm.LoadData(typeof(GameObject), "Ivan/Prefabs", m_sSecondWeapon, false);
        m_ObjectPool.InitGameObjects(o2, (int)m_WeapponDats[2][0], 2);
    }

    // Update is called once per frame
    void Update()
    {
        GunShoot();
        SwitchWeapon();
        Refill();
    }
    
    private void GunShoot()
    {
        if (Input.GetMouseButton(0) && m_bCooling && !m_bRefilling)
        {
            GameObject go = m_ObjectPool.LoadGameObjectFromPool(m_WeaponNum);

            if (go != null)
            {
                StartCoroutine(Cooling());
                go.transform.position = m_tGunPos.position;
                go.transform.forward = m_tGunPos.forward;
                m_CurrentActives.Add(go);
                go.SetActive(true);
                m_bCooling = false;
            }
            else
            {
                Debug.Log("There is no ammo.");
            }
        }
    }

    IEnumerator Cooling()
    {
        yield return new WaitForSeconds(m_WeapponDats[m_WeaponNum][3]);
        m_bCooling = true;
    }

    private void Refill()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int iCount = m_CurrentActives.Count;
            for (int i = 0; i < iCount; i++)
            {
                StartCoroutine(WaitRefill());
                m_ObjectPool.UnLoadObjectToPool(m_WeaponNum, m_CurrentActives[0]);
                m_CurrentActives.RemoveAt(0);
                m_bRefilling = true;
            }

        }
    }

    IEnumerator WaitRefill()
    {
        yield return new WaitForSeconds(m_WeapponDats[m_WeaponNum][2]);
        m_bRefilling = false;
    }

    private void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { m_WeaponNum = 1; }
        if (Input.GetKeyDown(KeyCode.E)) { m_WeaponNum = 2; }
    }
    
}


