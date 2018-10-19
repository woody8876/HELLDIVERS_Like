using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestLine : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DrawTools.GO = GameObject.Find("Range");
        DrawTools.DrawCircleSolid(transform, transform.position, 10);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawTools.DrawRectangleSolid(transform, transform.position, 20, 2);
        }
	}
}
