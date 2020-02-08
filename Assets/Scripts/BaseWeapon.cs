using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ObjectPooler))]
public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform projectileSpawn;
    [Tooltip ("Bullets spawned per second")]
    [SerializeField] private float fireRate;

    private float nextFireTime = 0;
    private bool firing = false;

    private ObjectPooler projectilePool;

    private void Start() 
    {
        projectilePool = GetComponent<ObjectPooler>();    
    }

    private void Update() 
    {
        if(firing)
        {
            Fire();
        }
    }

    public void Fire()
    {
        if(Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + (1 / fireRate);

        GameObject projectile = projectilePool.GetPooledObject();
        projectile.SetActive(true);
        projectile.transform.position = projectileSpawn.position;
        projectile.transform.rotation = projectileSpawn.rotation;
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
    }

    public void StartFiring()
    {
        firing = true;
    }

    public void StopFiring()
    {
        firing = false;
    }

}
