using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StratagemsInfo : MonoBehaviour {

    [Header("== Detail Setting ==")]
    [SerializeField] Text m_ItemName;
    [SerializeField] Text m_Cooldown;
    [SerializeField] Text m_Activation;
    [SerializeField] Image m_Item;
    [Header("== Private Field ==")]
    [SerializeField] int m_ID;
    private string cooldown = "Cooldown: ";
    private string activation = "Activation time: ";
    public int ID { get { return m_ID; } }

    public void SetStratagemInfo(int id)
    {
        m_ID = id;
        string s = " seconds";
        m_ItemName.text = GameData.Instance.StratagemTable[m_ID].Title;
        m_Cooldown.text = cooldown + GameData.Instance.StratagemTable[m_ID].CoolDown.ToString();
        m_Activation.text = activation + GameData.Instance.StratagemTable[m_ID].Activation.ToString();
        SetUI(m_ID);
    }

    private void SetUI(int id, bool b = false)
    {
        StratagemInfo info = GameData.Instance.StratagemTable[id];
        string s = (b) ? "_gray" : "";
        m_Item.sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), HELLDIVERS.UI.UIHelper.StratagemIconFolder, "icon_" + id + s, false);
    }

}
