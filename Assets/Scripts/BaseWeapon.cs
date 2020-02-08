using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform projectileSpawn;
    [Tooltip ("Bullets spawned per second")]
    [SerializeField] private float fireRate;

    private float nextFireTime = 0;

    public void Fire()
    {
        if(Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + (1 / fireRate);
        Rigidbody projectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation).GetComponent<Rigidbody>();
        projectile.velocity = transform.forward * projectileSpeed;

    }
}
