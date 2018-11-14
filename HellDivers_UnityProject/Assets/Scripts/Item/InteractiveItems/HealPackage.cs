using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPackage : InteractiveItem
{
    public float Heal { get { return m_Heal; } }
    [SerializeField] private float m_Heal;

    public override void OnInteract(Player player)
    {
        bool bHeal = player.TakeHealth(Heal);
        if (bHeal == false) return;

        if (ObjectPool.m_Instance != null)
        {
            int type = int.Parse(ID);
            ObjectPool.m_Instance.UnLoadObjectToPool(type, this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}