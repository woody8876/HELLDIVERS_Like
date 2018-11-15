using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenAlpha : MonoBehaviour
{
    [SerializeField] private Image m_Image;
    private float m_stratAlpha;
    private float m_targetAlpha;

    public void StartFadeIn()
    {
    }

    public void StartFadeOut()
    {
    }

    private void DoFade(float origin, float target)
    {
        float alpha = Mathf.Lerp(origin, target, Time.deltaTime);
    }

    private void Awake()
    {
        if (m_Image == null) m_Image = this.GetComponent<Image>();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}