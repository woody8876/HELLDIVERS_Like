using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Stratagem : MonoBehaviour
{
    #region Properties

    public StratagemInfo Info { get; private set; }
    public EState State { get { return state; } }
    public int UsesCount { get { return m_iUsesCount; } }
    public float CoolTimer { get { return m_fCoolTimer; } }
    public float ActTimer { get { return m_fActTimer; } }

    #endregion Properties

    #region PrivateVariable

    private Transform m_LaunchPos;
    private GameObject m_Display;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;

    private float m_Radius = 0.1f;
    private int m_iUsesCount = 0;
    private float m_fCoolTimer = 0.0f;
    private float m_fActTimer = 0.0f;

    private DoState m_DoState;

    #endregion PrivateVariable

    #region Initializer

    /// <summary>
    /// Setup stratagem with id form gamedata table.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool SetStratagemInfo(int id, Transform launchPos)
    {
        StratagemInfo newInfo = GetInfoFromGameData(id);
        if (newInfo == null || newInfo == this.Info) return false;
        this.Info = newInfo;

        GameObject o = ResourceManager.m_Instance.LoadData(typeof(GameObject), StratagemSystem.DisplayFolder, newInfo.display) as GameObject;
        if (m_Display != o)
        {
            if (o == null) o = StratagemSystem.DefaultDisplay;

            DestroyImmediate(m_Display);
            m_Display = Instantiate(o, this.transform.position, Quaternion.identity, this.transform);
            m_Animator = m_Display.GetComponent<Animator>();
        }

        m_LaunchPos = launchPos;
        this.transform.parent = m_LaunchPos;
        this.transform.localPosition = Vector3.zero;

        m_iUsesCount = 0;
        m_fCoolTimer = 0.0f;
        m_fActTimer = 0.0f;
        return true;
    }

    /// <summary>
    /// Get stratagem info with id from game data tables. If it does not exist return null.
    /// </summary>
    /// <param name="id">stratagem id which in the table</param>
    /// <returns></returns>
    private StratagemInfo GetInfoFromGameData(int id)
    {
        if (GameData.Instance.StratagemTable.ContainsKey(id) == false)
        {
            Debug.LogErrorFormat("Stratagem Error : Can't found ID : [{0}] from game data", id);
            return null;
        }

        return GameData.Instance.StratagemTable[id];
    }

    #endregion Initializer

    #region Interaction

    /// <summary>
    /// Add force to this gameobject for throw it out.
    /// </summary>
    public void Throw(Vector3 force)
    {
        this.transform.parent = null;
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.AddForce(force);
    }

    #endregion Interaction

    #region MonoBehaviour

    // Use this for initialization
    private void Start()
    {
        m_Rigidbody = this.GetComponent<Rigidbody>();
        m_Rigidbody.isKinematic = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_DoState != null) m_DoState();
    }

    #endregion MonoBehaviour

    #region StateMachine

    private delegate void DoState();

    private EState state = EState.Standby;

    public enum EState
    {
        Standby, Ready, ThrowOut, Activating, CoolingDown
    }

    public void ToReady()
    {
    }

    private void DoThrowOut()
    {
        RaycastHit raycastHit;
        Physics.SphereCast(this.transform.position, m_Radius, Vector3.down, out raycastHit);
        if (raycastHit.transform.tag == "Terrain")
        {
            m_Rigidbody.isKinematic = true;
            this.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

#if UNITY_EDITOR
        Gizmos.DrawWireSphere(this.transform.position, m_Radius);
#endif
    }

    private void DoCoolDown()
    {
        if (m_fCoolTimer >= Info.cooldown)
        {
            m_fCoolTimer = 0.0f;
        }
        m_fCoolTimer += Time.deltaTime;
    }

    #endregion StateMachine
}