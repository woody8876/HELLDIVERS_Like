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
        [SerializeField] private UIPanelStratagemAct m_PanelStratagemAct;
        [SerializeField] private UIDynamicHpBar m_DynamicHpBar;
        [SerializeField] private UIDynamicMissionMsg m_DynamicMissionMsg;
        [SerializeField] private UIPanelMissionFailed m_PanelMissionFaild;
        [SerializeField] private UIPanelRadar m_PanelRadar;
        private Dictionary<Player, UIDynamicHpBar> m_DynamicHpBarMap;

        public void Init()
        {
            m_PanelPlayerInfo = Instantiate(m_PanelPlayerInfo, this.transform).GetComponent<UIPanelPlayerInfo>();
            m_PanelStratagemAct = Instantiate(m_PanelStratagemAct, this.transform).GetComponent<UIPanelStratagemAct>();
            m_PanelMissionFaild = Instantiate(m_PanelMissionFaild, this.transform).GetComponent<UIPanelMissionFailed>();
            m_PanelRadar = Instantiate(m_PanelRadar, this.transform).GetComponent<UIPanelRadar>();
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

        public void DrawGameUI()
        {
            m_PanelPlayerInfo.gameObject.SetActive(true);
            m_PanelStratagemAct.gameObject.SetActive(true);
            foreach (KeyValuePair<Player, UIDynamicHpBar> playerHpBar in m_DynamicHpBarMap)
            {
                playerHpBar.Value.gameObject.SetActive(true);
            }
        }

        public void AddPlayer(Player player)
        {
            m_PanelPlayerInfo.AddPlayer(player);
            m_PanelStratagemAct.AddPlayer(player);
            AddDynamicHpBar(player);
        }

        public void AddRadarPoint(GameObject target, eMapPointType type)
        {
            m_PanelRadar.AddPointPrefab(target, type);
        }

        public void AddDynamicHpBar(Player player)
        {
            if (m_DynamicHpBarMap.ContainsKey(player)) return;

            UIDynamicHpBar hpBar = Instantiate(m_DynamicHpBar, this.transform).GetComponent<UIDynamicHpBar>();
            hpBar.Init(player);
            m_DynamicHpBarMap.Add(player, hpBar);
            hpBar.gameObject.SetActive(false);
        }

        public void AddDynamicMissionMsg(MissionTower mission)
        {
            UIDynamicMissionMsg missionMsg = Instantiate(m_DynamicMissionMsg, this.transform).GetComponent<UIDynamicMissionMsg>();
            missionMsg.Init(mission);
        }

        public void DrawMissionFailedUI()
        {
            m_PanelMissionFaild.StartUI();
        }
    }
}