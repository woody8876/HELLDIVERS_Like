using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(WeaponController))]
public class MoveTest : MonoBehaviour {

    public Transform GunPos;
    Vector3 m_vPos;
    WeaponController weaponController;
    List<int> weapons = new List<int> { 1101, 1301 };
	// Use this for initialization
	void Start () {
        weaponController = GetComponent<WeaponController>();
        weaponController.AddMultiWeapons(weapons, GunPos);
	}
	
	// Update is called once per frame
	void Update () {
        m_vPos.x = Input.GetAxis("Horizontal") * Time.deltaTime * 30;
        m_vPos.z = Input.GetAxis("Vertical") * Time.deltaTime * 30;
        transform.Translate(m_vPos);
	}
}
