using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTest : MonoBehaviour {

    private Animator m_Animator;
    Object obj;
    Vector3 vec;
    GameObject GO;
    // Use this for initialization
    void Start () {
        obj = Resources.Load("Test");
        vec = this.transform.position;
        vec.y += 0.2f;
        GO = GameObject.Instantiate(obj, this.transform) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_Animator = GO.GetComponent<Animator>();
            m_Animator.SetTrigger("startTrigger");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            m_Animator = GO.GetComponent<Animator>();
            m_Animator.SetTrigger("endTrigger");
        }

    }
}
