using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadesController : MonoBehaviour
{
    public enum eSwitch { UP, LEFT, DOWN, RIGHT }
    #region Public Method

    /// <summary>
    /// Equip grenades and set grenade to current active grenade, refresh grenade counter
    /// </summary>
    /// <param name="ids">Grenades' ID</param>
    /// <param name="t">Strat Position</param>
    public void AddGrenades(List<int> ids, Transform startPos, Transform throwPos)
    {
        foreach (int id in ids) { Equipment(id); }
        m_StarPos = startPos;
        m_ThrowPos = throwPos;
    }

    /// <summary>
    /// Hold grenade for enhance throwing force
    /// </summary>
    /// <returns></returns>
    public bool Holding()
    {
        if (GrenadeCounter <= 0) { return false; }

        if (!m_bHolding) { LoadGrenade(); }
        if (m_Grenades == null) { return false; }
        m_Grenades.GetComponent<Grenades>().m_Force += 5 * Time.fixedDeltaTime;
        return true;
    }
    /// <summary>
    /// Grenade's follow
    /// </summary>
    public void Following()
    {
        m_Grenades.transform.position = m_StarPos.position;
        m_Grenades.transform.forward = m_StarPos.forward;
    }

    /// <summary>
    /// Release the button to throw the grenade
    /// </summary>
    public void Throw()
    {
        if (!m_bHolding) return;
        m_iCounter--;
        m_dActiveGrenades[CurrentID] = m_iCounter;
        m_Grenades.transform.position = m_ThrowPos.position;
        m_Grenades.transform.forward = m_ThrowPos.forward;
        m_Grenades.transform.localScale *= 5f;
        m_Grenades.GetComponent<Grenades>().Throw();
        m_bHolding = false;
        if (OnCount != null) OnCount();
    }

    /// <summary>
    ///  Input StratagemVertical or StratagemHorizontal to switch type of grenades
    /// </summary>
    public void SwitchGrenades()
    {
        eSwitch? input = GetESwitch();
        if (Keys.Length <= 1) { return; }
        switch (input)
        {
            case eSwitch.UP:
                
                Equipment(Keys[0]);
                break;

            case eSwitch.LEFT:
                Equipment(Keys[1]);
                break;

            case eSwitch.RIGHT:
                if (Keys.Length < 3) { return; }
                Equipment(Keys[2]);
                break;

            case eSwitch.DOWN:
                if (Keys.Length < 4) { return; }
                Equipment(Keys[3]);
                break;

            default:
                break;
        }
        //m_dActiveGrenades[CurrentID] = GrenadeCounter;
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
            m_iCounter = m_dActiveGrenades[id];
            if (OnCount != null) OnCount();
        }
    }
    
    public void ResetGrenades()
    {
        for (int i = 0; i < Keys.Length; i++)
        {
            m_dActiveGrenades[Keys[i]] = 2;
        }
        m_iCounter = m_dActiveGrenades[m_iCurrentID];
    }

    #endregion Public Method

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
        m_iCurrentID = id;
        m_iCounter = m_dActiveGrenades[id];
        if (OnCount != null) OnCount();
        if (OnChangeID != null) OnChangeID();
    }

    //Set grenades active
    private void LoadGrenade()
    {
        m_Grenades = ObjectPool.m_Instance.LoadGameObjectFromPool(CurrentID)?? null;        
        if (m_Grenades == null) { return; }
        m_bHolding = true;
        m_Grenades.transform.localScale *= 0.2f;
        m_Grenades.SetActive(true);
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

    #endregion Private method

    #region Delegate of EventHolder

    public delegate void EventHolder();

    public EventHolder OnCount;
    public EventHolder OnChangeID;

    #endregion Delegate of EventHolder

    #region Public properties

    public int CurrentID { get { return m_iCurrentID; } }
    public Dictionary<int, int> ActiveGrenades { get { return m_dActiveGrenades; } }

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

    #endregion Public properties


    #region Private field

    private Dictionary<int, int> m_dActiveGrenades = new Dictionary<int, int>();
    private int[] Keys {
        get
        {
            int[] keys = new int[m_dActiveGrenades.Count];
            m_dActiveGrenades.Keys.CopyTo(keys, 0);
            return keys;
        }
    }
    private GameObject m_Grenades;
    private Transform m_StarPos;
    private Transform m_ThrowPos;
    private bool m_bHolding;

    [SerializeField]
    private int m_iCurrentID;

    private int m_iCounter;
    private int m_MaxCount;
    private string m_InputVertical = "StratagemVertical";
    private string m_InputHorizontal = "StratagemHorizontal";

    #endregion Private field

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetGrenades();
        }
    }
}