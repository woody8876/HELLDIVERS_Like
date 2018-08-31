using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForward : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Input.mousePosition;
        Vector3 vec;

        Ray r = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit rh;

        Physics.Raycast(r, out rh);

        vec = rh.point - this.transform.position;
        transform.forward = vec.normalized;
    }
}
