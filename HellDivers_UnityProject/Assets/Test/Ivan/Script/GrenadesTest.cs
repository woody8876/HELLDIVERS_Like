using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadesTest : MonoBehaviour {
    [SerializeField] GrenadesController grenadesController;
    bool bHolding;

    List<int> grenades = new List<int>() { 4001, 4002, 4003, 4004 };
	// Use this for initialization
	void Start () {
//        grenadesController.AddGrenades(grenades, transform, transform);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.E))
        {
            bHolding = grenadesController.Holding();
        }
        if(bHolding && !Input.GetKey(KeyCode.E))
        {
            grenadesController.Throw();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            grenadesController.ResetGrenades();
        }
    }
}
