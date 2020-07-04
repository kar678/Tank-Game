using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankExplosion : MonoBehaviour
{
    public ParticleSystem explosionParticles;         // Reference to the particles that will play on explosion.
    public AudioSource explosionAudio;                // Reference to the audio that will play on explosion.
    public float maxLifeTime = 2f;                    // The time in seconds before the explosion is removed.

    // Start is called before the first frame update
    void Start()
    {
        // If it isn't destroyed by then, destroy the explosion after it's lifetime.
        Destroy(gameObject, maxLifeTime);

        if (explosionParticles)
        {
            // Unparent the particles from the shell.
            explosionParticles.transform.parent = null;

            // Play the particle system.
            explosionParticles.Play();
        }

        if (explosionAudio)
        {
            // Play the explosion sound effect.
            explosionAudio.Play();
        }

        if (explosionParticles)
        {
            // Once the particles have finished, destroy the gameobject they are on.
            ParticleSystem.MainModule mainModule = explosionParticles.main;
            Destroy(explosionParticles.gameObject, mainModule.duration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
