using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewGameController : MonoBehaviour {

    float m_time = 0;
    float m_Speed = 1.5f;
    bool m_bstart = false;
    bool m_bContinue;
    [SerializeField] GameObject m_Plot;
    [SerializeField] GameObject m_Continue;
    [SerializeField] GameObject m_FadePanel;

    // Use this for initialization
    void Start () {
        Debug.Log("Panel Controller start.");
        Button btn = m_Continue.GetComponentInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(btn.gameObject);
        btn.onClick.AddListener(() => StartCoroutine(LoadScene()));

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (m_bstart)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                m_Plot.transform.position = Vector3.Lerp(m_Plot.transform.position, m_Plot.transform.position + m_Plot.transform.up * m_Speed * 800, 1f);
            }
            else if (m_bstart && m_Plot.transform.position.y < 400)
            {
                if (Input.GetKey(PlayerManager.Instance.Players[1].controllerSetting.Submit)) { m_Speed = 3.5f; }
                else { m_Speed = 1; }
                m_Plot.transform.position = Vector3.Lerp(m_Plot.transform.position, m_Plot.transform.position + m_Plot.transform.up * m_Speed, 0.7f);
            }
            else if (m_Plot.transform.position.y >= 400)
            {
                m_bstart = false;
                m_Continue.SetActive(true);
                m_Plot.gameObject.SetActive(false);
                m_bContinue = true;
            }
        }
        else
        {
            if (m_time < 2.5f) m_time += Time.fixedDeltaTime;
            else { m_bstart = true; }
        }
        if (m_bContinue)
        {
            if (Input.GetKey(PlayerManager.Instance.Players[1].controllerSetting.Submit))
            {
                m_Continue.GetComponentInChildren<Button>().onClick.Invoke();
            }
        }
	}

    IEnumerator LoadScene()
    {
        m_FadePanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneController.Instance.ToLobby();
    }

}
