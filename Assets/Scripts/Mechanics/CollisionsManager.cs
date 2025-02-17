﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollisionsManager : MonoBehaviour
{

    [SerializeField] float maxSpeed = 100;
    private AudioSource audioSource;
    private int requestScrapeSoundCount;
    private bool started;
    private float volumeModifier;
    private Rigidbody parentRB;

    void Start()
    {
        parentRB = transform.root.GetComponent<Rigidbody>();
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
            audioSource.volume = Mathf.Clamp01(parentRB.velocity.magnitude / maxSpeed);
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
        if(requestScrapeSoundCount > 0)
        {
            requestScrapeSoundCount -= 1;    
            HandleAudioChange();
        }
    }
}
