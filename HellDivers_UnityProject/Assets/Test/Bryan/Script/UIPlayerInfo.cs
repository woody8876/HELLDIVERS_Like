using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    public Player CurrentPlayer { get { return m_Player; } }

    private Player m_Player;
    private UIPlayerWeaponInfo m_UIWeapon;
    private Dictionary<int, UIPlayerStratagemInfo> m_UIStratagems;
    [SerializeField] private Text m_PlayerName;
    [SerializeField] private Text m_PlayerRank;
    [SerializeField] private Image m_PlayerRankImg;
    [SerializeField] private GameObject m_WeaponInfoPrefab;
    [SerializeField] private GameObject m_StratagemInfoPrefab;

    public void Initialize(Player player)
    {
        m_Player = player;

        InitTitleDisplay();
        InitWeaponDisplay();
        InitStratagemDisplay();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        m_UIWeapon.UpdateAmmoDisplay();

        foreach (KeyValuePair<int, UIPlayerStratagemInfo> stratagemUI in m_UIStratagems)
        {
            stratagemUI.Value.UpdateUses();
        }
    }

    private void InitTitleDisplay()
    {
        if (m_Player == null) return;

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

        string rankImgPath = string.Format("UI/Resource/Icons/Rank/{0}", rankImgFile);
        Sprite rankImg = Resources.Load<Sprite>(rankImgPath);
        m_PlayerRankImg.sprite = rankImg;
    }

    private void InitWeaponDisplay()
    {
        if (m_Player == null) return;

        m_UIWeapon = Instantiate(m_WeaponInfoPrefab, this.transform).GetComponent<UIPlayerWeaponInfo>();
        m_UIWeapon.Initialize(m_Player.WaeponController.CurrentWeaponInfo);
        m_UIWeapon.gameObject.SetActive(true);
    }

    private void InitStratagemDisplay()
    {
        if (m_Player == null) return;

        m_UIStratagems = new Dictionary<int, UIPlayerStratagemInfo>();
        for (int i = 0; i < m_Player.StratagemController.Stratagems.Count; i++)
        {
            Stratagem currentStratagem = m_Player.StratagemController.Stratagems[i];
            UIPlayerStratagemInfo stratagemUI = Instantiate(m_StratagemInfoPrefab, this.transform).GetComponent<UIPlayerStratagemInfo>();
            stratagemUI.Initialize(currentStratagem);
            m_UIStratagems.Add(currentStratagem.Info.ID, stratagemUI);
        }
    }
}