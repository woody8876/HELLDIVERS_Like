﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadesController : MonoBehaviour {

    [SerializeField]

    GrenaderInfo grenader = new GrenaderInfo();
    GameObject m_Grenades;
    bool m_bHolding;

	public void Start()
    {
        Object Grenades = Resources.Load("Grenades/Grenade_Pumpkin");
        Object Effect = Resources.Load("Grenades/Effect_Pumpkin");
        //ObjectPool.m_Instance.InitGameObjects(Grenades, 10, );
        //ObjectPool.m_Instance.InitGameObjects(Effect, 5, );
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space)) { Holding(); }
        if (!Input.GetKey(KeyCode.Space) && m_bHolding)  { Throw(); }
    }

    public void Holding()
    {
        if (!m_bHolding) { LoadGrenade(); }
        m_Grenades.GetComponent<Grenades>().m_Force += 10 * Time.fixedDeltaTime;
    }
    public void Throw()
    {
        m_Grenades.GetComponent<Grenades>().Throw();
        m_bHolding = false;
    }
    private void LoadGrenade()
    {
        m_Grenades = ObjectPool.m_Instance.LoadGameObjectFromPool(3001);
        m_Grenades.transform.position = this.transform.position;
        m_Grenades.transform.forward = this.transform.forward;
        m_Grenades.SetActive(true);
        m_bHolding = true;
    }
       
}