using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class MissionTower : MonoBehaviour, IInteractable
{
    public string ID { get { return m_Id; } }
    public float Radius { get { return m_Radius; } }
    public Vector3 Position { get { return this.transform.position; } }

    [SerializeField] private string m_Id;
    [SerializeField] private float m_Radius;
    private Animator m_Animator;

    // Use this for initialization
    private void Start()
    {
        InteractiveItemManager.Instance.AddItem(this);
        m_Animator = this.GetComponentInChildren<Animator>();
    }

    private void OnDisable()
    {
        InteractiveItemManager.Instance.RemoveItem(this);
    }

    public void OnInteract(Player player)
    {
        m_Animator.SetBool("Active", true);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireArc(this.transform.position, Vector3.up, this.transform.forward, 360f, Radius);
    }

#endif
}