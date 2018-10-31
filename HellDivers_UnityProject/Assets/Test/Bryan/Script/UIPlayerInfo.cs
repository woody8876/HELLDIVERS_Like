using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    public Player CurrentPlayer { get { return m_Player; } }
    private Player m_Player;
    [SerializeField] private GameObject m_Title;
    private Text m_PlayerName;
    private Text m_PlayerRank;

    public void Initialize(Player player)
    {
        m_Player = player;
    }

    // Use this for initialization
    private void Start()
    {
        if (m_Title != null)
        {
            m_PlayerName = m_Title.transform.Find("Text_Name").GetComponent<Text>();
            m_PlayerRank = m_Title.transform.Find("Text_Rank").GetComponent<Text>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void UpdateTitleDisplay(Player player)
    {
    }
}