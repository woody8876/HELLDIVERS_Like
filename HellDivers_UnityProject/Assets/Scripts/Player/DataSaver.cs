using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData")]
public class DataSaver : ScriptableObject {

    public string Username;
    public int Rank;
    public int Exp;
    public int Money;
    public List<int> Weapons;
    public List<int> Stratagems;
    public List<int> Grenades;
    public List<int> UnlockedWeapons;
    public List<int> UnlockedStratagems;



}
