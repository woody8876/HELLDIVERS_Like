using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputrManager
{
    public static InputrManager Instance { get { return m_Instance; } }
    private static InputrManager m_Instance;
    public Dictionary<int, ControllerSetting> InputSettingMap { get { return m_InputSettingMap; } }
    private Dictionary<int, ControllerSetting> m_InputSettingMap;

    public void Init()
    {
        if (m_Instance == null) m_Instance = this;
        m_InputSettingMap = new Dictionary<int, ControllerSetting>();

        ControllerSetting[] settings = Resources.LoadAll<ControllerSetting>("InputSettings");
        foreach (ControllerSetting s in settings)
        {
            char cID = s.name[0];
            int iID = int.Parse(cID.ToString());
            m_InputSettingMap.Add(iID, s);
        }
    }
}
