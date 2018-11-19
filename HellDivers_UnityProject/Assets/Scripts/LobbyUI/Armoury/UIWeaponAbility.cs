using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponAbility : MonoBehaviour {

    [Header("== Set Current Ability ==")]
    [SerializeField] Text m_WeaponName;
    [Header("== Set Current Value ==")]
    [SerializeField] RectTransform m_Current;
    [Header("== Set LevelUP Value ==")]
    [SerializeField] RectTransform m_LevelUP;
    [Header("== Set GameObject ==")]
    [SerializeField] GameObject m_Ability;
    [SerializeField] Text m_FireMode;
    Vector2 vec = new Vector2();

    public void SetAbility(string name, float currentWidth, float nextWidth)
    {
        m_Ability.SetActive(true);
        m_WeaponName.text = name;
        vec.Set(currentWidth, 35);
        m_Current.sizeDelta = vec;
        vec.Set(nextWidth, 35);
        m_LevelUP.sizeDelta = vec;
    }

    public void SetWord(string name, string mode)
    {
        m_FireMode.gameObject.SetActive(true);
        m_WeaponName.text = name;
        m_FireMode.text = mode;
    }

}
