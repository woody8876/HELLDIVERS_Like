using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTest : MonoBehaviour {

    float i;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        i = Random.Range(0.0f, 1.0f);
        Debug.Log(i);
	}
}
