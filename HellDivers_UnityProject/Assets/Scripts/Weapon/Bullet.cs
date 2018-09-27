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
    
    //Bullet's speed
    private float m_fSpeed = 100;
    Renderer m_bullet;
    //========================================================================
    void Start () {
        m_bullet = this.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update () {
        StartCoroutine(BulletDeath());
    }

    private void OnTriggerEnter(Collider other)
    {
        //m_bullet.enabled = false;
    }

    IEnumerator BulletDeath()
    {
        this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * m_fSpeed;
        Weapon_Battle.Instance.weaponBehaviours[m_Type].m_Weapon_CurrentActives.Add(this.gameObject);
        yield return new WaitForSeconds(Weapon_Battle.Instance.weaponBehaviours[m_Type].RANGE /m_fSpeed);
        ObjectPool.m_Instance.UnLoadObjectToPool((int)m_Type + 100, Weapon_Battle.Instance.weaponBehaviours[m_Type].m_Weapon_CurrentActives[0]);
        Weapon_Battle.Instance.weaponBehaviours[m_Type].m_Weapon_CurrentActives.RemoveAt(0);
        this.gameObject.SetActive(false);
    }


}
