using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIGrenadeInfo : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }

        [SerializeField] private Image m_Icon;
        [SerializeField] private Color m_BrightColor;
        [SerializeField] private Color m_DarkColor;
        private List<Image> m_Ammos = new List<Image>();

        public void Init(Player player, int grenadeID)
        {
            CurrentPlayer = player;

            string fileName = string.Format("icon_{0}", grenadeID);
            m_Icon.sprite = UIHelper.LoadSprite(UIHelper.GrenadeIconFolder, fileName);
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