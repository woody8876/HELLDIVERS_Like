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
    public eType type;
    public eCode[] code;
    public int uses;
    public float cooldown;
    public float activation;
    public string display;
    public string item;

    public enum eType
    {
        Supply, Defensive, Offensive, Special
    }

    public enum eCode
    {
        Up, Down, Left, Right
    }
}