using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(WeaponController))]
public class MoveTest : MonoBehaviour {

    public Transform GunPos;
    Vector3 m_vPos;
    WeaponController weaponController;
	// Use this for initialization
	void Start () {
        weaponController = GetComponent<WeaponController>();
        weaponController.AddWeapon(eWeaponType.Assault_Rifles, 1);
        weaponController.InitWeapon(GunPos);
	}
	
	// Update is called once per frame
	void Update () {
        m_vPos.x = Input.GetAxis("Horizontal");
        m_vPos.z = Input.GetAxis("Vertical");
        transform.position += m_vPos * Time.deltaTime * 10;
	}
}
