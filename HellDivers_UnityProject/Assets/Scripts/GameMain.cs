using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    /// <summary>
    /// Testing player info.
    /// </summary>
    public PlayerInfo m_PlayerData;

#pragma warning disable
    private AssetManager assetManager = new AssetManager();
    private ResourceManager resourceManager = new ResourceManager();
    private GameData gameData = new GameData();
    private ObjectPool objectPool = new ObjectPool();
#pragma warning disable

    private void Awake()
    {
        objectPool.Init();
        assetManager.Init();
        resourceManager.Init();
        gameData.Init();
    }

    // Use this for initialization
    private void Start()
    {
        if (m_PlayerData != null)
        {
            GameObject player = CreatPlayer(m_PlayerData);
            if (player != null) Camera.main.GetComponent<CameraFollowing>().FocusOnTarget(player.transform);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public GameObject CreatPlayer(PlayerInfo data)
    {
        if (data == null) return null;
        Transform spawnPos = null;
        if (MapInfo.Instance != null)
        {
            spawnPos = MapInfo.Instance.GetRandomSpawnPos();
        }

        if (spawnPos == null)
        {
            spawnPos = this.transform;
        }

        GameObject playerGo = Resources.Load("Characters/Ch00/ch00") as GameObject;
        playerGo = GameObject.Instantiate(playerGo, spawnPos);
        playerGo.transform.parent = null;

        Player p = playerGo.AddComponent<Player>();
        p.Initialize(data);

        return playerGo;
    }
}