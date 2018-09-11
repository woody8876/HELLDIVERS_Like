using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SupplyRequesterData", menuName = "HELLDIVERS/Supply Requester Data", order = 101)]
public class SupplyRequesterData : ScriptableObject
{
    public string id;
    public GameObject item;
    public float coolDownTime;
    public float actTime;
    public ERequestCode[] actCode;
}

public enum ERequestCode
{
    Up,
    Down,
    Left,
    Right
}