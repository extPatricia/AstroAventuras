using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    #region Properties
    #endregion

    #region Fields
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _backgroundMusicClip;
    [SerializeField] private AudioClip _menuMusicClip;
    [SerializeField] private AudioClip _finalMusicClip;

    private string _currentSceneName;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); //persiste entre escenas
            _audioSource = GetComponent<AudioSource>();

            bool isMusicOff = PlayerPrefs.GetInt("MusicOff", 0) == 1;
            _audioSource.mute = isMusicOff;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _currentSceneName = scene.name;

        if (_currentSceneName == "Menu")
        {
            PlayMusic(_menuMusicClip, 1f, true);
        }
        else if (_currentSceneName == "Final")
        {
            PlayMusic(_finalMusicClip, 0.6f, false);
        }
        else
        {
            PlayMusic(_backgroundMusicClip, 0.3f, true);
        }
    }

    private void PlayMusic(AudioClip clip, float volume, bool loop)
    {
        if (clip == null)
            return;

        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.loop = loop;
        _audioSource.Play();
    }

    public void ToggleMusic()
    {
        _audioSource.mute = !_audioSource.mute;
        PlayerPrefs.SetInt("MusicOff", _audioSource.mute ? 1 : 0);
    }

    public bool IsMuted()
    {
        return _audioSource.mute;
    }

 
}
