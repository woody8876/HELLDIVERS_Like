using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItem : MonoBehaviour, IInteractable
{
    public Vector3 Position { get { return this.transform.position; } }

    private string m_Id;
    private string m_Name;
    private float m_LifeTime = 120.0f;
    private float m_EndTime;

    protected virtual void OnEnable()
    {
        m_EndTime = Time.time + m_LifeTime;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        Player[] players = GameMain.Instance.Players.ToArray();
        if (players != null)
        {
            foreach (Player p in players)
            {
                p.Interactive.Subscribe(this);
            }
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (m_LifeTime != -1)
        {
            if (Time.time >= m_EndTime) this.gameObject.SetActive(false);
        }
    }

    public virtual void OnInteract(Player player)
    {
        Destroy(this.gameObject);
    }
}