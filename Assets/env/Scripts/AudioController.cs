using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
    public void ToggleMusic()
    {
       AudioManager.Instance.ToggleMusic(); 
    }
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }
    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume((_musicSlider.value)/10);
    }
    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume((_sfxSlider.value)/10);
    }
    void Start()
    {
        // 初始音量值 (預設為 50%)
        float defaultVolume = 0.5f; 

        // 加載保存的音量值，若無則使用預設值
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultVolume);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", defaultVolume);

        // 設定 Slider 的初始值
        _musicSlider.value = savedMusicVolume*10;
        _sfxSlider.value = savedSFXVolume*10;

        // 同時將音量應用到 AudioManager
        AudioManager.Instance.MusicVolume(savedMusicVolume);
        AudioManager.Instance.SFXVolume(savedSFXVolume);
    }
}
