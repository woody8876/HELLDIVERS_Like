using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eRadarPointType
{
    PLAYER,
    FISH,
    FISHVARIANT,
    PATROL,
    TANK,
}
public class UIRadarPoint : MonoBehaviour
{
    private Vector3 m_Pos = new Vector3();
    private Vector3 m_Center = new Vector3();
    private Vector3 m_Dir = new Vector3();
    private int m_PlayersCount = 0;
    [SerializeField] private Image m_Image;
    private Color m_Color;
    private bool m_Blink = false;
    public GameObject CurrentTarget { get { return m_CurrentTarget; } }
    private GameObject m_CurrentTarget;

    public void Init(GameObject target,eRadarPointType type)
    {
        m_CurrentTarget = target;
        m_Image = this.GetComponent<Image>();
        m_Color = m_Image.color;
        m_Blink = false;
        FindRadarCenter();
        CalculatePosition();
        switch (type)
        {
            case eRadarPointType.FISH:
                FishAI fish = target.GetComponent<FishAI>();
                fish.OnSpawn += ShowPoint;
                fish.OnDeath += HidePoint;
                break;
        }
        UIPanelRadar.Instance.UpdatePoint += UpdatePosition;
    }

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Blink)
        {
            Blink();
        }
        UIPanelRadar.Instance.UpdatePoint += UpdatePosition;
    }

    private void OnDestroy()
    {
        if (m_CurrentTarget == null) return;
        FishAI fish = m_CurrentTarget.GetComponent<FishAI>();
        fish.OnSpawn -= ShowPoint;
        fish.OnDeath -= HidePoint;

        //UIPanelRadar.Instance.UpdatePoint -= UpdatePosition;
    }

    private void ShowPoint()
    {
        this.gameObject.SetActive(true);
        CalculatePosition();
    }

    private void HidePoint()
    {
        ObjectPool.m_Instance.UnLoadObjectToPool(3002, this.gameObject);
    }

    private void FindRadarCenter()
    {
        List<Player> pList = InGamePlayerManager.Instance.Players;
        if (pList == null) return;
        m_Center.Set(0, 0, 0);
        for (int i = 0; i < pList.Count; i++)
        {
            if (pList[i].IsDead) continue;
            m_PlayersCount++;
            m_Center += pList[i].transform.position;
        }
        m_Center /= m_PlayersCount;
        m_PlayersCount = 0;
    }

    private void CalculatePosition()
    {
        m_Dir = m_CurrentTarget.transform.position - m_Center;
        m_Pos = this.transform.localPosition;
        m_Pos.x = m_Dir.x * (UIPanelRadar.Instance.RectWidth * 0.5f) / UIPanelRadar.Instance.RadarRadius;
        m_Pos.y = m_Dir.z * (UIPanelRadar.Instance.RectHeight * 0.5f) / UIPanelRadar.Instance.RadarRadius;
        this.transform.localPosition = m_Pos;
    }

    private void UpdatePosition()
    {
        FindRadarCenter();
        m_Dir = m_CurrentTarget.transform.position - m_Center;
        m_Pos = this.transform.localPosition;
        m_Pos.x = m_Dir.x * (UIPanelRadar.Instance.RectWidth * 0.5f) / UIPanelRadar.Instance.RadarRadius;
        m_Pos.y = m_Dir.z * (UIPanelRadar.Instance.RectHeight * 0.5f) / UIPanelRadar.Instance.RadarRadius;
        this.transform.localPosition = m_Pos;
        m_Color.r = 1f;
        m_Color.a = 1f;
        m_Image.color = m_Color;
        m_Blink = true;
    }

    private void Blink()
    {
        m_Color.r -= Time.deltaTime * 0.2f;
        m_Color.a -= Time.deltaTime * 0.4f;
        m_Image.color = m_Color;
        if (m_Color.a <= 0) m_Blink = false;
    }
}
