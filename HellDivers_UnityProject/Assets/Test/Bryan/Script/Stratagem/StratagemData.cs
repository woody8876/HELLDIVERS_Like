using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemData
{
    public int ID { get; set; }
    public int Rank { get; set; }
    public string Title { get; set; }
    public StratagemType Type { get; set; }
    public StratagemCode[] Code { get; set; }
    public int Uses { get; set; }
    public float Cooldown { get; set; }
    public float Activation { get; set; }
}

public enum StratagemType
{
    Undefine = -1, Supply, Defensive, Offensive, Special
}

public enum StratagemCode
{
    Up, Down, Left, Right
}