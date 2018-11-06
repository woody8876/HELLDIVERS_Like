using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelPlayerInfo : MonoBehaviour
{
    public static UIPanelPlayerInfo Instance { get { return m_Instance; } }
    private static UIPanelPlayerInfo m_Instance;

    private List<Player> m_Players;
    [SerializeField] private UIPlayerInfoElement m_UIPlayerInfoElement;

    public void Init(Player player)
    {
        m_UIPlayerInfoElement = Instantiate(m_UIPlayerInfoElement, this.transform).GetComponent<UIPlayerInfoElement>();
        m_UIPlayerInfoElement.Init(player);
    }

    private void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        else Destroy(this.gameObject);

        m_Players = new List<Player>();
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