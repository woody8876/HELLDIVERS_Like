using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    

    public Dictionary<int, InputInfo> PlayerInputInfoTable;
    // Use this for initialization
    private void Awake()
    {
        PlayerInputInfoTable = new Dictionary<int, InputInfo>();
    }
    void Start () {
        PlayerInputInfoTable = PlayerInputLoader.LoadData("PlayerInputTable");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {

        }
	}
}
