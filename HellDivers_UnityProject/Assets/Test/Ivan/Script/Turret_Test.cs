using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class Turret_Test : MonoBehaviour {

    public Transform GunPos;
    public Transform Target;
    public WeaponController WeaponController { get{ return m_weaponController; }}
    private WeaponController m_weaponController;

    private AssetManager m_AssetManager = new AssetManager();
    private ResourceManager m_ResourceManager = new ResourceManager();
    private ObjectPool m_ObjectPool = new ObjectPool();
    private GameData m_GameData = new GameData();


    private void Awake()
    {
        m_weaponController = GetComponent<WeaponController>();
        m_AssetManager.Init();
        m_ResourceManager.Init();
        m_ObjectPool.Init();
        m_GameData.Init();

    }
    // Use this for initialization
    void Start () {
        m_weaponController.AddWeapon(1901, GunPos);
	}
	
	// Update is called once per frame
	void Update () {
        m_weaponController.ShootState();
        transform.forward = Target.position - transform.position;
	}
}
