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

    public int ID { get { return m_ID; } }

    public void SetStratagemInfo(int id)
    {
        m_ID = id;
    }

}
