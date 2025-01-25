using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{  
   [System.Serializable]
   public class Sound
   {
    public string name;
    public AudioClip clip;
    public float volume = 1.0f; // 預設音量為 1.0
   }
   public static AudioManager Instance;
   public Sound[] musicSounds,sfxSounds;
   public AudioSource musicSource,sfxSource;
   private void Awake()
   {
      if(Instance==null)
      {
         if(Instance==null)
         {
            Instance = this;
            DontDestroyOnLoad(gameObject);
         }
         else
         {
            Destroy(gameObject);
         }
      }
   }
   private void Start()
   {  
      // 將 BOSS_music 的音量設為 30%
      Sound bossMusic = Array.Find(musicSounds, x => x.name == "BOSS_music");
      if (bossMusic != null)
      {
         bossMusic.volume = 0.3f;
      }
      PlayMusic("BOSS_music");
   }
   public void PlayMusic(string name)
   {
      Sound s =Array.Find(musicSounds, x=> x.name == name);
      if (s==null)
      {
         Debug.Log("Sound Not Found");
      }
      else 
      {
         musicSource.clip= s.clip;
         musicSource.Play();
      }
      
   }
   public void PlaySFX(string name)
   {
      Sound s = Array.Find(sfxSounds,x=> x.name == name);
      if (s==null)
      {
         Debug.Log("Sound Not Found");
      }
      else
      {
         sfxSource.PlayOneShot(s.clip);

      }
   }
   public void axe_swing()
   {
      PlaySFX("axe_swing");
   }
   public void ToggleMusic()
   {
      musicSource.mute =!musicSource.mute;
   }
   public void ToggleSFX()
   {
      sfxSource.mute =!sfxSource.mute;
   }
   public void MusicVolume(float volume)
   {
      musicSource.volume=volume;
   }
   public void SFXVolume(float volume)
   {
      sfxSource.volume=volume;
   }
}
