using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowing : MonoBehaviour
{
    private LinkedList<Transform> m_Targets;
    private Camera m_Cam;
    private Vector3 m_Destination;
    [SerializeField] private float m_Height = 10.0f;
    [SerializeField] private float m_CamRotX = 60.0f;
    [SerializeField] private float m_CamLerp;
    private Vector3 m_ExtraVec;

    public void FocusOnTheTarget(Transform t)
    {
        m_Targets.Clear();
        m_Targets.AddFirst(t);
    }

    public void AddTarget(Transform t)
    {
        m_Targets.AddLast(t);
    }

    public void RemoveTarget(Transform t)
    {
        m_Targets.Remove(t);
    }

    // Use this for initialization
    private void Start()
    {
        m_Targets = new LinkedList<Transform>();
        m_Cam = this.GetComponent<Camera>();
        m_Cam.transform.rotation = Quaternion.Euler(m_CamRotX, 0, 0);
        m_Cam.fieldOfView = 60.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateTheDestination();
        MoveToTheDestination();
    }

    private void UpdateTheDestination()
    {
        if (m_Targets.Count > 1)
        {
            m_Destination.Set(0, 0, 0);
            foreach (Transform t in m_Targets)
            {
                m_Destination += t.position;
            }
            m_Destination = m_Destination / m_Targets.Count;
        }
        else
        {
            m_Destination = m_Targets.First.Value.position;
        }

        m_Destination.y += m_Height;
        m_Destination.z += -Mathf.Tan((90 - m_CamRotX) * Mathf.Deg2Rad) * (m_Height - 1f);
    }

    private void MoveToTheDestination()
    {
        Vector3 currentPos = this.transform.position;
        currentPos = Vector3.Lerp(currentPos, m_Destination, m_CamLerp);
    }
}