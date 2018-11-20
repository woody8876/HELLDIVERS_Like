using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArmoury : MonoBehaviour {

    [SerializeField] GameObject m_PlayerInGame;

    ControllerSetting m_controller;
    bool b_AddPlayer;

    int[] Members
    {
        get
        {
            var key = new int[PlayerManager.Instance.Players.Count];
            PlayerManager.Instance.Players.Keys.CopyTo(key, 0);
            return key;
        }
    }
	// Use this for initialization
	void Start () {
        foreach (var item in Members)
        {
            CreatPlayerMenu(item);
        }
        SetController();
	}
	
    public void SetController()
    {
        m_controller = (PlayerManager.Instance.Players[1].controllerSetting == InputManager.Instance.InputSettingMap[1]) ? InputManager.Instance.InputSettingMap[2] : InputManager.Instance.InputSettingMap[1];
    }

    public void CreatPlayerMenu(int i)
    {
        Vector3 pos = new Vector3(150 + 810 * (i - 1), 780, 0);
        Quaternion rotate = new Quaternion(0, 0, 0, 1);
        GameObject go = Instantiate(m_PlayerInGame, pos, rotate, this.transform);
        go.name = "Player" + i;
        go.GetComponentInChildren<SetPlayerWeapon>().PlayerID = i;
    }

    // Update is called once per frame
    void Update () {
        if (PlayerManager.Instance.Players[2] == null)
        {
            if (Input.GetKey(m_controller.Submit))
            {
                PlayerManager.Instance.CreatePlayer(2, m_controller);
                CreatPlayerMenu(2);
            }
        }
	}
}
