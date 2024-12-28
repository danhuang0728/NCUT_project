using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioMixer mymixer;
    [SerializeField] private Slider musicSlider;

public void  SetMusicVolume()
{
 float volume = musicSlider.value;
 mymixer.SetFloat("music",volume);
}
    // Update is called once per frame

}
