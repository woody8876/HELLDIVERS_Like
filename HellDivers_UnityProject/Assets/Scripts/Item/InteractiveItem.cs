using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakable
{
    void OnTake(Player player);
}

[RequireComponent(typeof(Animator))]
public class InteractiveItem : MonoBehaviour, ITakable
{
    private string m_Id;
    private string m_Name;
    [SerializeField] private float m_HealPosint;
    private float m_LifeTime = 120.0f;
    private float m_EndTime;
    private Animator m_Animator;

    private void OnEnable()
    {
        m_EndTime = Time.time + m_LifeTime;
    }

    // Use this for initialization
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.time >= m_EndTime) this.gameObject.SetActive(false);
    }

    public void OnTake(Player taker)
    {
        bool bHeal = taker.TakeHealth(m_HealPosint);
        if (bHeal) Destroy(this.gameObject);
    }
}