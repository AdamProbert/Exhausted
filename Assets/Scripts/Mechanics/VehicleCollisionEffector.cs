using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class VehicleCollisionEffector : MonoBehaviour
{
    [SerializeField] ParticleSystem fxPrefab;
    [SerializeField] float minRigidBodyVelocity;
    [SerializeField] LayerMask collisionLayers;
    ParticleSystem fx;
    AudioSource audioSource;
    [SerializeField] AudioClip scrapeSound;
    Rigidbody rb;
    [SerializeField] float minVelocityExplosion;

    [Header("States")]
    [SerializeField] bool scraping = false;
    public bool debug = true;

    private void Start() 
    {
        rb = transform.root.GetComponent<Rigidbody>();    
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        audioSource.Pause();
    }

    public void PlayerDied()
    {
        audioSource.Stop();
    }

    private void Update() 
    {
        if(fx != null)
        {
            if(rb.velocity.magnitude > minRigidBodyVelocity)
            {
                fx.transform.position = transform.position;
                fx.transform.rotation = Quaternion.LookRotation(-rb.velocity);
                if(debug)
                {
                    Debug.DrawRay(transform.position, fx.transform.forward, Color.red);
                }
            }
            else
            {
                fx.Stop();
                audioSource.Pause();
                Destroy(fx.gameObject, 2f);
                fx = null; 
            }
            
        }    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            if(fx == null)
            {
                fx = Instantiate(fxPrefab, transform.position, Quaternion.LookRotation(-rb.velocity));
            }
            audioSource.Play();    
        }    
    }

    private void OnTriggerExit(Collider other) 
    {
        if(fx != null)
        {
            fx.Stop();
            Destroy(fx.gameObject, 2f);
            fx = null;
        }
        audioSource.Pause();
    }
}
