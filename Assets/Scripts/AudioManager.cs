using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource sfxAudioSource, musicAudioSource;
    //[SerializeField] private AudioClip[] audioClips;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } 
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySoundEffect(AudioClip audioClip, float volume)
    {
        sfxAudioSource.PlayOneShot(audioClip, volume);
    }

    //public void PlaySoundEffectName(string audioClip, float volume)
    //{
    //    for (int i = 0; i < audioClips.Length; i++)
    //    {
    //        if (audioClips[i].name == audioClip)
    //        {
    //            sfxAudioSource.PlayOneShot(audioClips[i], volume);
    //        }           
    //    }
    //}

}
