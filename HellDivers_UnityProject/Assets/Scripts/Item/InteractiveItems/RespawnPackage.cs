using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPackage : MonoBehaviour
{
    [SerializeField] private float m_SapwnRadius = 5.0f;

    private void OnEnable()
    {
        if (InGamePlayerManager.Instance == null) return;

        Vector3 spawnPos = this.transform.position;
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(this.transform.position);
        if (screenPos.x <= 0 || screenPos.x > Camera.main.scaledPixelWidth || screenPos.y < 0 || screenPos.y > Camera.main.scaledPixelHeight)
        {
            List<Player> pList = InGamePlayerManager.Instance.Players;
            for (int i = 0; i < pList.Count; i++)
            {
                Player player = pList[i];
                if (player.IsDead == false)
                {
                    Vector3 dir = player.transform.forward;
                    spawnPos = Quaternion.Euler(0.0f, Random.Range(0, 360), 0.0f) * dir * m_SapwnRadius;
                    spawnPos += player.transform.position;
                }
            }
        }

        List<Player> players = InGamePlayerManager.Instance.Players;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].IsDead)
            {
                players[i].Spawn(spawnPos);
                if (ObjectPool.m_Instance != null) ObjectPool.m_Instance.UnLoadObjectToPool(2100, this.gameObject);
                break;
            }
        }
    }
}