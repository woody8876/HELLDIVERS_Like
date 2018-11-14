using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPackage : MonoBehaviour
{
    private void OnEnable()
    {
        if (InGamePlayerManager.Instance == null) return;

        List<Player> players = InGamePlayerManager.Instance.Players;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].IsDead)
            {
                players[i].Spawn(this.transform.position);
                if (ObjectPool.m_Instance != null) ObjectPool.m_Instance.UnLoadObjectToPool(2100, this.gameObject);
                break;
            }
        }
    }
}