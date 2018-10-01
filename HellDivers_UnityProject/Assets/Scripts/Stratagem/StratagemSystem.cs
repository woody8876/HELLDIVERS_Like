using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StratagemSystem
{
    public static string DisplayFolder { get { return Setting.DisplayFolder; } }
    public static GameObject DefaultDisplay { get { return Setting.DefaultDisplay; } }

    private static StratagemSetting Setting
    {
        get
        {
            if (setting == null)
            {
                setting = Resources.Load<StratagemSetting>("StratagemSetting");
                if (setting == null)
                {
                    Debug.LogErrorFormat("Stratagem Error : Load setting file faild");
                }
            }
            return setting;
        }
    }

    private static StratagemSetting setting;
}