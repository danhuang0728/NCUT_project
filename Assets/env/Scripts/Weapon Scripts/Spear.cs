using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public SpearParticleController spearParticleController1;
    public SpearParticleController spearParticleController2;
    public Transform player_transform;
    public float attack_speed = 1f;
    void Start()
    {
        StartCoroutine(Particle_play());
    }


    void Update()
    {
        transform.position = player_transform.position;
    }
    public IEnumerator Particle_play()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(attack_speed);
            spearParticleController1.SpearParticle_play1();
            spearParticleController2.SpearParticle_play2();
        }
    }
}
