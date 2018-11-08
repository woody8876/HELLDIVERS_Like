using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIStratagemActElement : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }
        public Stratagem CurrentStratagem { get; private set; }

        [SerializeField] private Image m_Icon;
        [SerializeField] private Text m_Time;

        public void Init(Stratagem stratagem)
        {
            CurrentStratagem = stratagem;

            string fileName = string.Format("icon_{0}", stratagem.Info.ID);
            m_Icon.sprite = UIHelper.LoadSprite(UIHelper.StratagemIconFolder, fileName);
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