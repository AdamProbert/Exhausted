using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected AudioClip collisionSound;
    // [SerializeField] protected Transform playerShooter;
    protected Rigidbody rb;
    protected AudioSource audioSource;
    Quaternion rotation;

    private void Awake() 
    {
        Debug.Log("BaseProjectile: Awake called");
        Debug.Log("BaseProjectile: init parent: " + transform.parent);
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        // playerShooter = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        // Face forward
        rotation = transform.rotation;
        rotation.SetLookRotation (rb.velocity);
        transform.rotation = rotation;
    }

    protected void HandleCollision(Collision other) 
    {

        if(other.gameObject.layer == 8 && other.transform.root != this.transform.root)
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }
}
