using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Creater : MonoBehaviour {

    [SerializeField] PlayArmoury m_Armoury;
    [SerializeField] GameObject m_Waiting;
    [SerializeField] GameObject m_SelectData;
    [SerializeField] GameObject m_Button;
    [SerializeField] GameObject m_NewGame;
    [SerializeField] GameObject m_Load;



    ControllerSetting m_controller;
    DataSaver m_data;
    GameObject m_current;
    bool b_Select;

    #region MonoBehaviors

    private void Start()
    {
        SetController();
        SetData();
    }

    private void LateUpdate()
    {
        if (b_Select)
        {
            Selecting(); 
        }
        else if (Input.GetKey(m_controller.Start))
        {
            ChangeGO(m_Waiting, m_SelectData);
            m_current = m_NewGame;
            b_Select = true;
        }

    }

    #endregion

    #region Method

    private void Selecting()
    {
        if (Input.GetAxis(m_controller.Vertical) > 0)
        {
            m_current = m_NewGame;
            SetButtonPos();
        }
        if (Input.GetAxis(m_controller.Vertical) < 0)
        {
            m_current = m_Load;
            SetButtonPos();
        }
        if (Input.GetKey(m_controller.Cancel))
        {
            ChangeGO(m_SelectData, m_Waiting);
            b_Select = false;
        }
        if (Input.GetKey(m_controller.Submit))
        {
            if (m_current == m_NewGame) { PlayerManager.Instance.CreatePlayer(2, m_controller, m_data); }
            else if (m_current == m_Load) { PlayerManager.Instance.LoadPlayer(2, m_controller, m_data); }
            m_Armoury.CreatPlayerMenu(2);
            this.gameObject.SetActive(false); 
        }
    }

    private void SetController()
    {
        m_controller = (PlayerManager.Instance.Players[1].controllerSetting == InputManager.Instance.InputSettingMap[1]) ? InputManager.Instance.InputSettingMap[2] : InputManager.Instance.InputSettingMap[1];
    }

    private void SetData()
    {
        m_data = DataSaverManager.Instance.DataSaverMap[2];
    }

    private void SetButtonPos()
    {
        Vector3 pos = m_Button.transform.position;
        pos.y = m_current.transform.position.y;
        m_Button.transform.position = pos;
    }

    private void ChangeGO(GameObject pre, GameObject next)
    {
        pre.SetActive(false);
        next.SetActive(true);
    }

    #endregion
}
