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
        }

        public void AddPlayer(Player player)
        {
            m_PanelPlayerInfo.AddPlayer(player);
        }
    }
}