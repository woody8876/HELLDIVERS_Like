using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

    bool m_bBlack;
    Color color;
    private void OnEnable()
    {
        m_bBlack = false;
        color = GetComponent<Image>().color;
    }
    private void LateUpdate()
    {
        if (!m_bBlack)
        {
            color.a = Mathf.Lerp(color.a, 1, 0.03f);
            GetComponent<Image>().color = color;
            if (GetComponent<Image>().color.a > .95f) m_bBlack = true;
        }
        else
        {
            color.a = Mathf.Lerp(color.a, 0, 0.01f);
            GetComponent<Image>().color = color;
            if (GetComponent<Image>().color.a == 0) gameObject.SetActive(false);
        }
    }

}
