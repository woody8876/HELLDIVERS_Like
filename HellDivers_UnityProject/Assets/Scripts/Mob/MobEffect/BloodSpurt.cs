using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpurt : MonoBehaviour {

    //ParticleSystem m_ParticleSystem;
    private float m_Timer = 0.0f;
    private void OnEnable()
    {
        m_Timer = 0.0f;
    }
    public void Init(GameObject target, Vector3 hitPoint)
    {
        Vector3 pos = this.transform.position;
        pos = target.transform.position;
        pos.y += 2.0f;
        this.transform.position = pos;
        Vector3 dir = hitPoint - target.transform.position;
        dir.y = 0.0f;
        this.transform.forward = dir;
        this.gameObject.SetActive(true);
        this.transform.SetParent(target.transform);
    }
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        m_Timer += Time.deltaTime;
        if (m_Timer > 3.0f)
        {
            this.transform.parent = null;
            ObjectPool.m_Instance.UnLoadObjectToPool(3002, this.gameObject);
        }
        
    }
}
