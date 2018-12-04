using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProducerMain : MonoBehaviour {

    Dictionary<string, List<string>> m_producer = new Dictionary<string, List<string>>();
    [SerializeField] Text m_Content;
	// Use this for initialization
	void Awake () {
        m_producer = ProducerInfoLoder.LoadData("Table/ProducerTable");
        foreach (var item in m_producer)
        {
            m_Content.text = item.Key + "\n";
        }
	}
	
	// Update is called once per frame
	void Start () {
	}
}
