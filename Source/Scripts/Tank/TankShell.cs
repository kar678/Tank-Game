﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogwheel.AI;

namespace Cogwheel.Player
{
    public class TankShell : MonoBehaviour
    {
        public LayerMask tankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
        public ParticleSystem explosionParticles;         // Reference to the particles that will play on explosion.
        public AudioSource explosionAudio;                // Reference to the audio that will play on explosion.
        public float maxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
        public float explosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
        public float maxLifeTime = 2f;                    // The time in seconds before the shell is removed.
        public float explosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.


        private void Start()
        {
            // If it isn't destroyed by then, destroy the shell after it's lifetime.
            Destroy(gameObject, maxLifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, tankMask);

            // Go through all the colliders...
            for (int i = 0; i < colliders.Length; i++)
            {
                // ... and find their rigidbody.
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

                // If they don't have a rigidbody, go on to the next collider.
                if (!targetRigidbody)
                    continue;

                // Add an explosion force.
                targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                // Find the TankHealth script associated with the rigidbody.
                if(targetRigidbody.GetComponent<TankController>())
                {
                    TankController targetHealth = targetRigidbody.GetComponent<TankController>();

                    // Calculate the amount of damage the target should take based on it's distance from the shell.
                    float damage = CalculateDamage(targetRigidbody.position);
                    // Deal this damage to the tank.
                    targetHealth.HandleTakeDamage(damage);
                }
                else if(targetRigidbody.GetComponent<AITankController>())
                {
                    AITankController targetAI = targetRigidbody.GetComponent<AITankController>();

                    // Calculate the amount of damage the target should take based on it's distance from the shell.
                    float damage = CalculateDamage(targetRigidbody.position);
                    // Deal this damage to the tank.
                    targetAI.HandleTakeDamage(damage);
                }
                else
                {
                    // If there is no TankHealth script attached to the gameobject, go on to the next collider.
                    continue;
                }
            }

            if(explosionParticles)
            {
                // Unparent the particles from the shell.
                explosionParticles.transform.parent = null;

                // Play the particle system.
                explosionParticles.Play();
            }

            if(explosionAudio)
            {
                // Play the explosion sound effect.
                explosionAudio.Play();
            }

            if(explosionParticles)
            {
                // Once the particles have finished, destroy the gameobject they are on.
                ParticleSystem.MainModule mainModule = explosionParticles.main;
                Destroy(explosionParticles.gameObject, mainModule.duration);
            }


            // Destroy the shell.
            Destroy(gameObject);
        }

        private float CalculateDamage(Vector3 targetPosition)
        {
            // Create a vector from the shell to the target.
            Vector3 explosionToTarget = targetPosition - transform.position;

            // Calculate the distance from the shell to the target.
            float explosionDistance = explosionToTarget.magnitude;

            // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
            float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

            // Calculate damage as this proportion of the maximum possible damage.
            float damage = relativeDistance * maxDamage;

            // Make sure that the minimum damage is always 0.
            damage = Mathf.Max(0f, damage);

            return damage;
        }

    }
}
