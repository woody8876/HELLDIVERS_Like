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

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);
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