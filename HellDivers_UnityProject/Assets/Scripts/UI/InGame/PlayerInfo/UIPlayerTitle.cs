using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerTitle : MonoBehaviour
{
    private Player m_Player;
    [SerializeField] private Text m_PlayerName;
    [SerializeField] private Text m_PlayerRank;
    [SerializeField] private Image m_PlayerRankImg;
    [SerializeField] private Image m_PlayerDeathImg;

    public void Initialize(Player player)
    {
        m_Player = player;

        m_PlayerName.text = m_Player.Info.Username;
        m_PlayerRank.text = m_Player.Info.Rank.ToString();

        string rankImgFile = "icon_rank_";
        if (m_Player.Info.Rank < 1)
        {
            rankImgFile += "01";
        }
        else if (m_Player.Info.Rank > 40)
        {
            rankImgFile += "40";
        }
        else
        {
            rankImgFile += m_Player.Info.Rank.ToString("00");
        }

        m_PlayerRankImg.sprite = UIHelper.LoadSprite(UIHelper.RankIconFolder, rankImgFile);

        m_Player.OnStartSpawnNotify += DrawAliveTitle;
        m_Player.OnStartDeathNotify += DrawDathTitle;
    }

    private void OnDestroy()
    {
        m_Player.OnStartSpawnNotify -= DrawAliveTitle;
        m_Player.OnStartDeathNotify -= DrawDathTitle;
    }

    private void DrawAliveTitle()
    {
        m_PlayerDeathImg.gameObject.SetActive(false);
        m_PlayerRankImg.gameObject.SetActive(true);
        m_PlayerRank.gameObject.SetActive(true);
    }

    private void DrawDathTitle()
    {
        m_PlayerDeathImg.gameObject.SetActive(true);
        m_PlayerRankImg.gameObject.SetActive(false);
        m_PlayerRank.gameObject.SetActive(false);
    }
}