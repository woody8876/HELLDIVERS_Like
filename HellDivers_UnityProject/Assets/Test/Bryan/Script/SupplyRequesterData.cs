using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SupplyRequesterData", menuName = "HELLDIVERS/Supply Requester Data", order = 101)]
public class SupplyRequesterData : ScriptableObject
{
    public int ID { get { return m_Id; } }
    public GameObject Item { get { return m_Item; } }
    public float CoolDown { get { return m_CoolDown; } }
    public float AcivateTime { get { return m_AcivateTime; } }
    public ERequestCode[] RequestCode { get { return m_code; } }

    [SerializeField] private int m_Id;
    [SerializeField] private GameObject m_Item;
    [SerializeField] private float m_CoolDown;
    [SerializeField] private float m_AcivateTime;
    [SerializeField] private ERequestCode[] m_code;

    public void CopyTo(ref SupplyRequesterData target)
    {
        target = this;
    }
}

public enum ERequestCode
{
    Up,
    Down,
    Left,
    Right
}