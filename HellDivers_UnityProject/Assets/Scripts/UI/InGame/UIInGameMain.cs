using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIInGameMain : MonoBehaviour
    {
        public static UIInGameMain Instance { get; private set; }

        [SerializeField] private UIPanelPlayerInfo m_PanelPlayerInfo;
        [SerializeField] private UIDynamicHpBar m_DynamicHpBar;
        private Dictionary<Player, UIDynamicHpBar> m_DynamicHpBarMap;

        public void Init()
        {
            m_PanelPlayerInfo = Instantiate(m_PanelPlayerInfo, this.transform).GetComponent<UIPanelPlayerInfo>();
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Destroy(this.gameObject);
            }

            m_DynamicHpBarMap = new Dictionary<Player, UIDynamicHpBar>();
        }

        public void AddPlayer(Player player)
        {
            m_PanelPlayerInfo.AddPlayer(player);
            AddDynamicHpBar(player);
        }

        public void AddDynamicHpBar(Player player)
        {
            if (m_DynamicHpBarMap.ContainsKey(player)) return;

            UIDynamicHpBar hpBar = Instantiate(m_DynamicHpBar, this.transform).GetComponent<UIDynamicHpBar>();
            hpBar.Init(player);
            m_DynamicHpBarMap.Add(player, hpBar);
        }
    }
}