///2018.09.10
///Ivan.CC
///
/// Bullet behaviour.
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] private eWeaponType m_Type;
//    [SerializeField] private data
    //Bullet's speed
    private float m_fSpeed = 100;

    Weapon m_weapon;
 //========================================================================
    void Start () {
        m_weapon = new Weapon();
    }

    // Update is called once per frame
    void Update () {
        StartCoroutine(BulletDeath());
    }

    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator BulletDeath()
    {

        this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * m_fSpeed;
        m_weapon.m_Weapon_CurrentActives.Add(this.gameObject);
        yield return new WaitForSeconds(1);
        ObjectPool.m_Instance.UnLoadObjectToPool((int)m_Type + 100, m_weapon.m_Weapon_CurrentActives[0]);
        m_weapon.m_Weapon_CurrentActives.RemoveAt(0);
        this.gameObject.SetActive(false);

    }


}
