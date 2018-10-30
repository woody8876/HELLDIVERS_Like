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
    [SerializeField] private int m_ID;
    
    //Bullet's speed
    private float m_fSpeed = 100;
    private float m_fRange;
    private int m_iID;

    //Renderer m_bullet;
    //========================================================================
    void Start () {
        //m_bullet = this.gameObject.GetComponent<Renderer>();
        m_fRange = GameData.Instance.WeaponInfoTable[m_ID].Range;
        m_iID = GameData.Instance.WeaponInfoTable[m_ID].ID;
    }

    // Update is called once per frame
    void FixedUpdate () {
        StartCoroutine(BulletDeath());
    }

    private void OnTriggerEnter(Collider other)
    {
        //m_bullet.enabled = false;

        if (other.transform.tag == "Player")
        {
            IDamageable target = other.transform.GetComponent<IDamageable>();
            target.TakeDamage(GameData.Instance.WeaponInfoTable[m_ID].Damage, other.transform.position);
        }
    }

    IEnumerator BulletDeath()
    {
        this.transform.position = this.transform.position + this.transform.forward * Time.fixedDeltaTime * m_fSpeed;
        yield return new WaitForSeconds(m_fRange/m_fSpeed);
        ObjectPool.m_Instance.UnLoadObjectToPool(m_iID, this.gameObject);
    }


}
