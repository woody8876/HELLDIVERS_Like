using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class Title_Continue : MonoBehaviour {

    [SerializeField] GameObject m_FadePanel;
	// Use this for initialization
	void Start () {
        Debug.Log("Titli_Continue Start");
        Button btn = GetComponentInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(btn.gameObject);
        btn.onClick.AddListener(() => StartCoroutine(LoadScene()));
	}
	

    IEnumerator LoadScene()
    {
        m_FadePanel.SetActive(true);
        yield return new WaitForSeconds(.4f);
        SceneManager.LoadScene(1);
    }
}
