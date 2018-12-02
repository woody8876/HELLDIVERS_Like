using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    [SerializeField] private Image m_UpperImage;
    [SerializeField] private Image m_Current;
    [SerializeField] private Image m_UnderImage;

    private float vertical;
    // Use this for initialization
    void Start () {
        UIMissionBriefing.Instance.AddMission(eMissionType.Tower);
        UIMissionBriefing.Instance.AddMission(eMissionType.KillMob);
    }
	
	// Update is called once per frame
	void Update () {
        vertical = Input.GetAxis("Vertical");
        if(vertical >= 0)
        {

        }
        else
        {

        }
    }
}
