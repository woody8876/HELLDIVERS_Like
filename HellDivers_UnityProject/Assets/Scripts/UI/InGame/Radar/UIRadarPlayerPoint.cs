using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRadarPlayerPoint : MonoBehaviour {


    private Player m_CurrentPlayer;

    private void Init(Player player)
    {
        m_CurrentPlayer = player;

        m_CurrentPlayer.OnSpawnBegin += ShowPoint;
        m_CurrentPlayer.OnDeathBegin += HidePoint;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        m_CurrentPlayer.OnSpawnBegin -= ShowPoint;
        m_CurrentPlayer.OnDeathBegin -= HidePoint;
    }

    private void ShowPoint()
    {
        this.gameObject.SetActive(true);
    }

    private void HidePoint()
    {
        this.gameObject.SetActive(false);
    }
}
