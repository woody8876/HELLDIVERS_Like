using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadesController : MonoBehaviour
{
    public enum eSwitch { UP, LEFT, DOWN, RIGHT }

    #region Delegate of EventHolder

    public delegate void EventHolder();

    public EventHolder OnCount;
    public EventHolder OnChangeID;

    #endregion Delegate of EventHolder

    #region Public properties
    public int CurrentID { private set; get; }
    public int GrenadeCounter
    {
        get { return m_iCounter; }
        set
        {
            if (value > m_MaxCount) m_iCounter = m_MaxCount;
            else if (m_iCounter < 0) m_iCounter = 0;
            else m_iCounter = value;
        }
    }
    #endregion

    #region Public Method
    /// <summary>
    /// Equip grenades and set grenade to current active grenade, refresh grenade counter
    /// </summary>
    /// <param name="ids">Grenades' ID</param>
    /// <param name="t">Strat Position</param>
    public void AddGrenades(List<int> ids, Transform t)
    {
        foreach (int id in ids) { Equipment(id); }
        m_StarPos = t; 
    }
    /// <summary>
    /// Hold grenade for enhance throwing force
    /// </summary>
    /// <returns></returns>
    public bool Holding()
    {
        if (GrenadeCounter <= 0) { return false; }
        if (!m_bHolding) { LoadGrenade(); }
        m_Grenades.GetComponent<Grenades>().m_Force += 7 * Time.fixedDeltaTime;
        return true;
    }
    /// <summary>
    /// Release the button to throw the grenade
    /// </summary>
    public void Throw()
    {
        m_Grenades.GetComponent<Grenades>().Throw();
        m_bHolding = false;
        GrenadeCounter--;
        if (OnCount != null) OnCount();
    }
    /// <summary>
    ///  Input StratagemVertical or StratagemHorizontal to switch type of grenades
    /// </summary>
    public void SwitchGrenades()
    {
        eSwitch? input = GetESwitch();
        m_dActiveGrenades[CurrentID] = GrenadeCounter;
        switch (input)
        {
            case eSwitch.UP:
                Equipment(4002);
                break;
            case eSwitch.LEFT:
                Equipment(4003);
                break;
            case eSwitch.RIGHT:
                Equipment(4004);
                break;
            case eSwitch.DOWN:
                Equipment(4005);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Add the count of grenades with id
    /// </summary>
    /// <param name="id">Grenades' ID</param>
    /// <param name="count">Add count</param>
    public void AddGrenadesCount(int id, int count)
    {
        m_dActiveGrenades[id] += count;
        if (id == CurrentID)
        {
            GrenadeCounter = m_dActiveGrenades[id];
            if (OnCount != null) OnCount();
        }
    }
    #endregion

    #region Private method
    //Get switch code 
    private eSwitch? GetESwitch()
    {
        if (Input.GetAxisRaw(m_InputVertical) == 1) { return eSwitch.UP; }
        else if (Input.GetAxisRaw(m_InputVertical) == -1) { return eSwitch.DOWN; }
        else if (Input.GetAxisRaw(m_InputHorizontal) == -1) { return eSwitch.LEFT; }
        else if (Input.GetAxisRaw(m_InputHorizontal) == 1) { return eSwitch.RIGHT; }
        else { return null; }
    }
    //Create or equip grenades
    private void Equipment(int id, int count = 2)
    {
        if (m_dActiveGrenades.ContainsKey(id) == false) { CreateGrenades(id, count); }
        CurrentID = id;
        GrenadeCounter = m_dActiveGrenades[id];
        if (OnCount != null) OnCount();
        if (OnChangeID != null) OnChangeID();
    }
    //Set grenades active
    private void LoadGrenade()
    {
        m_Grenades = ObjectPool.m_Instance.LoadGameObjectFromPool(CurrentID);
        m_Grenades.transform.position = m_StarPos.position;
        m_Grenades.transform.forward = m_StarPos.forward;
        m_Grenades.SetActive(true);
        m_bHolding = true;
    }
    //Create new grenade whitch doesn't exist in dictionary
    private void CreateGrenades(int id, int count)
    {
        GrenadeInfo grenaderInfo = GameData.Instance.GrenadeInfoTable[id];
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

        m_MaxCount = count;
        m_dActiveGrenades.Add(id, count);
    }
    #endregion

    #region MonoBehaviors
    private void Start()
    {
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        SwitchGrenades();
        if (Input.GetKey(KeyCode.B)) { Holding(); }
        if (!Input.GetKey(KeyCode.B) && m_bHolding) { Throw(); }
    }
    #endregion

    #region Private field
    private Dictionary<int, int> m_dActiveGrenades = new Dictionary<int, int>();
    private GameObject m_Grenades;
    private Transform m_StarPos;
    private bool m_bHolding;
    private int m_iCounter;
    private int m_MaxCount;
    private string m_InputVertical = "StratagemVertical";
    private string m_InputHorizontal = "StratagemHorizontal";
    #endregion
}