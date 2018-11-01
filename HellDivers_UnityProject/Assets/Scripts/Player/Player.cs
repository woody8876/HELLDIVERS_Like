using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(StratagemController))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(CharacterController))]
public class Player : Character
{
    #region Properties

    /// <summary>
    /// Player weapon behavior controller.
    /// </summary>
    public WeaponController WaeponController { get { return m_WeapoonController; } }

    /// <summary>
    /// Player stratagem behavior controller.
    /// </summary>
    public StratagemController StratagemController { get { return m_StratagemController; } }

    /// <summary>
    /// Current player infos.
    /// </summary>
    public PlayerInfo Info { get { return m_Data; } }

    #endregion Properties

    #region Private Variable

    private PlayerInfo m_Data;
    private PlayerParts m_Parts;
    private PlayerController m_Controller;
    private StratagemController m_StratagemController;
    private WeaponController m_WeapoonController;

    #endregion Private Variable

    #region Initializer

    /// <summary>
    /// Initialize by player info to setup player's weapons and stratagems.
    /// </summary>
    /// <param name="data"></param>
    public void Initialize(PlayerInfo data)
    {
        data.CopyTo(m_Data);

        // Setup stratagems
        if (m_StratagemController.Stratagems.Count > 0) m_StratagemController.Clear();
        m_StratagemController.AddStratagems(m_Data.Stratagems, m_Parts.RightHand, m_Parts.LaunchPoint);

        // Setup weapons
        m_WeapoonController.ClearWeapon();
        m_WeapoonController.AddMultiWeapons(m_Data.Weapons, m_Parts.LaunchPoint);
    }

    #endregion Initializer

    #region MonoBehaviour

    private void Awake()
    {
        this.tag = "Player";
        m_Data = new PlayerInfo();
        m_Parts = GetComponent<PlayerParts>();
        m_Controller = GetComponent<PlayerController>();
        m_WeapoonController = GetComponent<WeaponController>();
        m_StratagemController = GetComponent<StratagemController>();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    #endregion MonoBehaviour

    #region Public Function

    /// <summary>
    /// Set player spawn on the position.
    /// </summary>
    /// <param name="spawnPos">Spawn position</param>
    public void Spawn(Vector3 spawnPos)
    {
        m_CurrentHp = m_MaxHp;

        this.transform.position = spawnPos;
        m_StratagemController.ResetAllUses();

        // Setup weapons
        m_WeapoonController.ClearWeapon();
        m_WeapoonController.AddMultiWeapons(m_Data.Weapons, m_Parts.LaunchPoint);

        this.gameObject.SetActive(true);
        StartCoroutine(OnSpawn());
    }

    /// <summary>
    /// Increase current health point HP.
    /// </summary>
    /// <param name="heal">Heal point</param>
    /// <returns>Was the current health point increased or not ?</returns>
    public bool TakeHealth(float heal)
    {
        if (IsDead || m_CurrentHp >= m_MaxHp) return false;

        CurrentHp += heal;
        return true;
    }

    /// <summary>
    /// Decrease current health point by damage point.
    /// </summary>
    /// <param name="dmg">Damage point</param>
    /// <param name="hitPoint">Hit point position</param>
    /// <returns>Was the current health point decreased or not ?</returns>
    public override bool TakeDamage(float dmg, Vector3 hitPoint)
    {
        bool bHurt = m_Controller.PerformPlayerHurt();
        if (bHurt == false) return false;

        return base.TakeDamage(dmg, hitPoint);
    }

    /// <summary>
    /// Do dead. perform daead animation then disable this player.
    /// </summary>
    [ContextMenu("DoDeath")]
    public override void Death()
    {
        if (IsDead) return;
        m_bDead = true;
        StartCoroutine(DoDeath());
    }

    /// <summary>
    /// Interact with items which are exist in interactive item manager.
    /// </summary>
    public void InteractWithItem()
    {
        if (InteractiveItemManager.Instance == null) return;
        InteractiveItemManager.Instance.OnInteractive(this);
    }

    #endregion Public Function

    #region Private Function

    /*--------------------------------------------------------
     * Check animation time was finished and set this alive. *
     --------------------------------------------------------*/

    private IEnumerator OnSpawn()
    {
        if (m_Controller != null)
        {
            m_Controller.PerformPlayerRelive();
            yield return new WaitUntil(() => m_Controller.bIsAlive);
        }

        GameMain.Instance.CameraFolloing.AddTarget(this.transform);
        m_bDead = false;
    }

    /*------------------------------------------------------
     * Check animation time was finished and disable this. *
     ------------------------------------------------------*/

    private IEnumerator DoDeath()
    {
        if (m_Controller != null)
        {
            m_Controller.PerformPlayerDead();
            yield return new WaitUntil(() => m_Controller.bIsDead);
        }

        GameMain.Instance.CameraFolloing.RemoveTarget(this.transform);

        GameMain.Instance.RespawnPlayer(this);
        this.gameObject.SetActive(false);
    }

    #endregion Private Function
}