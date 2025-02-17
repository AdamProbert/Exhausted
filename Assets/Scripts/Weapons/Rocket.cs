﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explode))]
public class Rocket : BaseProjectile
{
    Explode explode;
    [SerializeField] private float flightTimeBeforeWiggle;
    [SerializeField] private bool wiggle;
    [SerializeField][Range(0, 10)] private float wiggleAmount;

    private float startTime;

    // Start is called before the first frame update
    private void Awake() 
    {
        explode = GetComponent<Explode>();
        base.Init();
    }

    private void OnEnable() 
    {
        // rb.AddForce(Vector3.up * Random.Range(3, 8), ForceMode.Impulse);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(wiggle && (startTime + flightTimeBeforeWiggle) < Time.time && base.rb)
        {
            // Make it wiggle
            base.rb.AddForce(Vector3.up * Random.Range(-wiggleAmount, wiggleAmount), ForceMode.Impulse);   
            base.rb.AddForce(Vector3.right * Random.Range(-wiggleAmount, wiggleAmount), ForceMode.Impulse);   
        }

        base.FaceForward();
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.transform.root != base.playerShooter)
        {
            explode.DOIT();
            base.HandleCollision(other);    
        }
    }
}
