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
      public float volume = 1.0f;
   }

   public static AudioManager Instance;
   public Sound[] musicSounds, sfxSounds;
   public AudioSource musicSource, sfxSource;

   private int currentBattleMusicIndex = 0;
   public string[] battleMusicOrder = { "BOSS_music", "Battle_music2", "Battle_music3" }; // 在Inspector中设置战斗音乐顺序
   public string restMusic = "Rest_music"; // 在Inspector中设置休息音乐名称

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   private void Start()
   {  
      //PlayMusic("BOSS_music");
   }

   public void PlayMusic(string name, bool shouldLoop = true)
   {
      Sound s = Array.Find(musicSounds, x => x.name == name);
      if (s == null)
      {
         // Debug.Log("Sound Not Found");
      }
      else 
      {
         musicSource.clip = s.clip;
         musicSource.volume = s.volume;
         musicSource.loop = shouldLoop;
         musicSource.Play();
      }
   }

   public void PlaySFX(string name)
   {
      Sound s = Array.Find(sfxSounds, x => x.name == name);
      if (s == null)
      {
         // Debug.Log("Sound Not Found");
      }
      else
      {
         sfxSource.PlayOneShot(s.clip, s.volume);
      }
   }

   public void axe_swing()
   {
      PlaySFX("axe_swing");
   }

   public void ToggleMusic()
   {
      musicSource.mute = !musicSource.mute;
   }

   public void ToggleSFX()
   {
      sfxSource.mute = !sfxSource.mute;
   }

   public void MusicVolume(float volume)
   {
      musicSource.volume = volume;
   }

   public void SFXVolume(float volume)
   {
      sfxSource.volume = volume;
   }

   public void PlayNextBattleMusic(string name)
   {   
      PlayMusic(name, true);
   }

   public void PlayRestMusic()
   {
      PlayMusic(restMusic, true);
   }
}
