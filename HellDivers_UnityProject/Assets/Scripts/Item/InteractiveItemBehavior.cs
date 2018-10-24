using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBehavior
{
    private List<IInteractable> m_Items;

    public void Subscribe(IInteractable item)
    {
        m_Items.Add(item);
    }

    public void Unsubscribe(IInteractable item)
    {
        m_Items.Remove(item);
    }

    public void OnInteract(Player player)
    {
        IInteractable nearest = null;
        float nearestDistance = float.MaxValue;
        foreach (IInteractable item in m_Items)
        {
            float distance = Vector3.Distance(player.transform.position, item.Position);
            if (distance < nearestDistance)
            {
                nearest = item;
            }
        }

        if (nearest != null) nearest.OnInteract(player);
    }
}

public interface IInteractable
{
    Vector3 Position { get; }

    void OnInteract(Player player);
}