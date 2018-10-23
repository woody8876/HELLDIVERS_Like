using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMob : Character
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IDamageable target = other.transform.GetComponentInParent<IDamageable>();
            target.TakeDamage(10, other.ClosestPoint(this.transform.position));
        }
    }
}