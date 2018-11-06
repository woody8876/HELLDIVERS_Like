using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    // Testing player info.
    public PlayerInfo m_PlayerData;

    #region Properties

    public static GameMain Instance { get; private set; }
    public List<Player> Players { get { return m_Players; } }
    public CameraFollowing CameraFolloing { get { return m_CameraFollowing; } }

    #endregion Properties

    private AssetManager m_AssetManager = new AssetManager();
    private ResourceManager m_ResourceManager = new ResourceManager();
    private ObjectPool m_ObjectPool = new ObjectPool();
    private GameData m_GameData = new GameData();
    private InteractiveItemManager m_ItemManager = new InteractiveItemManager();
    private List<Player> m_Players = new List<Player>();
    private MobManager m_MobSpawner = new MobManager();
    private CameraFollowing m_CameraFollowing;
    [SerializeField] private float m_RespawnTime = 1;
    private float m_SpawnRadius = 10;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        m_AssetManager.Init();
        m_ResourceManager.Init();
        m_ObjectPool.Init();
        m_GameData.Init();
        m_ItemManager.Init();
        m_MobSpawner.Init();
        m_CameraFollowing = Camera.main.GetComponent<CameraFollowing>();
    }

    // Use this for initialization
    private void Start()
    {
        if (m_PlayerData != null) CreatPlayer(m_PlayerData);

        m_MobSpawner.SpawnPatrol(40);
        InvokeRepeating("SpawnMobs", 0.0f, 3.0f);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void RespawnPlayer(Player player)
    {
        StartCoroutine(RespawnProcess(player));
    }

    private IEnumerator RespawnProcess(Player player)
    {
        yield return new WaitForSeconds(m_RespawnTime);
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Enemies") | 1 << LayerMask.NameToLayer("Obstacle");
        Vector3 spawnPos = player.transform.position;
        bool bBlock;
        do
        {
            spawnPos = player.transform.position;
            float radius = Random.Range(0, m_SpawnRadius);
            spawnPos += Quaternion.Euler(0.0f, Random.Range(0, 360), 0.0f) * Vector3.forward * radius;
            bBlock = Physics.CheckSphere(spawnPos, 1.5f, layerMask);
        } while (bBlock);

        player.Spawn(spawnPos);
    }

    public GameObject CreatPlayer(PlayerInfo data)
    {
        GameObject playerGo = ResourceManager.m_Instance.LoadData(typeof(GameObject), "Characters/Ch00", "ch00") as GameObject;
        playerGo = GameObject.Instantiate(playerGo);
        Player player = playerGo.AddComponent<Player>();
        player.Initialize(data);
        m_Players.Add(player);

        Transform spawnPos = null;
        if (MapInfo.Instance != null) spawnPos = MapInfo.Instance.GetRandomSpawnPos();
        if (spawnPos == null) spawnPos = this.transform;
        player.Spawn(spawnPos.position);

        // Camera start following player
        if (m_Players.Count == 1) m_CameraFollowing.FocusOnTarget(player.transform);
        else m_CameraFollowing.AddTarget(player.transform);

        return playerGo;
    }

    private void SpawnMobs()
    {
        m_MobSpawner.SpawnFish(5);
    }
}