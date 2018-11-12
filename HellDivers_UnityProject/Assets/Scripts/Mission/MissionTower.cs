using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

[RequireComponent(typeof(CheckCodesMechine))]
public class MissionTower : Mission, IInteractable
{
    #region Properties

    public Player CurrentPlayer { get; private set; }
    public string ID { get { return m_Id; } }
    public float Radius { get { return m_Radius; } }
    public Vector3 Position { get { return this.transform.position; } }
    public eCode[] Codes { get { return m_Codes; } }

    #endregion Properties

    #region Private Variable

    [SerializeField] private string m_Id = "MissionTower";
    [SerializeField] private float m_Radius = 4;
    [SerializeField] private int m_CodeLenghtMin = 6;
    [SerializeField] private int m_CodeLenghtMax = 8;
    [SerializeField] private float m_ActivatingTime = 5;
    private float m_ActTimer;
    private int m_CodeLenght;
    private GameObject m_TowerGo;
    private Animator m_Animator;
    private eCode[] m_Codes;
    private CheckCodesMechine m_CodeMechine;
    private TowerDoState DoState;

    private delegate void TowerDoState();

    #endregion Private Variable

    public void StartMission()
    {
    }

    #region MonoBehaviour

    // Use this for initialization
    private void Start()
    {
        m_TowerGo = ResourceManager.m_Instance.LoadData(typeof(GameObject), "Mission/Towers", "Tower01") as GameObject;
        m_TowerGo = Instantiate(m_TowerGo, this.transform);
        m_Animator = m_TowerGo.GetComponentInChildren<Animator>();

        InteractiveItemManager.Instance.AddItem(this);
        m_CodeMechine = this.GetComponent<CheckCodesMechine>();
        m_CodeMechine.OnGetResult += SuccessOnCheckCode;
        m_CodeMechine.OnStop += StartCheckCodes;
        m_Codes = GenerateCode();
    }

    private void FixedUpdate()
    {
        if (DoState != null) DoState();
    }

    private void OnDisable()
    {
        InteractiveItemManager.Instance.RemoveItem(this);
    }

    private void OnDestroy()
    {
        m_CodeMechine.OnGetResult -= SuccessOnCheckCode;
    }

    #endregion MonoBehaviour

    #region Public Function

    public void OnInteract(Player player)
    {
        if (CurrentPlayer != null) return;
        CurrentPlayer = player;
        StartCheckCodes();
    }

    #endregion Public Function

    #region Private Function

    private void StartCheckCodes()
    {
        m_CodeMechine.Clear();
        m_CodeMechine.AddCodes(this, m_Codes);
        m_CodeMechine.StartCheckCodes();
    }

    private void SuccessOnCheckCode()
    {
        m_Animator.SetTrigger("Start");
        m_ActTimer = 0;
        DoState = ActivationState;
    }

    /*-----------------------------------------------------
     * It's "Activation State" timer, count on m_ActTimer *
     -----------------------------------------------------*/

    private void ActivationState()
    {
        if (m_ActTimer < m_ActivatingTime) m_ActTimer += Time.fixedDeltaTime;
        else
        {
            m_Animator.SetTrigger("Finish");
            DoState = null;
        }
    }

    private void FinalState()
    {
        m_bFinished = true;
        InteractiveItemManager.Instance.RemoveItem(this);
        if (OnFinished != null) OnFinished();
    }

    /*------------------------------------------------------------------------
     * Create random codes lenth between m_CodeLenghtMin amd m_CodeLenghtMax *
     ------------------------------------------------------------------------*/

    private eCode[] GenerateCode()
    {
        m_CodeLenght = UnityEngine.Random.Range(m_CodeLenghtMin, m_CodeLenghtMax);
        eCode[] codes = new eCode[m_CodeLenght];

        for (int i = 0; i < m_CodeLenght; i++)
        {
            int codeChar = UnityEngine.Random.Range(1, 4);
            eCode ecode = (eCode)Enum.Parse(typeof(eCode), codeChar.ToString());
            codes[i] = ecode;
        }

        return codes;
    }

    #endregion Private Function

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireArc(this.transform.position, Vector3.up, this.transform.forward, 360f, Radius);
    }

#endif
}