using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMain : MonoBehaviour
{
    // Testing player info.

    #region Properties

    public static GameMain Instance { get; private set; }

    #endregion Properties

    private AssetManager m_AssetManager = new AssetManager();
    private ResourceManager m_ResourceManager = new ResourceManager();
    private ObjectPool m_ObjectPool = new ObjectPool();

    private void Awake()
    {
        m_AssetManager.Init();
        m_ResourceManager.Init();
        m_ObjectPool.Init();
    }

    private void Start()
    {
        if (MusicManager.Instance.CurrentSelection != eMusicSelection.Theme) MusicManager.Instance.PlayMusic(eMusicSelection.Theme, 2);
    }
}