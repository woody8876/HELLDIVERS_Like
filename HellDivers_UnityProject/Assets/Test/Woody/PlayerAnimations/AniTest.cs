using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniTest : MonoBehaviour {

    public Animator m_Animator;
	// Use this for initialization
	void Start () {
        m_Animator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        //AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(0);

        //Debug.Log(info.IsName("Walk"));
        //if(info.normalizedTime >= 0.99f)
        //{
        //    Debug.Log("Complete");
        //}

    }
}
