using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; }
    private SceneController m_SceneController;
    private GameData m_GameData = new GameData();
    private PlayerManager m_PlayerManager = new PlayerManager();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        m_GameData.Init();
        m_SceneController = this.gameObject.AddComponent<SceneController>();
        m_PlayerManager.Init();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}