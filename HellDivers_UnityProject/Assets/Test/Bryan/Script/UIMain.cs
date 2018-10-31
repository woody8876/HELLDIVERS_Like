using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    public static UIMain Instance { get; private set; }
    [SerializeField] private Transform m_PlayerInfoPanel;
    [SerializeField] private GameObject m_PlayerInfoPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void AddPlayerInfo(Player player)
    {
        GameObject newPlayerinfo = Instantiate(m_PlayerInfoPrefab, m_PlayerInfoPanel);
        UIPlayerInfo uiPlayerInfo = newPlayerinfo.GetComponent<UIPlayerInfo>();
        uiPlayerInfo.Initialize(player);
    }
}