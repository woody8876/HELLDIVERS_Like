using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerCreater
{
    public static GameObject CreatMainPlayer(PlayerInfo data)
    {
        if (data == null) return null;

        GameObject playerGo = Resources.Load("Characters/Ch00/ch00") as GameObject;
        playerGo = GameObject.Instantiate(playerGo);
        Player p = playerGo.AddComponent<Player>();
        p.Initialize(data);

        return playerGo;
    }
}