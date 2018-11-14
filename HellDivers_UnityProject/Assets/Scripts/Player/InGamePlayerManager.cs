using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HELLDIVERS.UI.InGame;

public class InGamePlayerManager : MonoBehaviour
{
    public static InGamePlayerManager Instance { get; private set; }
    public List<Player> Players { get { return m_Players; } }
    public float SpawnRadius { get { return m_SpawnRadius; } set { m_SpawnRadius = (value < 0) ? 0 : value; } }
    public float RespawnTime { get { return m_RespawnTime; } set { m_RespawnTime = (value < 0) ? 0 : value; } }
    [SerializeField] private float m_SpawnRadius = 10.0f;
    [SerializeField] private float m_RespawnTime = 5.0f;
    private List<Player> m_Players;
    private Dictionary<Player, PlayerStates> m_PlayerMap;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        m_Players = new List<Player>();
        m_PlayerMap = new Dictionary<Player, PlayerStates>();
    }

    public void CreatePlayer(PlayerInfo playerInfo)
    {
        GameObject playerGo = ResourceManager.m_Instance.LoadData(typeof(GameObject), "Characters/Ch00", "ch00") as GameObject;
        playerGo = GameObject.Instantiate(playerGo);
        Player player = playerGo.AddComponent<Player>();
        player.Initialize(playerInfo);
        PlayerStates states = new PlayerStates(player);
        m_PlayerMap.Add(player, states);
        m_Players.Add(player);
        playerGo.SetActive(false);

        // Create player info UI
        UIInGameMain.Instance.AddPlayer(player);
    }

    public void SpawnPlayers()
    {
        if (m_PlayerMap.Count <= 0) return;
        if (MapInfo.Instance == null) return;

        int indexSpawnPos = Random.Range(0, MapInfo.Instance.SpawnPos.Count - 1);
        Transform spawnPos = MapInfo.Instance.SpawnPos[indexSpawnPos];

        foreach (KeyValuePair<Player, PlayerStates> player in m_PlayerMap)
        {
            player.Key.transform.position = spawnPos.position;
            player.Key.gameObject.SetActive(true);

            if (GameMain.Instance.CameraFolloing.Targets.Count <= 0)
            {
                GameMain.Instance.CameraFolloing.FocusOnTarget(player.Key.transform);
            }
            else
            {
                GameMain.Instance.CameraFolloing.AddTarget(player.Key.transform);
            }
        }
    }

    public void RespawnPlayer(Player player)
    {
        if (m_Players.Count >= 2) return;
        StartCoroutine(RespawnProcess(player));
    }

    private IEnumerator RespawnProcess(Player player)
    {
        yield return new WaitForSeconds(RespawnTime);
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Enemies") | 1 << LayerMask.NameToLayer("Obstacle");
        Vector3 spawnPos = player.transform.position;
        bool bBlock;
        do
        {
            spawnPos = player.transform.position;
            float radius = Random.Range(0, SpawnRadius);
            spawnPos += Quaternion.Euler(0.0f, Random.Range(0, 360), 0.0f) * Vector3.forward * radius;
            bBlock = Physics.CheckSphere(spawnPos, 1.5f, layerMask);
        } while (bBlock);

        player.Spawn(spawnPos);
    }

    private void PlayerDeathCount(Player player)
    {
        m_PlayerMap[player].TimesOfDeath++;
    }

    public class PlayerStates
    {
        public PlayerStates(Player player)
        {
            m_Player = player;
        }

        public Player m_Player;
        public int TimesOfDeath;
    }
}