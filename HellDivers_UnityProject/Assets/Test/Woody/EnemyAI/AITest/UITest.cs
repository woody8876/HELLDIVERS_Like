using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour {

    [SerializeField] private Text m_Introduction;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            m_Introduction.text = "Fuck off";
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_Introduction.text = "Fuck on";
        }
    }
}
