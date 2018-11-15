using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarBG : MonoBehaviour {

    private Image m_Image;
    private Color m_Color;
    private bool bAdd = true;
	// Use this for initialization
	void Start () {
        m_Image = this.GetComponent<Image>();
        m_Color = m_Image.color;
    }
	
	// Update is called once per frame
	void Update () {
        if(bAdd)
        {
            
            m_Color.g += Time.deltaTime * 0.2f;
            m_Image.color = m_Color;
            if (m_Color.g > 0.2) bAdd = false;
        }
        else if(bAdd == false)
        {
            m_Color.g -= Time.deltaTime * 0.2f;
            m_Image.color = m_Color;
            if (m_Color.g <= 0) bAdd = true;
        }
    }
}
