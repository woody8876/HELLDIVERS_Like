using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDynamicHPBar : MonoBehaviour
{
    public Player CurrentPlayer { get { return m_CurrentPlayer; } }
    private Player m_CurrentPlayer;
    [SerializeField] private Text m_PlayerName;
    [SerializeField] private Image m_Fill;
    [SerializeField] private Vector2 m_Position;

    public void Initialize(Player player)
    {
        m_CurrentPlayer = player;
        m_PlayerName.text = player.Info.Username;
    }

    public void UpdateHPAmout()
    {
        this.gameObject.SetActive(!m_CurrentPlayer.IsDead);

        Vector3 pos = Camera.main.WorldToScreenPoint(m_CurrentPlayer.transform.position);
        pos.x += m_Position.x;
        pos.y += m_Position.y;
        this.transform.position = pos;
        m_Fill.fillAmount = m_CurrentPlayer.CurrentHpPercent;
    }

    private void Update()
    {
        if (m_CurrentPlayer != null) UpdateHPAmout();
    }
}
