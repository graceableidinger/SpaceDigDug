using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerDigdug : MonoBehaviour
{
    public AudioSource source;
    public AudioSource music;
    public AudioClip laser;
    public AudioClip fireball;
    public AudioClip pDeath;
    public AudioClip eDeath;
    public AudioClip powerup;
    public AudioClip rock;
    public AudioClip saber;
    public AudioClip victory;
    public AudioClip pause;
    public AudioClip mining;
    private static AudioManagerDigdug instance;
    public static AudioManagerDigdug Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManagerDigdug>();
                if (instance == null)
                {
                    GameObject test = new GameObject();
                    test.hideFlags = HideFlags.HideAndDontSave;
                    instance = test.AddComponent<AudioManagerDigdug>();
                }
            }
            return instance;
        }
    }
    public void PlayLaserSound()
    {
        source.clip = laser;
        source.Play();
    }
    public void PlayFireballSound()
    {
        source.clip = fireball;
        source.Play();
    }
    public void PlayPlayerDeathSound()
    {
        source.clip = pDeath;
        source.Play();
    }
    public void PlayEnemyDeathSound()
    {
        source.clip = eDeath;
        source.Play();
    }
    public void PlayPowerupSound()
    {
        source.clip = powerup;
        source.Play();
    }
    public void PlayRockSound()
    {
        source.clip = rock;
        source.Play();
    }
    public void PlaySaberSound()
    {
        source.clip = saber;
        source.Play();
    }
    public void PlayPauseSound()
    {
        source.clip = pause;
        source.Play();
        music.mute = !music.mute;
        if (!music.mute)
        {
            source.Stop();
        }
    }
    public void PlayVictorySound()
    {
        source.clip = victory;
        source.Play();
    }
    public void PlayMiningSound()
    {
        source.clip = mining;
        source.Play();
    }


}
