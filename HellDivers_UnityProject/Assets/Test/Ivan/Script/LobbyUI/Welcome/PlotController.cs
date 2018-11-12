using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotController : MonoBehaviour {

    float m_time = 0;
    bool m_bstart = false;
    [SerializeField] GameObject m_PlayerID;
    [SerializeField] GameObject m_Continue;
	// Use this for initialization
	void Start () {
        Debug.Log("Panel Controller start.");
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        if (m_time < 2.5f) m_time += Time.fixedDeltaTime;
        else { m_bstart = true; }
        if (m_bstart && transform.position.y <400)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.up, 0.7f);
        }
        if (transform.position.y >= 400) {
            m_bstart = false;
            m_Continue.SetActive(true);
            this.gameObject.SetActive(false);

        }
	}
}
