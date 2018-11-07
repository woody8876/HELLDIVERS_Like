using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    public static class UIHelper
    {
        public static readonly string RankIconFolder = "UI/Resource/Icons/Rank";
        public static readonly string StratagemIconFolder = "UI/Resource/Icons/Stratagem";
        public static readonly string WeaponIconFolder = "UI/Resource/Icons/Weapon";
        public static readonly string GrenadeIconFolder = "UI/Resource/Icons/Grenade";

        public static Sprite LoadSprite(string path, string fileName)
        {
            Sprite sprite = null;

            string fullPath = path + "/" + fileName;

            if (AssetManager.m_Instance != null)
            {
                sprite = AssetManager.m_Instance.GetAsset(typeof(Sprite), fileName, path) as Sprite;
                if (sprite == null)
                {
                    sprite = Resources.Load<Sprite>(fullPath);
                    AssetManager.m_Instance.AddAsset(typeof(Sprite), fileName, path, sprite);
                }
            }
            else
            {
                sprite = Resources.Load<Sprite>(fullPath);
            }
            return sprite;
        }
    }
}