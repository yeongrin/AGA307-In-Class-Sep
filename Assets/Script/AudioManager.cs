using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip[] enemyHitSounds;
    public AudioClip[] enemyDieSounds;
    public AudioClip[] enemyAttackSounds;
    public AudioClip[] footsteps;
    
    public AudioClip GetEnemyHitSound()
    {
        return enemyHitSounds[Random.Range(0, enemyHitSounds.Length)]; //i is used in loop. this is random code.
    }

    public AudioClip GetEnemyDieSound()
    {
        return enemyDieSounds[Random.Range(0, enemyDieSounds.Length)]; //i is used in loop. this is random code.
    }

    public AudioClip GetEnemyAttackSound()
    {
        return enemyAttackSounds[Random.Range(0, enemyAttackSounds.Length)];
    }

    public AudioClip GetFootstepSound()
    {
        return footsteps[Random.Range(0, footsteps.Length)]; //i is used in loop. this is random code.
    }

    /// <summary>
    /// Plays an Audio Clip with adjusted pitch value
    /// </summary>
    /// <param name="_clip">The clip to play</param>
    /// <param name="_source">The audio source to play on</param>
    public void PlaySound(AudioClip _clip, AudioSource _source, float _volume = 1)
    {
        if (_source == null || _clip == null)
            return;

        _source.clip = _clip;
        _source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        _source.volume = _volume;
        _source.Play();
    }
}

