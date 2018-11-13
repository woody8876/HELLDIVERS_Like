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
    public float AvaliableRadius { get { return m_AvaliableRadius; } }
    public float ActiveTimer { get { return m_ActTimer; } }

    public float ActTimeCountDown
    {
        get
        {
            float result = (m_ActivatingTime < m_ActTimer) ? 0 : m_ActivatingTime - m_ActTimer;
            return result;
        }
    }

    public Vector3 Position { get { return this.transform.position; } }
    public CheckCodesMechine CodeMechine { get { return m_CodeMechine; } }
    public eCode[] Codes { get { return m_Codes; } }

    #endregion Properties

    #region Private Variable

    [SerializeField] private string m_Id = "MissionTower";
    [SerializeField] private float m_Radius = 4;
    [SerializeField] private float m_AvaliableRadius = 15;
    [SerializeField] private int m_CodeLenghtMin = 6;
    [SerializeField] private int m_CodeLenghtMax = 8;
    [SerializeField] private float m_ActivatingTime = 180;
    [SerializeField] private float m_MobSpawnTime = 30;
    [SerializeField] private float m_MinRadius = 20;
    [SerializeField] private float m_MaxRadius = 25;
    [SerializeField] private int m_MobNum = 5;
    private float m_ActTimer;
    private float m_MobTimer;
    private int m_CodeLenght;
    private GameObject m_TowerGo;
    private Animator m_Animator;
    private eCode[] m_Codes;
    private CheckCodesMechine m_CodeMechine;
    private TowerDoState DoState;

    private delegate void TowerDoState();

    public delegate void TowerEventHolder();

    public event TowerEventHolder OnActivating;

    public event TowerEventHolder OnStop;

    public event TowerEventHolder OnActive;

    #endregion Private Variable

    #region MonoBehaviour

    private void Awake()
    {
        m_CodeMechine = this.GetComponent<CheckCodesMechine>();
        m_CodeMechine.OnGetResult += SuccessOnCheckCode;
        m_CodeMechine.OnFaild += StartCheckCodes;
        m_Codes = GenerateCode();
        m_MobTimer = m_MobSpawnTime;
    }

    // Use this for initialization
    private void Start()
    {
        m_TowerGo = ResourceManager.m_Instance.LoadData(typeof(GameObject), "Mission/Towers", "Tower01") as GameObject;
        m_TowerGo = Instantiate(m_TowerGo, this.transform);
        m_Animator = m_TowerGo.GetComponentInChildren<Animator>();

        InteractiveItemManager.Instance.AddItem(this);
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
        DoState = CheckCodeState;
    }

    private void StopCheckCodes()
    {
        m_CodeMechine.StopCheckCodes();
        CurrentPlayer = null;
        m_Animator.SetTrigger("Stop");
        m_MobTimer = m_MobSpawnTime;
        DoState = null;
        if (OnStop != null) OnStop();
    }

    private void SuccessOnCheckCode()
    {
        m_Animator.SetTrigger("Start");
        m_ActTimer = 0;
        DoState = ActivationState;
    }

    /*-----------------------------
     * It's "Checking Code State" *
     -----------------------------*/

    private void CheckCodeState()
    {
        if (Vector3.Distance(this.transform.position, CurrentPlayer.transform.position) > m_AvaliableRadius)
        {
            StopCheckCodes();
        }
    }

    /*-----------------------------------------------------
     * It's "Activation State" timer, count on m_ActTimer *
     -----------------------------------------------------*/

    private void ActivationState()
    {
        if (Vector3.Distance(this.transform.position, CurrentPlayer.transform.position) > m_AvaliableRadius || CurrentPlayer.IsDead)
        {
            Debug.LogError("Out");
            StopCheckCodes();
        }

        if (m_ActTimer < m_ActivatingTime)
        {
            m_ActTimer += Time.fixedDeltaTime;

            if (m_MobTimer < m_MobSpawnTime)
            {
                m_MobTimer += Time.fixedDeltaTime;
            }
            else
            {
                MobManager.m_Instance.SpawnFish(m_MobNum, this.transform, m_MinRadius, m_MaxRadius);
                m_MobTimer = 0;
            }

            if (OnActivating != null) OnActivating();
        }
        else
        {
            m_Animator.SetTrigger("Finish");
            m_bFinished = true;
            InteractiveItemManager.Instance.RemoveItem(this);
            if (OnActive != null) OnActive();
            StartCoroutine(MissionFinish());
            DoState = null;
        }
    }

    private IEnumerator MissionFinish()
    {
        yield return new WaitUntil(CheckFinish);

        if (OnFinished != null) OnFinished(this);
        yield break;
    }

    private bool CheckFinish()
    {
        if (m_Animator.IsInTransition(0)) return false;
        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Finish") && stateInfo.normalizedTime >= 1) return true;
        else return false;
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