using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadesController : MonoBehaviour {

    [SerializeField] int m_iGrenadeCount;
    [SerializeField] int m_iGrenadeID;

    List<int> m_lActiveGrenades = new List<int>();
    GrenadeInfo grenaderInfo;
    GameObject m_Grenades;
    bool m_bHolding;

	private void AddGrenades(int id, int count)
    {
        grenaderInfo = GameData.Instance.GrenadeInfoTable[(int)id];
        string m_sGrenade = "Grenade_" + grenaderInfo.Title;
        string m_sEffect = "Effect_" + grenaderInfo.Title;
        Object grenade;
        Object effect;
        if (ResourceManager.m_Instance != null)
        {
            grenade = ResourceManager.m_Instance.LoadData(typeof(GameObject), "Grenades", m_sGrenade, false);
            effect = ResourceManager.m_Instance.LoadData(typeof(GameObject), "Grenades", m_sEffect, false);
        }
        else
        {
            grenade = Resources.Load("Grenades/Grenade_Pumpkin");
            effect = Resources.Load("Grenades/Effect_Pumpkin");
        }
        if (ObjectPool.m_Instance == null) ObjectPool.m_Instance.Init();
        ObjectPool.m_Instance.InitGameObjects(grenade, count, id);
        ObjectPool.m_Instance.InitGameObjects(effect, count, id + 100);


        m_lActiveGrenades.Add(id);
	}

    public void Equipment(int id, int count)
    {
        bool bExist = false;
        for (int i = 0; i < m_lActiveGrenades.Count; i++)
        {
            if (m_lActiveGrenades[i] == id)
            {
                bExist = true;
                break;
            }
        }
        if (!bExist) AddGrenades(id);

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
