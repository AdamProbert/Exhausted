using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VehicleCollisionEffector : MonoBehaviour
{
    [SerializeField] ParticleSystem fxPrefab;
    [SerializeField] float minRigidBodyVelocity;
    [SerializeField] LayerMask collisionLayers;
    ParticleSystem fx;

    Rigidbody rb;

    [Header("States")]
    [SerializeField] bool scraping = false;
    public bool debug = true;

    private void Start() 
    {
        rb = transform.root.GetComponent<Rigidbody>();    
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
                Destroy(fx.gameObject, 2f);
                fx = null; 
            }
            
        }    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(fx == null)
        {
            if(collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
            {
                fx = Instantiate(fxPrefab, transform.position, Quaternion.LookRotation(-rb.velocity));
            }
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
    }
}
