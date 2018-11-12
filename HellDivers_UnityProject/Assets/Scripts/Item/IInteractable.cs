using UnityEngine;

public interface IInteractable
{
    string ID { get; }
    float Radius { get; }
    Vector3 Position { get; }

    void OnInteract(Player player);
}