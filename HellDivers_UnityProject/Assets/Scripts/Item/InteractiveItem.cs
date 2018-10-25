using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItem : MonoBehaviour
{
    public string ID { get { return m_Id; } }
    public string Title { get { return m_Title; } }
    public float Radius { get { return m_Radius; } }
    public Vector3 Position { get { return this.transform.position; } }

    [SerializeField] private string m_Id;
    [SerializeField] private string m_Title;
    [SerializeField] private float m_Radius = 2;
    private float m_LifeTime = 120.0f;
    private float m_EndTime;

    protected virtual void OnEnable()
    {
        InteractiveItemManager.Instance.AddItem(this);
        m_EndTime = Time.time + m_LifeTime;
    }

    // Use this for initialization
    protected virtual void Start()
    {
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

    protected virtual void OnDisable()
    {
        InteractiveItemManager.Instance.RemoveItem(this);
    }
}