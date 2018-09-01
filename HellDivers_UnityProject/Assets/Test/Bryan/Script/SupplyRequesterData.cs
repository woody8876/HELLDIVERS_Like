using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SupplyRequesterData", menuName = "HELLDIVERS/Supply Requester Data", order = 101)]
public class SupplyRequesterData : ScriptableObject
{
    [SerializeField] private GameObject m_Item;
    public GameObject Item { get { return m_Item; } }

    [SerializeField] private float m_CoolDown;
    public float CoolDown { get { return m_CoolDown; } }

    [SerializeField] private float m_AcivateTime;
    public float AcivateTime { get { return m_AcivateTime; } }

    [SerializeField] private ERequestCode[] m_code;
    public ERequestCode[] RequestCode { get { return m_code; } }
}

public enum ERequestCode
{
    Up,
    Down,
    Left,
    Right
}