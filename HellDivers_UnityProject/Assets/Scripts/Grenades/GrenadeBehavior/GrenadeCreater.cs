using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeCreater : IGrenadesBehaviors {

    protected GrenadeInfo _grenadeInfo;

    public virtual GrenadeInfo grenadeInfo { get { return _grenadeInfo; } }

    public virtual void Init(int grenadeID)
    {
        _grenadeInfo = new GrenadeInfo();
        GameData.Instance.GrenadeInfoTable[grenadeID].CopyTo(_grenadeInfo);
        string m_sGrenade = "Grenade_" + _grenadeInfo.Title;
        string m_sEffect = "Effect_" + _grenadeInfo.Title;

        Object grenade;
        Object effect;

        if (ResourceManager.m_Instance != null)
        {
            grenade = ResourceManager.m_Instance.LoadData(typeof(GameObject), "GrenadesStorage/Grenades", m_sGrenade, false);
            effect = ResourceManager.m_Instance.LoadData(typeof(GameObject), "GrenadesStorage/Effects", m_sEffect, false);
        }
        else
        {
            grenade = Resources.Load("GrenadesStorage/Grenades/" + m_sGrenade);
            effect = Resources.Load("GrenadesStorage/Effects/" + m_sEffect);
        }
        if (ObjectPool.m_Instance == null) ObjectPool.m_Instance.Init();
        ObjectPool.m_Instance.InitGameObjects(grenade, _grenadeInfo.MaxCount, grenadeID);
        ObjectPool.m_Instance.InitGameObjects(effect, _grenadeInfo.MaxCount, grenadeID + 100);
    }

    public virtual void Throw(ref GameObject go, Transform t)
    {
        go.transform.position = t.position;
        go.transform.forward = t.forward;
        go.transform.localScale *= 5f;
    }


}
