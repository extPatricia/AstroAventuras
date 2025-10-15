using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    public Sprite _soundOn;
    public Sprite _soundOff;
    [SerializeField] private Image _buttonImage;
    // Start is called before the first frame update
    void Start()
    {
        _buttonImage = GetComponent<Image>();
        UpdateIcon();
    }

    public void ToggleMusic()
    {
        if(MusicManager.Instance != null)
        {
            MusicManager.Instance.ToggleMusic();
            UpdateIcon();
        }
    }

    private void UpdateIcon()
    {
        if(MusicManager.Instance != null && _buttonImage != null)
        {
            if (MusicManager.Instance.IsMuted())
            {
                _buttonImage.sprite = _soundOff;
            }
            else
            {
                _buttonImage.sprite = _soundOn;
            }
        }
    }
}
