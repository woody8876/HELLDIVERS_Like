using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParts : MonoBehaviour
{
    #region Properties

    public Transform LaunchPoint { get { return m_LaunchPoint; } }
    public Transform RightHand { get { return m_RightHand; } }
    public GameObject Body { get { return m_Body; } }
    public GameObject Gun { get { return m_Gun; } }

    #endregion Properties

    #region Private Variable

    [SerializeField] private Transform m_LaunchPoint;
    [SerializeField] private Transform m_RightHand;
    [SerializeField] private GameObject m_Body;
    [SerializeField] private GameObject m_Gun;

    #endregion Private Variable
}