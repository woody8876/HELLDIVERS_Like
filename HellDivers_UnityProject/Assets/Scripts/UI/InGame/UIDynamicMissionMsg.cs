using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIDynamicMissionMsg : MonoBehaviour
    {
        public MissionTower CurrentTower { get; private set; }
        public string Message { get { return m_textMessage.text; } set { m_textMessage.text = value; } }

        [SerializeField] private Text m_textMessage;
        [SerializeField] private Text m_textTime;
        [SerializeField] private Transform m_arrowRoot;
        [SerializeField] private Image m_imgArrow;

        public void Init(MissionTower tower)
        {
            CurrentTower = tower;
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