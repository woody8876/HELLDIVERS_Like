using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundData
{
    public int Id;
    public AudioClip audioClip;
    public float volumeScale = 1;
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public AudioSource AudioSource { get { return m_Audio; } }
    public Dictionary<int, SoundData> AudioClips { get { return m_ClipsMap; } }

    private AudioSource m_Audio;
    private Dictionary<int, SoundData> m_ClipsMap = new Dictionary<int, SoundData>();

    private void Awake()
    {
        m_Audio = this.GetComponent<AudioSource>();
    }

    public void SetAudioClips(List<SoundData> datas)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            SetAudioClip(datas[i]);
        }
    }

    public void SetAudioClip(SoundData data)
    {
        if (m_ClipsMap.ContainsKey(data.Id))
        {
            m_ClipsMap[data.Id] = data;
        }
        else
        {
            m_ClipsMap.Add(data.Id, data);
        }
    }

    public void PlayOnce(int Id, float volumeScale = 1)
    {
        if (m_ClipsMap.ContainsKey(Id) == false) return;
        m_Audio.PlayOneShot(m_ClipsMap[Id].audioClip, volumeScale);
    }

    public void PlayOnce(int Id)
    {
        if (m_ClipsMap.ContainsKey(Id) == false) return;
        PlayOnce(Id, m_ClipsMap[Id].volumeScale);
    }

    public void Stop()
    {
        m_Audio.Stop();
    }
}