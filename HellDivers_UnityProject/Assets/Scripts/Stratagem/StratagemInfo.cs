using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StratagemInfo
{
    #region Properties

    public int ID { get { return id; } set { id = value; } }
    public int Rank { get { return rank; } set { rank = (value < 0) ? 0 : value; } }
    public string Title { get { return title; } set { title = value; } }
    public eType Type { get { return type; } set { type = value; } }
    public eCode[] Codes { get { return codes; } set { codes = value; } }
    public int Uses { get { return uses; } set { uses = (value < -1) ? -1 : value; } }
    public float CoolDown { get { return cooldown; } set { cooldown = value; } }
    public float Activaion { get { return activation; } set { activation = value; } }
    public string DisplayID { get { return displayId; } set { displayId = value; } }

    #endregion Properties

    [SerializeField] private int id;
    [SerializeField] private int rank;
    [SerializeField] private string title;
    [SerializeField] private eType type;
    [SerializeField] private eCode[] codes;
    [SerializeField] private int uses;
    [SerializeField] private float cooldown;
    [SerializeField] private float activation;
    [SerializeField] private string displayId;
    [SerializeField] private string resultId;

    public enum eType
    {
        Supply, Defensive, Offensive, Special
    }

    public enum eCode
    {
        Up, Down, Left, Right
    }

    public StratagemInfo Clone()
    {
        StratagemInfo clone = new StratagemInfo();
        CopyTo(clone);
        return clone;
    }

    public void CopyTo(StratagemInfo info)
    {
        info.id = this.id;
        info.rank = this.rank;
        info.title = this.title;
        info.type = this.type;
        info.codes = this.codes;
        info.uses = this.uses;
        info.cooldown = this.cooldown;
        info.activation = this.activation;
        info.displayId = this.displayId;
        info.resultId = this.resultId;
    }
}