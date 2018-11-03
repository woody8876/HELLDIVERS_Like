using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGrenades
{
    NORMAL,
    LIGHTING,
    PUMPKIN,
    MINE,
    TIMEBOMB
}

public class GrenadeInfo {

    public int ID { private set ; get ; }
    public int Type { private set; get; }
    public string Title { private set; get; }
    public float Damage { private set; get; }
    public float Timer { private set; get; }
    public float Range { private set; get; }
    public int MaxCount { private set; get; }

    public void SetID(int id) { ID = id; }
    public void SetType(int type) { Type = type; }
    public void SetTitle(string title) { Title = title; }
    public void SetDamage(float damage) { Damage = damage; }
    public void SetTimer(float timer) { Timer = timer; }
    public void SetRange(float range) { Range = range; }
    public void SetMaxCount(int count) { MaxCount = count; }



}

