using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIPanelPlayerInfo : MonoBehaviour
    {
        public static UIPanelPlayerInfo Instance { get { return m_Instance; } }
        private static UIPanelPlayerInfo m_Instance;

        private List<Player> m_Players;
        private GameObject m_UIPlayerInfoElementPrefab;
        private Dictionary<Player, UIPlayerInfoElement> m_PlayerInfoUIMap;

        public void AddPlayer(Player player)
        {
            UIPlayerInfoElement uiPlayerInfoElement = Instantiate(m_UIPlayerInfoElementPrefab, this.transform).GetComponent<UIPlayerInfoElement>();
            uiPlayerInfoElement.Init(player);

            m_PlayerInfoUIMap.Add(player, uiPlayerInfoElement);
        }

        public void RemovePlayer(Player player)
        {
            if (m_PlayerInfoUIMap.ContainsKey(player) == false) return;

            Destroy(m_PlayerInfoUIMap[player].gameObject);
            m_PlayerInfoUIMap.Remove(player);
        }

        private void Awake()
        {
            if (m_Instance == null) m_Instance = this;
            else Destroy(this.gameObject);
            m_UIPlayerInfoElementPrefab = ResourceManager.m_Instance.LoadData(typeof(GameObject), "UI/InGame/PanelPlayersInfo", "PlayerInfoElement") as GameObject;
            m_PlayerInfoUIMap = new Dictionary<Player, UIPlayerInfoElement>();
            m_Players = new List<Player>();
        }
    }
}