using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StratagemInfo
{
    public int id;
    public int rank;
    public string title;
    public StratagemType type;
    public StratagemCode[] code;
    public int uses;
    public float cooldown;
    public float activation;
}

public enum StratagemType
{
    Undefine = -1, Supply, Defensive, Offensive, Special
}

public enum StratagemCode
{
    Up, Down, Left, Right
}