using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T : MonoBehaviour {

    public float m_fSpeed = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            m_fSpeed = 100;
        }
        this.transform.position += transform.forward * Time.deltaTime * m_fSpeed;

    }
    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
        other.gameObject.SetActive(false);
    }
}
