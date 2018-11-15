using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRecord
{
    public int TimesOfDeath
    {
        get { return timesOfDeath; }
        set { if (value > timesOfDeath) timesOfDeath = value; }
    }

    public int NumOfKills
    {
        get { return numOfKills; }
        set { if (value > numOfKills) numOfKills = value; }
    }

    public int Money { get { return money; } set { money = value; } }

    public int NumOfMission
    {
        get { return numOfMission; }
        set { if (value > numOfMission) numOfMission = value; }
    }

    public int Exp
    {
        get { return exp; }
        set { if (value > exp) exp = value; }
    }

    private int timesOfDeath;
    private int numOfKills;
    private int money;
    private int numOfMission;
    private int exp;

    public void Reset()
    {
        timesOfDeath = 0;
        numOfKills = 0;
        numOfMission = 0;
    }
}