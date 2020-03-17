using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected AudioClip collisionSound;
    [SerializeField] LayerMask collideableLayers;
    [SerializeField] public Transform playerShooter; // Populated by object pooler
    protected Rigidbody rb;
    protected AudioSource audioSource;
    Quaternion rotation;
    

    public void Init() 
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void FaceForward()
    {
        // Face forward
        if(rb != null)
        {
            rotation = transform.rotation;
            rotation.SetLookRotation(rb.velocity);
            transform.rotation = rotation;
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    protected void HandleCollision(Collision other) 
    {

        if(collideableLayers == (collideableLayers | (1 << other.gameObject.layer)) && other.transform.root != playerShooter)
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }
}
