using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManager;

    [SerializeField] AudioSource SoundAudioSource;
    [SerializeField] AudioSource BGMAudioSource;
    
    [SerializeField] AudioDictionary[] SoundAudios;
    [SerializeField] AudioDictionary[] BGMAudios;

    public void PlayAudio (string audioType, string audioName)
    {
        if (audioType == "background")
        {
            SoundAudioSource.clip = GetAudioFromAudioName(SoundAudios, audioName);
            SoundAudioSource.Play();
        }
        else if (audioType == "sound")
        {
            SoundAudioSource.PlayOneShot(GetAudioFromAudioName(SoundAudios, audioName));
        }
        else if (audioType == "music")
        {
            BGMAudioSource.clip = GetAudioFromAudioName(BGMAudios, audioName);
            BGMAudioSource.Play();
        }
    }

    public void StopAudio(string audioType)
    {
        if (audioType == "background")
        {
            SoundAudioSource.Stop();
        }
        else if (audioType == "music")
        {
            BGMAudioSource.Stop();
        }
    }

    AudioClip GetAudioFromAudioName(AudioDictionary[] audios, string audioName)
    {
        foreach (AudioDictionary audio in audios)
        {
            if (audio.name == audioName)
            {
                return audio.audio;
            }
        }
        return null;
    }
    
    [Serializable]
    class AudioDictionary
    {
        public string name;
        public AudioClip audio;
    }
}
