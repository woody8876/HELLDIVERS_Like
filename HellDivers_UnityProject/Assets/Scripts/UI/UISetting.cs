using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UISetting", menuName = "UI Setting", order = 100)]
public class UISetting : ScriptableObject
{
    public string RankIconFolder = "UI/Resource/Icons/Rank";
    public string StratagemIconFolder = "UI/Resource/Icons/Stratagem";
    public string WeaponIconFolder = "UI/Resource/Icons/Weapon";
    public string GrenadeIconFolder = "UI/Resource/Icons/Grenade";
    public Color Player1Color;
    public Color Player2Color;
}