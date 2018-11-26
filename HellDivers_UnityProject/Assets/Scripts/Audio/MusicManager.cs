using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMusicSelection
{
    Theme, BattleField, MissionSuccess, MissionFailed
}

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public eMusicSelection CurrentSelection { get { return m_CurrentSelection; } }

    private AudioSource m_AudioSource;
    private eMusicSelection m_CurrentSelection;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        m_AudioSource = this.GetComponent<AudioSource>();
        m_AudioSource.loop = true;
    }

    private void Start()
    {
        PlayMusic(eMusicSelection.Theme, 2);
    }

    public void PlayMusic(eMusicSelection music, float time = 0)
    {
        BGMSetting bgm = Resources.Load<BGMSetting>("BGMSetting");
        if (bgm == null) return;
        m_AudioSource.Stop();
        switch (music)
        {
            case eMusicSelection.Theme:
                m_CurrentSelection = eMusicSelection.Theme;
                m_AudioSource.clip = bgm.Theme;
                break;

            case eMusicSelection.BattleField:
                m_CurrentSelection = eMusicSelection.BattleField;
                int index = Random.Range(0, bgm.BattleField.Count);
                m_AudioSource.clip = bgm.BattleField[index];
                break;

            case eMusicSelection.MissionSuccess:
                m_CurrentSelection = eMusicSelection.MissionSuccess;
                m_AudioSource.clip = bgm.MissionSuccess;
                break;

            case eMusicSelection.MissionFailed:
                m_CurrentSelection = eMusicSelection.MissionFailed;
                m_AudioSource.clip = bgm.MissionFailed;
                break;
        }

        m_AudioSource.Play();
        if (time > 0) StartCoroutine(AudioFadeIn(time));
    }

    public void FadeOut(float time = 1)
    {
        StartCoroutine(AudioFadeOut(time));
    }

    private IEnumerator AudioFadeOut(float time)
    {
        while (m_AudioSource.volume > 0)
        {
            m_AudioSource.volume -= Time.deltaTime / time;
            if (m_AudioSource.volume < 0)
            {
                m_AudioSource.volume = 0;
                m_AudioSource.Stop();
            }
            yield return null;
        }
    }

    private IEnumerator AudioFadeIn(float time)
    {
        m_AudioSource.volume = 0;
        while (m_AudioSource.volume < 1)
        {
            m_AudioSource.volume += Time.deltaTime / time;
            if (m_AudioSource.volume > 1) m_AudioSource.volume = 1;
            yield return null;
        }
    }
}