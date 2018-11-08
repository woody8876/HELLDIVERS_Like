using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBullet : MonoBehaviour {

    [SerializeField]private float m_Speed = 15.0f;
    [SerializeField]private float m_LifeTime = 5.0f;
    private float m_Timer = 0.0f;
    private float m_NextPosDis;
    // Use this for initialization
    private void OnEnable()
    {
        m_Timer = 0.0f;
    }
    void Start () {
        m_NextPosDis = Time.fixedDeltaTime * m_Speed;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        m_Timer += Time.deltaTime;
        if(m_Timer >= m_LifeTime)
        {
            ObjectPool.m_Instance.UnLoadObjectToPool(3201, this.gameObject);
        }
        Detect();
        this.transform.position = this.transform.position + this.transform.forward * m_Speed * Time.deltaTime;
    }

    private void Detect()
    {
        RaycastHit rh;
        if (Physics.Raycast(transform.position, transform.forward, out rh, m_NextPosDis, 1 << LayerMask.NameToLayer("Player")))
        {
            Debug.Log("Hit");
            GameObject go = rh.collider.gameObject;
            IDamageable target = rh.transform.gameObject.GetComponent<IDamageable>();
            target.TakeDamage(10.0f, rh.point);
            BulletDeath();
        }
        else if (Physics.Raycast(transform.position, transform.forward, out rh, m_NextPosDis, 1 << LayerMask.NameToLayer("Obstcale")))
        {
            BulletDeath();
        }
    }

    private void BulletDeath()
    {
        ObjectPool.m_Instance.UnLoadObjectToPool(3201, this.gameObject);
    }
}
