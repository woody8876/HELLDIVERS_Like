using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStratagemsList : MonoBehaviour {

    [SerializeField] UIStratagemsDisplay stratagemsDisplay;
    [SerializeField] GameObject m_Content;
    [SerializeField] GameObject m_StratagemUI;

    List<GameObject> m_Stratagems = new List<GameObject>();

    GameObject m_currentSelectObject;
	// Use this for initialization
	void Start ()
    {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateStratagemUI(int player)
    {
        GameObject go;
        List<int> unlockStratagems = PlayerManager.Instance.Players[player].info.UnlockedStratagems;
        for (int i = 0; i < unlockStratagems.Count; i++)
        {
            go = Instantiate(m_StratagemUI, m_Content.transform);
            int id = unlockStratagems[i];
            go.name = id.ToString();
            stratagemsDisplay.SetStratagemUI(go, id);
            m_Stratagems.Add(go);
        }
    }

    private void ChangeStratagem()
    {
        for (int i = 0; i < m_Stratagems.Count; i++)
        {
            if (stratagemsDisplay.CurStratagemID.ToString() == m_Stratagems[i].name)
            {
                m_currentSelectObject = m_Stratagems[i];
            }
        }
    }

    #region Button Navigation
    private void ButtonNavUP()
    {
        GameObject go = m_currentSelectObject;
        if (go == m_Stratagems[0] || go == m_Stratagems[1] || go == m_Stratagems[2]) return;
        for (int i = 3; i < m_Stratagems.Count; i++)
        {
            if (go == m_Stratagems[i].gameObject)
            {
                m_currentSelectObject = m_Stratagems[i - 3].gameObject;
                return;
            }
        }
    }
    private void ButtonNavDown()
    {
        GameObject go = m_currentSelectObject;
        int left = m_Stratagems.Count % 3;
        switch (left)
        {
            case 0:
                if (go == m_Stratagems[m_Stratagems.Count - 1 ]
                    || go == m_Stratagems[m_Stratagems.Count - 2] 
                    || go == m_Stratagems[m_Stratagems.Count - 3]) return;
                for (int i = 0; i < m_Stratagems.Count-3; i++)
                {
                    if (go == m_Stratagems[i].gameObject)
                    {
                        m_currentSelectObject = m_Stratagems[i + 3].gameObject;
                        return;
                    }
                }
                break;
            case 1:
                for (int i = 0; i < m_Stratagems.Count; i++)
                {
                    if (go == m_Stratagems[i].gameObject)
                    {
                        if (i + 3 > m_Stratagems.Count - 1) m_currentSelectObject = m_Stratagems[m_Stratagems.Count - 1].gameObject;
                        else { m_currentSelectObject = m_Stratagems[i + 3].gameObject; }
                        return;
                    }
                }
                break;
            case 2:
                if (go == m_Stratagems[m_Stratagems.Count - 1] 
                    || go == m_Stratagems[m_Stratagems.Count - 2]) return;
                for (int i = 0; i < m_Stratagems.Count-2; i++)
                {
                    if (go == m_Stratagems[i].gameObject)
                    {
                        if (i + 3 > m_Stratagems.Count - 1) m_currentSelectObject = m_Stratagems[m_Stratagems.Count - 1].gameObject;
                        else { m_currentSelectObject = m_Stratagems[i + 3].gameObject; }
                        return;
                    }
                }
                break;
        }

    }

    private void ButtonNavLeft()
    {
        GameObject go = m_currentSelectObject;
        //if(go == )
    } 
    private void ButtonNavRight()
    {
    }
    #endregion
    private void SetInfo(GameObject go)
    {
        int id = int.Parse(go.name);
        stratagemsDisplay.Info.SetStratagemInfo(id);
    }
    private void OnValueChange(GameObject go) { }

    private void OnSelectEvent(GameObject go)
    {
        SetInfo(go);
        OnValueChange(go);
        go.GetComponent<LobbyUI_Stratagems>().SetHighlightBG();
    }
    private void DisSelectEvent(GameObject go)
    {
        go.GetComponent<LobbyUI_Stratagems>().SetBG();
    }
    private void OnClickEvent()
    {
        stratagemsDisplay.SetCurID(stratagemsDisplay.Info.ID);
        DisSelectEvent(m_currentSelectObject);

    }
}
