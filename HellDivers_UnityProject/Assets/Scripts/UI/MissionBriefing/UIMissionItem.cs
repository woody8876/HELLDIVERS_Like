using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionItem : MonoBehaviour {

    private string m_Introduction;

    public void Init(eMissionType type)
    {
        switch (type)
        {
            case eMissionType.Tower:
                m_Introduction = "Tower Mission";
                break;
            case eMissionType.KillMob:
                m_Introduction = "Kill Mission";
                break;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
