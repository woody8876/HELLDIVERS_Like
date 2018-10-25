using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItemManager
{
    public static InteractiveItemManager Instance { get; private set; }
    private static Dictionary<string, List<InteractiveItem>> m_ItemMap;

    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            m_ItemMap = new Dictionary<string, List<InteractiveItem>>();
        }
    }

    public void AddItem(InteractiveItem item)
    {
        if (m_ItemMap.ContainsKey(item.ID))
        {
            m_ItemMap[item.ID].Add(item);
        }
        else
        {
            List<InteractiveItem> itemList = new List<InteractiveItem>();
            itemList.Add(item);
            m_ItemMap.Add(item.ID, itemList);
        }
    }

    public void RemoveItem(InteractiveItem item)
    {
        if (m_ItemMap.ContainsKey(item.ID))
        {
            m_ItemMap[item.ID].Remove(item);
        }
    }

    public void OnInteractive(Player player)
    {
        InteractiveItem nearestItem = GetNearestItem(player.transform.position);
        if (nearestItem != null) nearestItem.OnInteract(player);
    }

    private InteractiveItem GetNearestItem(Vector3 position)
    {
        if (m_ItemMap.Count <= 0) return null;

        InteractiveItem nearestItem = null;
        float nearestDist = float.MaxValue;
        foreach (KeyValuePair<string, List<InteractiveItem>> itemList in m_ItemMap)
        {
            List<InteractiveItem> currentList = itemList.Value;
            if (currentList.Count <= 0) continue;
            for (int i = 0; i < currentList.Count; i++)
            {
                InteractiveItem currentItem = currentList[i];
                float dist = Vector3.Distance(position, currentItem.Position);
                if (dist > currentItem.Radius) continue;

                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearestItem = currentItem;
                }
            }
        }
        return nearestItem;
    }
}