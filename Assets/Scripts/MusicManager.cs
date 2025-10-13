using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    #region Properties
    #endregion

    #region Fields
    public static MusicManager Instance { get; private set; }
    [SerializeField] private AudioSource audioSource;
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
            audioSource = GetComponent<AudioSource>();

            bool isMusicOff = PlayerPrefs.GetInt("MusicOff", 0) == 1;
            audioSource.mute = isMusicOff;
        }
    }

    public void ToggleMusic()
    {
        audioSource.mute = !audioSource.mute;
        PlayerPrefs.SetInt("MusicOff", audioSource.mute ? 1 : 0);
    }

    public bool IsMuted()
    {
        return audioSource.mute;
    }

 
}
