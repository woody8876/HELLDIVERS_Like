using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadesController : MonoBehaviour {

    public int CurrentID { private set; get; }
    public int GrenadeCounter {
        private set
        {
            if (m_iCounter < 0) m_iCounter = 0;
            else m_iCounter = value;
        }
        get { return m_iCounter; }
    }
    
    #region Delegate of EventHolder
    public delegate void EventHolder();
    public EventHolder OnCount;
    public EventHolder OnChangeID;
    #endregion

    List<int> m_lActiveGrenades = new List<int>();
    GrenadeInfo grenaderInfo;
    GameObject m_Grenades;
    bool m_bHolding;
    int m_iCounter;


    /// <summary>
    /// Equip grenades and set grenade to current active grenade, refresh grenade counter
    /// </summary>
    /// <param name="id">Grenade's ID</param>
    /// <param name="count">The quality of grenade-equipment</param>
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
        if (!bExist) AddGrenades(id, count);
        CurrentID = id;
        GrenadeCounter = count;
        if (OnCount != null) OnCount();
        if (OnChangeID != null) OnChangeID();
    }

    public bool Holding()
    {
        if (GrenadeCounter <=0) { return false; }
        if (!m_bHolding) { LoadGrenade(); }
        m_Grenades.GetComponent<Grenades>().m_Force += 10 * Time.fixedDeltaTime;
        return true;
    }

    public void Throw()
    {
        m_Grenades.GetComponent<Grenades>().Throw();
        m_bHolding = false;
        GrenadeCounter--;
        if (OnCount != null) OnCount();
    }



    private void LoadGrenade()
    {
        m_Grenades = ObjectPool.m_Instance.LoadGameObjectFromPool(CurrentID);
        m_Grenades.transform.position = this.transform.position;
        m_Grenades.transform.forward = this.transform.forward;
        m_Grenades.SetActive(true);
        m_bHolding = true;
    }

	private void AddGrenades(int id, int count)
    {
        grenaderInfo = GameData.Instance.GrenadeInfoTable[id];
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
        ObjectPool.m_Instance.InitGameObjects(effect, (int)(count*0.5f), id + 100);

        m_lActiveGrenades.Add(id);
	}




    private void Start()
    {
        //Equipment(4001, 10);
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { Equipment(4001, 10); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { Equipment(4002, 10); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { Equipment(4003, 10); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { Equipment(4005, 10); }
        
        if (Input.GetKey(KeyCode.Space)) { Holding(); }
        if (!Input.GetKey(KeyCode.Space) && m_bHolding)  { Throw(); }
    }
}
