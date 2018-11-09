using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StratagemInfo
{
    #region Properties

    public int ID { get { return id; } }
    public int Rank { get { return rank; } }
    public string Title { get { return title; } }
    public eType Type { get { return type; } }
    public eCode[] Codes { get { return codes; } }
    public int Uses { get { return uses; } }
    public float CoolDown { get { return cooldown; } }
    public float Activation { get { return activation; } }
    public string DisplayID { get { return displayId; } }
    public string ResultID { get { return resultId; } }

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