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

            for (int i = 0; i < CurrentTower.Codes.Length; i++)
            {
                CreateArrow(CurrentTower.Codes[i]);
            }
            m_arrowRoot.gameObject.SetActive(false);
        }

        private void CreateArrow(eCode code)
        {
            Image codeImg = Instantiate(m_imgArrow, m_arrowRoot);
            switch (code)
            {
                case eCode.Up:
                    codeImg.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;

                case eCode.Down:
                    codeImg.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;

                case eCode.Left:
                    codeImg.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;

                case eCode.Right:
                    codeImg.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }
        }

        private void SubscribeEvent()
        {
            //CurrentTower.CodeMechine.OnStart
        }

        private void RefreshTimer()
        {
            string minutes = Mathf.Floor(CurrentTower.ActTimeCountDown / 60).ToString("00");
            string seconds = (CurrentTower.ActTimeCountDown % 60).ToString("00");
            m_textTime.text = string.Format("{0}:{1}", minutes, seconds);
        }
    }
}