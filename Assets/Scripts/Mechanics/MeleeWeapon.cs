using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MeleeWeapon : BaseDamager
{
    [Header("Impact")]
    [SerializeField] float minVelocityForImpact;
    [SerializeField] float explosivePower;
    [SerializeField] LayerMask canDamage;

    [Header("Effects")]
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] AudioClip hitSound;

    private AudioSource audioSource;
    private Rigidbody parentRB;


    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        parentRB = transform.root.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision enter called");
        if(parentRB.velocity.magnitude > minVelocityForImpact & other.transform.root != transform.root) // If going fast enough and not us..
        {
            if(canDamage == (canDamage | (1 << other.gameObject.layer))) // And in colidable layer
            {
                other.gameObject.GetComponentInParent<Player>().TakeDamage(base.GetDamage());
                hitEffect.Play();
                audioSource.PlayOneShot(hitSound, .3f);

                // Add impact force
                Vector3 dir = other.transform.position - transform.position;
                dir = dir.normalized;
                other.gameObject.GetComponentInParent<Rigidbody>().AddForce(dir*explosivePower*parentRB.velocity.magnitude, ForceMode.Impulse);
            }
        }
    }
}
