using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public AudioSource AudioSource { get { return m_Audio; } }
    public Dictionary<int, AudioClip> AudioClips { get { return m_ClipsMap; } }

    private AudioSource m_Audio;
    private Dictionary<int, AudioClip> m_ClipsMap = new Dictionary<int, AudioClip>();

    private void Awake()
    {
        m_Audio = this.GetComponent<AudioSource>();
    }

    public void SetAudioClips(List<AudioClip> clips)
    {
        for (int i = 0; i < clips.Count; i++)
        {
            SetAudioClip(i, clips[i]);
        }
    }

    public void SetAudioClip(int Id, AudioClip clip)
    {
        if (m_ClipsMap.ContainsKey(Id))
        {
            m_ClipsMap[Id] = clip;
        }
        else
        {
            m_ClipsMap.Add(Id, clip);
        }
    }

    public void Play(int Id, float volumeScale = 1)
    {
        if (m_ClipsMap.ContainsKey(Id) == false) return;
        m_Audio.PlayOneShot(m_ClipsMap[Id], volumeScale);
    }

    public void Stop()
    {
        m_Audio.Stop();
    }
}