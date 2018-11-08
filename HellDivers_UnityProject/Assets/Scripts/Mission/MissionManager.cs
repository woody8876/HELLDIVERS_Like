using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager
{
    public MissionManager Instance { get; private set; }

    public MissionManager()
    {
        if (Instance == null) Instance = this;
    }
}