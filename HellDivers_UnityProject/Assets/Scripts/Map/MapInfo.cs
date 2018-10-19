using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public static MapInfo Instance;

    public List<Transform> SpawnPos { get { return m_SpawnPos; } }
    [SerializeField] private List<Transform> m_SpawnPos;

    public Transform GetRandomSpawnPos()
    {
        if (m_SpawnPos != null || m_SpawnPos.Count > 0)
        {
            int rand = Random.Range(0, m_SpawnPos.Count - 1);
            return m_SpawnPos[rand];
        }
        return null;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}