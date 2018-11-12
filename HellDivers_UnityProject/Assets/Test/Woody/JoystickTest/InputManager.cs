using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager{


    public Dictionary<int, InputInfo> PlayerInputInfoTable;

    public void Init()
    {
        PlayerInputInfoTable = new Dictionary<int, InputInfo>();
        Load();
    }
    public void Load()
    {
        PlayerInputInfoTable = PlayerInputLoader.LoadData("PlayerInputTable");
    }
}
