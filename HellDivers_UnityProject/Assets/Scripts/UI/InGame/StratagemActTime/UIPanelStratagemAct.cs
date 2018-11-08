using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIPanelStratagemAct : MonoBehaviour
    {
        public static UIPanelStratagemAct Instance { get; private set; }

        [SerializeField] private UIStratagemActElement m_StratagemActElement;
        private Dictionary<Player, List<UIStratagemActElement>> m_PlayerMap;

        public void AddPlayer(Player player)
        {
            if (m_PlayerMap.ContainsKey(player)) return;

            List<UIStratagemActElement> pList = new List<UIStratagemActElement>();
            for (int i = 0; i < player.StratagemController.Stratagems.Count; i++)
            {
                UIStratagemActElement actElement = Instantiate(m_StratagemActElement, this.transform).GetComponent<UIStratagemActElement>();
                actElement.Init(player, player.StratagemController.Stratagems[i]);
                actElement.gameObject.SetActive(false);
                pList.Add(actElement);
            }

            m_PlayerMap.Add(player, pList);
        }

        public void RemovePlayer(Player player)
        {
            if (m_PlayerMap.ContainsKey(player) == false) return;

            List<UIStratagemActElement> pList = m_PlayerMap[player];
            foreach (UIStratagemActElement uiElement in pList)
            {
                Destroy(uiElement.gameObject);
            }

            m_PlayerMap.Remove(player);
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);

            GameObject stratagemActElementGo = ResourceManager.m_Instance.LoadData(typeof(GameObject), "UI/InGame/StratagemActTime", "UIStratagemActElement") as GameObject;
            m_PlayerMap = new Dictionary<Player, List<UIStratagemActElement>>();
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}