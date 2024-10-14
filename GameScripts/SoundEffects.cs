using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource healSound;
    [SerializeField] private AudioSource blockSound;

    public void PlayDeathSound()
    {
        deathSound.pitch = 2f;
        deathSound.Play();
    }

    public void PlayHurtSound()
    {
        hurtSound.time = 0.18f;
        hurtSound.pitch = 1f;
        hurtSound.Play();
    }

    public void PlayHealSound()
    {
        healSound.Stop();
        healSound.pitch = 1f;
        healSound.Play();
    }

    public void PlayBoostSound()
    {
        healSound.Stop();
        healSound.pitch = 0.5f;
        healSound.Play();
    }

    public void PlayBlockSound()
    {
        hurtSound.time = 0.18f;
        hurtSound.pitch = 2f;
        hurtSound.Play();
    }
}
