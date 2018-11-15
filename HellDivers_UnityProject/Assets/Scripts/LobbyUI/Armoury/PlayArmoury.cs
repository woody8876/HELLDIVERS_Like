using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArmoury : MonoBehaviour {

    [SerializeField] GameObject m_PlayerInGame;

    int[] Members
    {
        get
        {
            var key = new int[PlayerManager.Instance.Players.Count];
            PlayerManager.Instance.Players.Keys.CopyTo(key, 0);
            return key;
        }
    }
	// Use this for initialization
	void Start () {
        foreach (var item in Members)
        {
            Vector3 pos = new Vector3(150 + 810 * (item - 1), 780, 0);
            Quaternion rotate = new Quaternion(0, 0, 0, 1);
            GameObject go = Instantiate(m_PlayerInGame, pos, rotate,this.transform);
            go.name = "Player" + item;
            go.GetComponentInChildren<SetPlayerWeapon>().PlayerID = item;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
