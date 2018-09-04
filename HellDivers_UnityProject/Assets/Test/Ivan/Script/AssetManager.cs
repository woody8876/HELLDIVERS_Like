///2018.09.02
/// Ivan.C
///
/// Manage the Asset data.
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager
{
    public class AssetObject
    {
        public Object m_Asset;
        public string m_sName;
        public string m_sPath;
    }

    static public AssetManager m_Instance;

    private Dictionary<System.Type, List<AssetObject>> m_AssetMap;

    /// <summary>
    /// Initial this script
    /// </summary>
    public void Init()
    {
        m_Instance = this;
        m_AssetMap = new Dictionary<System.Type, List<AssetObject>>();
    }

    /// <summary>
    /// Add asset to this dictionary
    /// </summary>
    /// <param name="type">asset type</param>
    /// <param name="sName">asset name</param>
    /// <param name="sPath">asset path</param>
    /// <param name="o">asset </param>
    public void AddAsset(System.Type type, string sName, string sPath, Object o)
    {
        List<AssetObject> pList= null;
        if (m_AssetMap.ContainsKey(type) == false)
        {
            pList = new List<AssetObject>();
            m_AssetMap.Add(type, pList);
        }
        pList = m_AssetMap[type];
        foreach (AssetObject tempO in pList)
        {
            if (tempO.m_sName.Equals(sName) && tempO.m_sPath.Equals(sPath))
            {
                return;
            }
        }
        AssetObject ao = new AssetObject();
        ao.m_sName = sName;
        ao.m_sPath = sPath;
        ao.m_Asset = o;
        pList.Add(ao);
    }

    /// <summary>
    /// Get asset from m_AssetMap(Dictionarry
    /// </summary>
    /// <param name="type">asset type</param>
    /// <param name="sName">asset name</param>
    /// <param name="sPath">asset path</param>
    /// <returns></returns>
    public Object GetAsset(System.Type type, string sName, string sPath)
    {
        if (m_AssetMap.ContainsKey(type) == false)
        {
            return null;
        }
        List<AssetObject> pList = m_AssetMap[type];

        if (pList == null) { return null; }

        for (int i = 0; i < pList.Count; i++)
        {
            if (pList[i].m_sName.Equals(sName) && pList[i].m_sPath.Equals(sPath))
            {
                Debug.Log("Get data from Asset :" + sName);
                return pList[i].m_Asset;
            }
        }
        return null;
    }

    /// <summary>
    /// Clear all data in m_AssetMap
    /// </summary>
    public void ClearAsset()
    {
        foreach (var data in m_AssetMap)
        {
            List<AssetObject> pList = data.Value;
            pList.Clear();
        }
        m_AssetMap.Clear();
        Resources.UnloadUnusedAssets();
    }

}
