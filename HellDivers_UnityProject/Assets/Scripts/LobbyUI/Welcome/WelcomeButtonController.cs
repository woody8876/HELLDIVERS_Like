using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WelcomeButtonController : MonoBehaviour {

    #region SerializeField
    [SerializeField] Button m_Start;
    [SerializeField] Button m_Continue;
    [SerializeField] Button m_Exit;
    [SerializeField] GameObject m_Circle;
    [SerializeField] GameObject m_Press;
    [SerializeField] GameObject m_Menu;
    [SerializeField] GameObject m_FadePanel;
    [SerializeField] GameObject m_Animantion;
    #endregion

    private float m_fTimer;
    private ControllerSetting m_controller;
    private bool m_bSetting;
    #region Mono Behaviors
    private void Start()
    {
        SetPress();
        SetStart();
        SetContinue();
        SetExit();
        m_fTimer = 0.2f;
    }

    private void LateUpdate()
    {
        if (!m_bSetting)
        {
            if (!PlayerManager.Instance.Players[1].controllerSetting) return;
            else
            {
                m_controller = PlayerManager.Instance.Players[1].controllerSetting;
                m_bSetting = true;
            }
        }

        if (m_fTimer < 0)
        {
            if (Input.GetKey(m_controller.Submit))
            {
                Button btn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                btn.onClick.Invoke();
                m_fTimer = 0.2f;
            }
        }
        m_fTimer -= Time.fixedDeltaTime;
    }
    #endregion

    #region Set Button event
    private void SetPress()
    {
        Button btn = m_Press.GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => SetSelectGO(m_Continue));
        btn.onClick.AddListener(() => Change(m_Press, m_Menu));
        btn.onClick.AddListener(() => Debug.Log("click"));
    }

    private void SetStart()
    {
        AddEvent(m_Start);
        m_Start.onClick.AddListener(() => StartCoroutine(ChangePanel()));
    }

    private void SetContinue()
    {
        AddEvent(m_Continue);
        m_Continue.onClick.AddListener(()=> StartCoroutine(ChangeScene()));
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
    private void AddEvent(Button btn)
    {
        EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((eventdata) => SwitchCircle(btn.gameObject));
        trigger.triggers.Add(entry);
    }
    //Move the circle to the front of the chosen button
    private void SwitchCircle(GameObject go)
    {
        Vector3 pos = m_Circle.transform.position;
        pos.y = go.transform.position.y;
        m_Circle.transform.position = pos;
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
    #endregion
}
