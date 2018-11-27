using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HELLDIVERS.UI;

public class UIMissionInfo : MonoBehaviour
{
    [SerializeField] private Image m_CheckMark;
    [SerializeField] private Image m_Icon;
    [SerializeField] private Text m_Description;
    [SerializeField] private Text m_Count;

    public void Initialize(MissionManager manager, eMissionType type)
    {
        SetIcon(type);
    }

    private void SetIcon(eMissionType type)
    {
        switch (type)
        {
            case eMissionType.Tower:
                Sprite icon = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.MissionIconFolder, "MissionTower");
                m_Icon.sprite = icon;
                break;
        }
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