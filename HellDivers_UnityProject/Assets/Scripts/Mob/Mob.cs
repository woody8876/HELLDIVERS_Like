using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Character
{

    public override bool TakeDamage(float dmg, Vector3 hitPoint)
    {
        return base.TakeDamage(dmg, hitPoint);
    }

    public override void Death()
    {
        base.Death();
    }
}
