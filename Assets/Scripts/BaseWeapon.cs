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

    protected ObjectPooler projectilePool;

    protected void Init() 
    {
        projectilePool = GetComponent<ObjectPooler>();    
    }

    protected Vector3 GetParentVelocity()
    {
        return parentRigidBody.velocity;
    }

    public abstract void Fire();
    public abstract void StartFiring();
    public abstract void StopFiring();

}
