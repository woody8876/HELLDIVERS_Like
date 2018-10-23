using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("TestButtonY")>0)
        {
            Debug.Log(Input.GetAxis("TestButtonY"));
        }
        if (Input.GetAxis("TestButtonY") < 0)
        {
            Debug.Log(Input.GetAxis("TestButtonY"));
        }
        if (Input.GetAxis("TestButtonX") > 0)
        {
            Debug.Log(Input.GetAxis("TestButtonX"));
        }
        if (Input.GetAxis("TestButtonX") < 0)
        {
            Debug.Log(Input.GetAxis("TestButtonX"));
        }
    }
}
