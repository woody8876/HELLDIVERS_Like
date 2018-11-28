using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WelcomeButtonController : MonoBehaviour {

    #region SerializeField
    [Header("== Button ==")]
    [SerializeField] Button m_Start;
    [SerializeField] Button m_Continue;
    [SerializeField] Button m_Exit;
    [SerializeField] Button m_Press;
    [SerializeField] Button m_LoadData1;
    [SerializeField] Button m_LoadData2;
    [SerializeField] Button m_LoadData3;
    [SerializeField] Button m_LoadData4;
    [Header("== Circle ==")]
    [SerializeField] GameObject m_Circle;
    [SerializeField] GameObject m_Circle2;
    [Header("== GameObject ==")]
    [SerializeField] GameObject m_Menu;
    [SerializeField] GameObject m_ContinueGO;
    [SerializeField] GameObject m_FadePanel;
    [SerializeField] GameObject m_Animantion;
    #endregion

    public ControllerSetting m_Controller { private set; get; }

    private DataSaver m_data;
    private float m_fTimer;
    private bool m_bSetting;

    #region Mono Behaviors
    private void Start()
    {
        SetPress();
        SetStart();
        SetContinue();
        SetExit();
        SetLoadData();
        m_fTimer = 0.2f;
    }

    private void LateUpdate()
    {
        if (!m_bSetting)
        {
            if (m_fTimer < 0)
            {
                EventSystem.current.SetSelectedGameObject(m_Press.gameObject);
                m_fTimer = 0.5f;
            }
            m_fTimer -= Time.fixedDeltaTime;
        }
    }
    #endregion

    #region Set Button event

    private void SetPress()
    {
        m_Press.onClick.AddListener(() => SetSelectGO(m_Continue));
        m_Press.onClick.AddListener(() => Change(m_Press.gameObject, m_Menu));
        m_Press.onClick.AddListener(() => m_bSetting = true);
    }

    private void SetStart()
    {
        AddEvent(m_Start);
        m_Start.onClick.AddListener(() => DataLoader(1));
        m_Start.onClick.AddListener(() => StartCoroutine(ChangePanel()));
    }

    private void SetContinue()
    {
        AddEvent(m_Continue);
        m_Continue.onClick.AddListener(() => SetSelectGO(m_LoadData1));
        m_Continue.onClick.AddListener(() => Change(m_Menu, m_ContinueGO));
    }

    private void SetLoadData()
    {
        AddEvent(m_LoadData1, false);
        AddEvent(m_LoadData2, false);
        AddEvent(m_LoadData3, false);
        AddEvent(m_LoadData4, false);
        m_LoadData1.onClick.AddListener(() => DataLoader(1));
        m_LoadData2.onClick.AddListener(() => DataLoader(2));
        m_LoadData3.onClick.AddListener(() => DataLoader(3));
        m_LoadData4.onClick.AddListener(() => DataLoader(4));
        m_LoadData1.onClick.AddListener(()=> StartCoroutine(ChangeScene()));
        m_LoadData2.onClick.AddListener(()=> StartCoroutine(ChangeScene()));
        m_LoadData3.onClick.AddListener(()=> StartCoroutine(ChangeScene()));
        m_LoadData4.onClick.AddListener(()=> StartCoroutine(ChangeScene()));
    }

    private void SetExit()
    {
        AddEvent(m_Exit);
        m_Exit.onClick.AddListener(() => Application.Quit());
    }
    //For Start
    IEnumerator ChangePanel()
    {
        CloseButton();
        yield return new WaitForSeconds(.8f);
        Change(m_Menu, m_Animantion);
        this.gameObject.SetActive(false);
        
    }
    //For Continue
    IEnumerator ChangeScene()
    {
        CloseButton();
        yield return new WaitForSeconds(1.5f);
        SceneController.Instance.ToLobby();
    }

    #endregion

    #region Method
    //Disable button control 
    private void CloseButton()
    {
        m_FadePanel.SetActive(true);
        m_Start.interactable = false;
        m_Continue.interactable = false;
        m_Exit.interactable = false;
    }
    //Add event into button
    private void AddEvent(Button btn, bool circle1 = true)
    {
        EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        if (circle1) entry.callback.AddListener((eventdata) => SwitchCircle(btn.gameObject));
        else entry.callback.AddListener((eventdata) => SwitchCircle(btn.gameObject, false));
        trigger.triggers.Add(entry);
    }
    //Move the circle to the front of the chosen button
    private void SwitchCircle(GameObject go, bool circle1 = true)
    {
        if (circle1)
        {
            Vector3 pos = m_Circle.transform.position;
            pos.y = go.transform.position.y;
            m_Circle.transform.position = pos;
        }
        else
        {
            Vector3 pos = m_Circle2.transform.position;
            pos.y = go.transform.position.y;
            m_Circle2.transform.position = pos;
        }
    }
    //Change active panel
    private void Change(GameObject pre, GameObject next)
    {
        pre.SetActive(false);
        next.SetActive(true);
    }
    //Set next highlight button
    private void SetSelectGO(Button next)
    {
        EventSystem.current.SetSelectedGameObject(next.gameObject, null);
    }
    //Set controller
    public void SetController(ControllerSetting controller)
    {
        m_Controller = controller;
    } 

    #endregion

    #region Start 

    private void CreateData()
    {
        PlayerManager.Instance.CreatePlayer(1, m_Controller, m_data);
    }

    private void LoadData()
    {
        PlayerManager.Instance.LoadPlayer(1, m_Controller, m_data);
    }

    private void DataLoader(int i, bool load = true)
    {
        m_data = DataSaverManager.Instance.DataSaverMap[i];
        if (!load) CreateData();
        else LoadData();
    }

    #endregion

}
