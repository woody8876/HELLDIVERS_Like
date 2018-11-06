///2018.09.10
///Ivan.CC
///
/// Weapon's data.
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo
{
    private float _FireRate;
    private int m_Ammo;
    private int m_Mags;

    #region Properties
    public int ID { private set; get; }
    public int Type { private set; get; }
    public string Title { private set; get; }
    public float Damage { private set; get; }
    public float Explosion_Damage { private set; get; }
    public float FireRate { get { return 1 / (_FireRate * 0.017f); } }
    public int Capacity { private set; get; }
    public int Start_Mags { private set; get; }
    public int Max_Mags { private set; get; }
    public float Empty_Reload_Speed { private set; get; }
    public float Tactical_Reload_Speed { private set; get; }
    public float Min_Spread { private set; get; }
    public float Max_Spread { private set; get; }
    public float Spread_Increase_per_shot { private set; get; }
    public float Range { private set; get; }
    public float FireMode { private set; get; }
    #endregion
    #region Set Properties
    public void SetID(int id) { ID = id; }
    public void SetType(int type) { Type = type; }
    public void SetTitle(string title) { Title = title; }
    public void SetDamage(float damage) { Damage = damage; }
    public void SetExplosion(float explosion) { Explosion_Damage = explosion; }
    public void SetFireRate(float fireRate) { _FireRate = fireRate; }
    public void SetCapacity(int capacity) { Capacity = capacity; }
    public void SetStart_Mags(int sMags) { Start_Mags = sMags; }
    public void SetMax_Mags(int mMags) { Max_Mags = mMags; }
    public void SetEmpty_Reload_Speed(float erSpeed) { Empty_Reload_Speed = erSpeed; }
    public void SetTactical_Reload_Speed(float trSpeed) { Tactical_Reload_Speed = trSpeed; }
    public void SetMin_Spread(float minSpread) { Min_Spread = minSpread; }
    public void SetMax_Spread(float maxSpread) { Max_Spread = maxSpread; }
    public void SetSpread_Increase_per_shot(float SpreadIPS) { Spread_Increase_per_shot = SpreadIPS; }
    public void SetRange(float range) { Range = range; }
    public void SetFireMode(float mode) { FireMode = mode; }
    #endregion

    public int Ammo
    {
        get { return m_Ammo; }
        set
        {
            if (value > Capacity) m_Ammo = Capacity;
            else if (value < 0) m_Ammo = 0;
            else m_Ammo = value;
        }
    }
    public int Mags
    {
        get { return m_Mags; }
        set
        {
            if (value > Max_Mags) m_Mags = Max_Mags;
            else if (value < 0) m_Mags = 0;
            else m_Mags = value;
        }
    }
    public float ReloadSpeed
    {
        get
        {
            float reloadTime = (m_Ammo <= 0) ? Empty_Reload_Speed : Tactical_Reload_Speed;
            return reloadTime;
        }
    }
}

[System.Serializable]
public class Weapon : IWeaponBehaviour
{
    protected WeaponInfo _weaponInfo;
    protected virtual int activeAmmo { get { return (int)((weaponInfo.Range * 0.01f) / weaponInfo.FireRate) + 1; } }

    #region Behaviours
    public virtual WeaponInfo weaponInfo { get { return _weaponInfo; } }
    public virtual void Init(int weaponID) { _weaponInfo = GameData.Instance.WeaponInfoTable[(int)weaponID]; }
    public virtual GameObject WeaponLoader()
    {
        string m_sWeapon = "Bullet_" + weaponInfo.Title;
        string m_sEffect = "Effect_" + weaponInfo.Title;
        Object HitMobEffect;
        Object HitObsEffect;
        Object weapon;
        GameObject effect;

        if (ResourceManager.m_Instance != null)
        {
            HitMobEffect = ResourceManager.m_Instance.LoadData(typeof(GameObject), "WeaponStorage", "HitMob", false);
            HitObsEffect = ResourceManager.m_Instance.LoadData(typeof(GameObject), "WeaponStorage", "HitObs", false);
            weapon = ResourceManager.m_Instance.LoadData(typeof(GameObject), "WeaponStorage", m_sWeapon, false);
            effect = ResourceManager.m_Instance.LoadData(typeof(GameObject), "WeaponStorage", m_sEffect, true) as GameObject;
        }
        else
        {
            Debug.LogWarning("No ResourceManager.");
            HitMobEffect = Resources.Load("WeaponStorage/" + "HitMob");
            HitObsEffect = Resources.Load("WeaponStorage/" + "HitObs");
            weapon = Resources.Load("WeaponStorage/" + m_sWeapon);
            effect = Resources.Load("WeaponStorage/" + m_sEffect) as GameObject;
        }
        if (ObjectPool.m_Instance == null) ObjectPool.m_Instance.Init();
        ObjectPool.m_Instance.InitGameObjects(HitMobEffect, activeAmmo, 10);
        ObjectPool.m_Instance.InitGameObjects(HitObsEffect, activeAmmo, 20);
        ObjectPool.m_Instance.InitGameObjects(weapon, activeAmmo, _weaponInfo.ID); 
        return effect;
    }
    public virtual void Shot(Transform t, float spread) { }
    public virtual void Reload() { }
    #endregion
}