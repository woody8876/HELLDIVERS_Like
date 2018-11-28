using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIMissionInfo : MonoBehaviour
    {
        [SerializeField] protected Image m_CheckMark;
        [SerializeField] protected Image m_Icon;
        [SerializeField] protected Text m_Description;
        [SerializeField] protected Text m_Count;
        protected eMissionType m_Type;

        public void Initialize(Mission mission)
        {
            m_Type = mission.Type;
            SetupDisplay(mission.Type);
            OnInitialized(mission);
        }

        protected virtual void OnInitialized(Mission mission)
        {
        }

        protected virtual void SetupDisplay(eMissionType type)
        {
            switch (type)
            {
                case eMissionType.Tower:
                    m_Icon.sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.MissionIconFolder, "MissionTower");
                    m_Description.text = "Activate Truth Transmitter";
                    break;

                case eMissionType.KillMob:
                    m_Icon.sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.MissionIconFolder, "MissionAssassinate");
                    m_Description.text = "MissionAssassinate";
                    break;
            }
        }
    }
}