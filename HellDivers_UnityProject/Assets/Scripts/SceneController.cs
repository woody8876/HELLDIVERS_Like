using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    [ContextMenu("ToGame")]
    public void ToGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    [ContextMenu("ToLobby")]
    public void ToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}