using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public AudioClip audioClip;
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void attackSoundPlay(){
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioClip, 1.0f);
        }
    }
}
