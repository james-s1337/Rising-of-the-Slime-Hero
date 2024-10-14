using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playlist : MonoBehaviour
{
    [SerializeField] private AudioSource waveMusic;
    [SerializeField] private AudioSource bossMusic;

    [SerializeField] private AudioSource victoryMusic;
    [SerializeField] private AudioSource deathMusic;

    private void Start()
    {
        waveMusic.loop = true;
        bossMusic.loop = true;

        waveMusic.Play();
    }

    public void PlayBossMusic()
    {
        waveMusic.Stop();
        bossMusic.Play();
    }

    public void PlayVictoryMusic()
    {
        bossMusic.Stop();
        victoryMusic.time = 2f;
        victoryMusic.Play();
    }

    public void PlayDeathMusic()
    {
        waveMusic.Stop();
        bossMusic.Stop();
        deathMusic.Play();
    }
}
