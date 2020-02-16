using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ObjectPooler))]
abstract public class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected Transform projectileSpawn;
    [Tooltip ("Bullets spawned per second")]
    [SerializeField] protected float fireRate;
    [SerializeField] protected Rigidbody parentRigidBody; // Usually the car

    [SerializeField] Sprite uiImage;

    protected ObjectPooler projectilePool;

    protected AudioSource audioSource;
    [SerializeField] protected AudioClip fireSound;

    protected void Init() 
    {
        projectilePool = GetComponent<ObjectPooler>();    
        audioSource = GetComponent<AudioSource>();
    }

    protected Vector3 GetParentVelocity()
    {
        return parentRigidBody.velocity;
    }

    public abstract void Fire();
    public abstract void StartFiring();
    public abstract void StopFiring();

    public Sprite GetUIImage()
    {
        return uiImage;
    }

}
