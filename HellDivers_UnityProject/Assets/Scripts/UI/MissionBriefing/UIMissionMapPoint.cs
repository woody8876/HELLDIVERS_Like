using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionMapPoint : MonoBehaviour {

    [SerializeField] private Image m_Icon;
    [SerializeField] private Color m_NormalColor;
    [SerializeField] private Color m_HighlightColor;

    public GameObject CurrentTarget { get { return m_CurrentTarget; } set { m_CurrentTarget = value; } }

    private GameObject m_CurrentTarget;

    public void Init(GameObject target)
    {
        m_CurrentTarget = target;
    }

    private void Awake()
    {
        m_Icon.color = m_NormalColor;
    }

    public void Highlight()
    {
        m_Icon.color = m_HighlightColor;
    }

    public void Normal()
    {
        m_Icon.color = m_NormalColor;
    }
}
