using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    public delegate void UIEventHolder();

    public static class UIHelper
    {
        public static string RankIconFolder { get { return Setting.RankIconFolder; } }
        public static string StratagemIconFolder { get { return Setting.StratagemIconFolder; } }
        public static string WeaponIconFolder { get { return Setting.WeaponIconFolder; } }
        public static string GrenadeIconFolder { get { return Setting.GrenadeIconFolder; } }
        public static string MissionIconFolder { get { return Setting.MissionIconFolder; } }
        public static Color Player1_Color { get { return Setting.Player1Color; } }
        public static Color Player2_Color { get { return Setting.Player2Color; } }

        public static Color GetPlayerColor(int num)
        {
            switch (num)
            {
                case 1:
                    return Setting.Player1Color;

                case 2:
                    return Setting.Player2Color;
            }

            return Color.white;
        }

        private static UISetting Setting
        {
            get
            {
                if (setting == null)
                {
                    setting = Resources.Load<UISetting>("UISetting");
                    if (setting == null)
                    {
                        Debug.LogErrorFormat("UISetting Error : Load setting file faild");
                    }
                }
                return setting;
            }
        }

        private static UISetting setting;
    }
}