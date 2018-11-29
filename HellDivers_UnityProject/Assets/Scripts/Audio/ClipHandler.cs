﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClipHandler : MonoBehaviour {

    [SerializeField] int m_ID;
    [SerializeField] AudioSource m_Audio;

    public void SetID (int i) { m_ID = i; }

    private void UnLoad() { ObjectPool.m_Instance.UnLoadObjectToPool(m_ID, this.gameObject); }

    IEnumerator WaitToUnLoad()
    {
        yield return new WaitForSeconds(m_Audio.clip.length);
        UnLoad();
    } 

    private void OnEnable()
    {
        StartCoroutine(WaitToUnLoad());
    }

}
