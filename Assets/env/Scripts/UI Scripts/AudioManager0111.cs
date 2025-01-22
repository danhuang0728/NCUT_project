using UnityEngine;

public class AudioManager0111 : MonoBehaviour
{
    public static AudioManager0111 Instance;

    public AudioSource musicSource; // 控制音樂的 AudioSource
    public AudioSource sfxSource;   // 控制音效的 AudioSource

    private void Awake()
    {
        // 單例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 場景切換時保持不銷毀
        }
        else
        {
            Destroy(gameObject); // 如果已有一個實例，銷毀多餘的
        }
    }

    // 設置音樂音量
    public void MusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }

    // 設置音效音量
    public void SFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
    }

    // 開關音樂靜音
    public void ToggleMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = !musicSource.mute;
        }
    }

    // 開關音效靜音
    public void ToggleSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.mute = !sfxSource.mute;
        }
    }
}
