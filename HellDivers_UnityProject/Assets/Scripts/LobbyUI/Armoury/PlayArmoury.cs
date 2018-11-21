using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArmoury : MonoBehaviour {

    [SerializeField] GameObject m_PlayerInGame;

    #region Private Field
    ControllerSetting m_controller;
    Dictionary<int, bool> m_dPlayerReady = new Dictionary<int, bool>();
    bool b_AddPlayer;
    bool b_Loding;
    int[] Members
    {
        get
        {
            var key = new int[PlayerManager.Instance.Players.Count];
            PlayerManager.Instance.Players.Keys.CopyTo(key, 0);
            return key;
        }
    }
    #endregion

    #region MonoBehaviors

    void Start () {
        foreach (var item in Members)
        {
            CreatPlayerMenu(item);
        }
        SetController();
	}

    void LateUpdate () {
        if (CheckState() && !b_Loding)
        {
            SceneController.Instance.ToGameScene();
            b_Loding = true;
        }
        if (b_AddPlayer) return;
        if (!PlayerManager.Instance.Players.ContainsKey(2) || !PlayerManager.Instance.Players[2].controllerSetting)
        {
            if (Input.GetKey(m_controller.Submit))
            {
                PlayerManager.Instance.CreatePlayer(2, m_controller);
                CreatPlayerMenu(2);
                b_AddPlayer = true;
            }
        }
	}

    #endregion

    #region Private Method

    private void SetController()
    {
        m_controller = (PlayerManager.Instance.Players[1].controllerSetting == InputManager.Instance.InputSettingMap[1]) ? InputManager.Instance.InputSettingMap[2] : InputManager.Instance.InputSettingMap[1];
    }

    private void CreatPlayerMenu(int i)
    {
        Vector3 pos = new Vector3(150 + 810 * (i - 1), 780, 0);
        Quaternion rotate = new Quaternion(0, 0, 0, 1);
        GameObject go = Instantiate(m_PlayerInGame, pos, rotate, this.transform);
        go.name = "Player" + i;
        //go.GetComponentInChildren<SetPlayerWeapon>().PlayerID = i;
        go.GetComponent<ControlEvent>().SetID(i);
        m_dPlayerReady.Add(i, false);
    }

    private bool CheckState()
    {
        for (int i = 1; i < m_dPlayerReady.Count + 1 ; i++)
        {
            if (m_dPlayerReady[i] == false) return false;
        }
        return true;
    }

    #endregion

    public void SetPlayerState(int i, bool ready = true)
    {
        m_dPlayerReady[i] = ready;
    }
}
