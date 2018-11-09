using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour {
    [SerializeField]
//    List<GameObject> m_Weapons;
    int[] Keys
    {
        get
        {
            int[] i = new int[GameData.Instance.WeaponInfoTable.Count];
            GameData.Instance.WeaponInfoTable.Keys.CopyTo(i, 0);
            return i;
        }
    }
    [SerializeField] bool m_interfaceID;

    // Use this for initialization
    void Start () {
	}
    private void OnEnable()
    {
        for (int i = 0; i < GameData.Instance.WeaponInfoTable.Count; i++)
        {
            Debug.Log(Keys[i]);
            if (Keys[i] == 1901) { return; }
            GameObject go = Resources.Load("Lobby/WeaponUI") as GameObject;
            go = Instantiate(go, this.transform);
            go.GetComponent<UI_Weapon>().m_ID = Keys[i];
            go.GetComponent<UI_Weapon>().SetWeaponUI() ;
            go.transform.SetAsLastSibling();
        }	
        
    }

    private void CreateWeaponUI()
    {

    }


    public void PrimaryWeapon()
    {
        m_interfaceID = true;
    }
    public void SecondaryWeapon()
    {
        m_interfaceID = false;
    }
}
