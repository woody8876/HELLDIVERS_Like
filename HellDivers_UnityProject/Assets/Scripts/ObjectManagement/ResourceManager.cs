using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public static ResourceManager m_Instance;

    /// <summary>
    /// Initial this script
    /// </summary>
    public void Init()
    {
        m_Instance = this;
    }

    public Object LoadData(System.Type t, string sPath, string sName, bool bInstatiate = false)
    {
        Object o = AssetManager.m_Instance.GetAsset(t, sName, sPath);

        if (o == null)
        {
            string sFullPath = sPath + "/" + sName;
            o = Resources.Load(sFullPath);
            AssetManager.m_Instance.AddAsset(t, sName, sPath, o);
        }
        if (bInstatiate) { o = GameObject.Instantiate(o); }
        return o;
    }

    public Sprite LoadSprite(System.Type t, string sPath, string sName, bool bInstatiate = false)
    {
        Sprite sprite = null;
        string fullPath = sPath + "/" + sName;

        if (AssetManager.m_Instance != null)
        {
            sprite = AssetManager.m_Instance.GetAsset(t, sName, sPath) as Sprite;
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>(fullPath);
                AssetManager.m_Instance.AddAsset(t, sName, sPath, sprite);
            }
            if (bInstatiate) { sprite = GameObject.Instantiate(sprite); }
        }
        else { sprite = Resources.Load<Sprite>(fullPath); }
        return sprite;

    }
}