using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollisionsManager : MonoBehaviour
{

    private AudioSource audioSource;
    private int requestScrapeSoundCount;

    private bool started;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
        requestScrapeSoundCount = 0;
        audioSource.Play();
        audioSource.Pause();
        started = true;
    }

    // Message
    public void PlayerDied()
    {
        audioSource.Stop();
        started = false;
    }

    private void HandleAudioChange() 
    {   
        if(started)
        {
            if(requestScrapeSoundCount > 0 & !audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if(requestScrapeSoundCount <= 0 & audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    public void RequestPlayAudio()
    {
        requestScrapeSoundCount += 1;
        HandleAudioChange();
    }

    public void RequestStopAudio()
    {
        requestScrapeSoundCount -= 1;
        HandleAudioChange();
    }
}
