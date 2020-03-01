using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected AudioClip collisionSound;
    [SerializeField] public Transform playerShooter; // Populated by object pooler
    protected Rigidbody rb;
    protected AudioSource audioSource;
    Quaternion rotation;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void FaceForward()
    {
        // Face forward
        rotation = transform.rotation;
        rotation.SetLookRotation(rb.velocity);
        transform.rotation = rotation;
    }

    protected void HandleCollision(Collision other) 
    {

        if(other.gameObject.layer == 8 && other.transform.root != playerShooter)
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }
}
