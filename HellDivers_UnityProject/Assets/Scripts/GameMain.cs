using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
#pragma warning disable
    private AssetManager assetManager = new AssetManager();
    private ResourceManager resourceManager = new ResourceManager();
    private GameData gameData = new GameData();
    private ObjectPool objectPool;
#pragma warning disable

    private void Awake()
    {
        objectPool = this.gameObject.AddComponent<ObjectPool>();
        assetManager.Init();
        resourceManager.Init();
        gameData.LoadGameData();
    }

    // Use this for initialization
    private void Start()
    {
        PlayerInfo playerInfo = new PlayerInfo()
        {
            Rank = 1,
            DisplayID = "ch01",
            Hp = 100,
            WeaponId = new List<int> { 1 },
            StratagemId = new List<int> { 1, 1 }
        };

        GameObject playerGo = new GameObject("Player", typeof(Player), typeof(Weapon_Battle), typeof(StratagemController));
        playerGo.GetComponent<Player>().Info = playerInfo;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}