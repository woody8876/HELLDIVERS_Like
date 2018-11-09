using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInfo {
    #region Properties
    public int ID { private set; get; }
    public string Horizontal { private set; get; }
    public string Vertical { private set; get; }
    public string DirectionHorizontal { private set; get; }
    public string DirectionVertical { private set; get; }
    public string StratagemHorizontal { private set; get; }
    public string StratagemVertical { private set; get; }
    public string Fire { private set; get; }
    public string Stratagem { private set; get; }
    public string Run { private set; get; }
    public string WeaponSwitch { private set; get; }
    public string Reload { private set; get; }
    public string MeleeAttack { private set; get; }
    public string Interactive { private set; get; }
    public string Roll { private set; get; }
    public string Grenade { private set; get; }
    #endregion
    #region Set Properties
    public void SetID(int id) { ID = id; }
    public void SetHorizontal(string horizontal) { Horizontal = horizontal; }
    public void SetVertical(string vertical) { Vertical = vertical; }
    public void SetDirectionHorizontal(string directionHorizontal) { DirectionHorizontal = directionHorizontal; }
    public void SetDirectionVertical(string directionVertical) { DirectionVertical = directionVertical; }
    public void SetStratagemHorizontal(string stratagemHorizontal) { StratagemHorizontal = stratagemHorizontal; }
    public void SetStratagemVertical(string stratagemVertical) { StratagemVertical = stratagemVertical; }
    public void SetFire(string fire) { Fire = fire; }
    public void SetStratagem(string stratagem) { Stratagem = stratagem; }
    public void SetRun(string run) { Run = run; }
    public void SetWeaponSwitch(string weaponSwitch) { WeaponSwitch = weaponSwitch; }
    public void SetReload(string reload) { Reload = reload; }
    public void SetMeleeAttack(string meleeAttack) { MeleeAttack = meleeAttack; }
    public void SetInteractive(string interactive) { Interactive = interactive; }
    public void SetRoll(string roll) { Roll = roll; }
    public void SetGrenade(string grenade) { Grenade = grenade; }
    #endregion
}
