using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputSetting")]
public class ControllerSetting : ScriptableObject
{
    public string Horizontal = "Horizontal";
    public string Vertical = "Vertical";
    public string DirectionHorizontal = "DirectionHorizontal";
    public string DirectionVertical = "DirectionVertical";
    public string StratagemHorizontal = "StratagemHorizontal";
    public string StratagemVertical = "StratagemVertical";
    public string Fire = "Fire";
    public string Grenade = "Grenade";
    public KeyCode MeleeAttack = KeyCode.Joystick1Button0;
    public KeyCode Interactive = KeyCode.Joystick1Button1;
    public KeyCode Roll = KeyCode.Joystick1Button2;
    public KeyCode WeaponSwitch = KeyCode.Joystick1Button3;
    public KeyCode Stratagem = KeyCode.Joystick1Button4;
    public KeyCode Reload = KeyCode.Joystick1Button5;
    public KeyCode Run = KeyCode.Joystick1Button8;
}
