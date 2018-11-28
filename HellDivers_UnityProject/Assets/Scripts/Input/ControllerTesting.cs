using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTesting : MonoBehaviour {

    [SerializeField] WelcomeButtonController buttonController;
    [SerializeField] AxisEvent axis;
    private ControllerSetting m_ControllerSetting1;
    private ControllerSetting m_ControllerSetting2;

    bool m_Interactive;

    // Use this for initialization
    void Start () {
        m_ControllerSetting1 = InputManager.Instance.InputSettingMap[1];
        m_ControllerSetting2 = InputManager.Instance.InputSettingMap[2];
        //if (!PlayerManager.Instance.Players.ContainsKey(1) && !PlayerManager.Instance.Players.ContainsKey(2))
        //{
        //    PlayerManager.Instance.CreatePlayer(1, m_ControllerSetting1);
        //}

    }

    // Update is called once per frame
    void Update () {
        if (m_Interactive) return;
		if (Input.GetKey(m_ControllerSetting1.Submit))
        {
            buttonController.SetController(m_ControllerSetting1);
            axis.SetController(m_ControllerSetting1);
            m_Interactive = true;
        }
        else if(Input.GetKey(m_ControllerSetting2.Submit))
        {
            buttonController.SetController(m_ControllerSetting2);
            axis.SetController(m_ControllerSetting2);
            m_Interactive = true;
        }
	}
}
