using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; }
    private SceneController m_SceneController;
    private MusicManager m_MusicManager;
    private InputManager m_InputManager = new InputManager();
    private GameData m_GameData = new GameData();
    private PlayerManager m_PlayerManager = new PlayerManager();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        m_GameData.Init();
        m_SceneController = this.gameObject.AddComponent<SceneController>();
        m_MusicManager = this.gameObject.AddComponent<MusicManager>();
        m_PlayerManager.Init();
        m_InputManager.Init();
    }

    // Use this for initialization
    private void Start()
    {
        m_MusicManager.PlayMusic(eMusicSelection.Theme, 3);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    [ContextMenu("Print Player Info")]
    public void PrintPlayerInfo()
    {
        foreach (var player in PlayerManager.Instance.Players)
        {
            PlayerInfo info = player.Value.info;
            Debug.LogFormat("Player {0} : Rank {1}", player.Key, info.Rank);
            Debug.LogFormat("Player {0} : Exp {1}", player.Key, info.Exp);
        }
    }
}