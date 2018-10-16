using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Camera))]
public class CameraFollowing : MonoBehaviour
{
    #region Define Inputs

    private string m_InputAim = "Fire1";
    private string m_InputHorizontal = "Horizontal";
    private string m_InputVertical = "Vertical";
    private string m_InputRun = "Run";

    #endregion Define Inputs

    #region Porpeties

    public LinkedList<Transform> Targets { get { return m_Targets; } }

    public float Speed
    {
        get { return m_CamLerp; }
        set
        {
            if (value > 1) m_CamLerp = 1.0f;
            else if (value < 0) m_CamLerp = 0.0f;
            m_CamLerp = value;
        }
    }

    #endregion Porpeties

    #region Private Variable

    private LinkedList<Transform> m_Targets = new LinkedList<Transform>();
    private Camera m_Cam;
    [SerializeField] private float m_CamHeight = 10.0f;
    [SerializeField] private float m_CamRotX = 60.0f;
    [SerializeField] private float m_CamLerp = 0.1f;
    [SerializeField] private float m_CamWalkAdd = 3.0f;
    [SerializeField] private float m_CamRunAdd = 8.0f;
    private Vector3 m_Destination;
    private Vector2 m_ExtraVec;

    #endregion Private Variable

    #region Public Function

    /// <summary>
    /// Set focus on the target. And clear all processing targets.
    /// </summary>
    public void FocusOnTarget(Transform t)
    {
        m_Targets.Clear();
        m_Targets.AddFirst(t);
    }

    /// <summary>
    /// Add a target.
    /// </summary>
    public void AddTarget(Transform t)
    {
        m_Targets.AddLast(t);
    }

    /// <summary>
    /// Remove a target.
    /// </summary>
    public bool RemoveTarget(Transform t)
    {
        if (m_Targets.Remove(t)) return true;
        else return false;
    }

    #endregion Public Function

    #region MonoBehaviour

    // Use this for initialization
    private void Start()
    {
        m_Cam = this.GetComponent<Camera>();
        m_Cam.transform.rotation = Quaternion.Euler(m_CamRotX, 0, 0);
        m_Cam.fieldOfView = 60.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_Targets.Count <= 0) return;

        UpdateDestination();

        if (m_Targets.Count == 1)
        {
            if (Input.GetButton(m_InputAim))
            {
                m_ExtraVec.x = m_Targets.First.Value.forward.x;
                m_ExtraVec.y = m_Targets.First.Value.forward.z * 0.75f;
            }
            else
            {
                float h = Input.GetAxis(m_InputHorizontal);
                float v = Input.GetAxis(m_InputVertical);

                bool bRun = Input.GetButton(m_InputRun);
                m_ExtraVec.x = (bRun) ? h * m_CamRunAdd : h * m_CamWalkAdd;
                m_ExtraVec.y = (bRun) ? v * m_CamRunAdd : v * m_CamWalkAdd;

                if (m_ExtraVec.y <= 0) m_ExtraVec.y *= 1.25f;
            }
            AddOnDestination(m_ExtraVec.x, m_ExtraVec.y);
        }

        MoveToDestination();
    }

    #endregion MonoBehaviour

    #region Private Function

    private void UpdateDestination()
    {
        if (m_Targets.Count > 1)
        {
            m_Destination.Set(0, 0, 0);
            foreach (Transform target in m_Targets)
            {
                m_Destination += target.position;
            }
            m_Destination /= m_Targets.Count;
        }
        else
        {
            m_Destination = m_Targets.First.Value.position;
        }

        m_Destination.y += m_CamHeight;
        m_Destination.z += -Mathf.Tan((90 - m_CamRotX) * Mathf.Deg2Rad) * (m_CamHeight - 1f);
    }

    private void AddOnDestination(float x, float z)
    {
        m_Destination.x += x;
        m_Destination.z += z;
    }

    private void MoveToDestination()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, m_Destination, m_CamLerp);
    }

    #endregion Private Function

#if UNITY_EDITOR

    #region Debug Draw

    public bool m_ShowDebugDraw;

    private void OnDrawGizmos()
    {
        if (m_ShowDebugDraw == false) return;
        if (m_Targets.First == null) return;

        Vector3 pos = m_Targets.First.Value.position;
        pos.y += 1.0f;
        targetOnScreen = m_Cam.WorldToScreenPoint(pos);

        screenBottom.Set(targetOnScreen.x, 0, targetOnScreen.z);
        DrawTestLine(screenBottom, pos);

        screenTop.Set(targetOnScreen.x, m_Cam.pixelHeight, targetOnScreen.z);
        DrawTestLine(screenTop, pos);

        screenLeft.Set(0.0f, targetOnScreen.y, targetOnScreen.z);
        DrawTestLine(screenLeft, pos);

        screenRight.Set(m_Cam.pixelWidth, targetOnScreen.y, targetOnScreen.z);
        DrawTestLine(screenRight, pos);
    }

    private void DrawTestLine(Vector3 border, Vector3 agent)
    {
        border = m_Cam.ScreenToWorldPoint(border);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(border, agent);

        float dist = Vector3.Distance(border, agent);
        Vector3 center = (border + agent) * 0.5f;
        Handles.Label(center, dist.ToString("0.00"));
    }

    private Vector3 targetOnScreen;
    private Vector3 screenBottom;
    private Vector3 screenTop;
    private Vector3 screenLeft;
    private Vector3 screenRight;

    #endregion Debug Draw

#endif
}