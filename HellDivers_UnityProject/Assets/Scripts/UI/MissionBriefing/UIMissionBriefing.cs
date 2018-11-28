using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionBriefing : MonoBehaviour
{

    public static UIMissionBriefing Instance { get; private set; }

    [SerializeField] private UIMissionBriefingTitle m_Title;
    [SerializeField] private UIMissionBriefingBackground m_Background;
    [SerializeField] private UIMissionBriefingButton m_Button;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
