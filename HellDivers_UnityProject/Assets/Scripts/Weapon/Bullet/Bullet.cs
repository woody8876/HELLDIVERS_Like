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
    private float m_fNextPosDis;
    private float m_fRange;
    private float m_fDamage;
    private float m_Time;
    //Renderer m_bullet;
    //========================================================================
    void Start () {
        //m_bullet = this.gameObject.GetComponent<Renderer>();
        m_fRange = GameData.Instance.WeaponInfoTable[m_ID].Range;
        m_fDamage = GameData.Instance.WeaponInfoTable[m_ID].Damage;
        m_fNextPosDis = Time.fixedDeltaTime * m_fSpeed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        m_Time += Time.fixedDeltaTime;
        if (m_Time <= m_fRange / m_fSpeed)
        {
            Detect();
            this.transform.position = this.transform.position + this.transform.forward * m_fNextPosDis;
        }
        else
        {
            BulletDeath();
        }
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

    private void Detect()
    {
        RaycastHit rh;
        if (Physics.Raycast(transform.position, transform.forward, out rh, m_fNextPosDis, 1 << LayerMask.NameToLayer("Enemies")))
        {
            IDamageable target = rh.transform.gameObject.GetComponent<IDamageable>();
            target.TakeDamage(m_fDamage, rh.point);
            PlayHitEffect(rh.point);
            BulletDeath();
        }
        else if (Physics.Raycast(transform.position, transform.forward, out rh, m_fNextPosDis, 1 << LayerMask.NameToLayer("Terrain")))
        {
            BulletDeath();
        }
    }

    private void PlayHitEffect(Vector3 pos)
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(10);
        go.transform.position = pos;
        go.SetActive(true);
    }

    private void BulletDeath()
    {
        m_Time = 0;
        ObjectPool.m_Instance.UnLoadObjectToPool(m_ID, this.gameObject);
    }


}
