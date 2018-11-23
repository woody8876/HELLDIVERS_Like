using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public bool IsFinished { get { return m_bFinished; } }

    protected bool m_bFinished;

    public delegate void MissionEventHolder(Mission mission);

    public MissionEventHolder OnMissionStart;
    public MissionEventHolder OnMissionComplete;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}