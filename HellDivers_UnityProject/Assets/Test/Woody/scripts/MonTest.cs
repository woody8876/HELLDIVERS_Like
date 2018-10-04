using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonTest : MonoBehaviour {
    private float speed;
	// Use this for initialization
	void Start () {
        speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.x > 5) speed = -1;
        if (this.transform.position.x < -5) speed = 1;
        this.transform.position += new Vector3(speed * Time.deltaTime,0,0);
    }
}
