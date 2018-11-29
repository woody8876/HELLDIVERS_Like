using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HELLDIVERS.UI;

[RequireComponent(typeof(UITweenCanvasGroup))]
public class UIMissionReward : MonoBehaviour
{
    [SerializeField] private Image m_Icon;
    [SerializeField] private Image m_CkeckMark;
    [SerializeField] private Text m_Description;
    private UITweenCanvasGroup m_CanvasTweener;

    public void Initialize(eMissionType type)
    {
        SetupDisplay(type);
        m_CanvasTweener.Play();
    }

    private void SetupDisplay(eMissionType type)
    {
        switch (type)
        {
            case eMissionType.Tower:
                m_Icon.sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.MissionIconFolder, "icon_Mission_01");
                m_Description.text = "Activate Truth Transmitter";
                break;

            case eMissionType.KillMob:
                m_Icon.sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.MissionIconFolder, "icon_Mission_02");
                m_Description.text = "Exterminate Enemys";
                break;
        }
    }

    private void Awake()
    {
        m_CanvasTweener = this.GetComponent<UITweenCanvasGroup>();
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