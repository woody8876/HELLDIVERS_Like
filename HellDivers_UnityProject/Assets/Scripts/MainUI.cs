using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public static MainUI Instance { get; private set; }
    public UIPanelLoading LoadingPanel { get { return m_PanelLoading; } }
    [SerializeField] private UIPanelLoading m_PanelLoading;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    private void Start()
    {
        m_PanelLoading = Instantiate(m_PanelLoading, this.transform);
        m_PanelLoading.gameObject.SetActive(false);
    }
}