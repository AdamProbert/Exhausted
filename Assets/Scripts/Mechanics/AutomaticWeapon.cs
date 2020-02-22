using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : BaseWeapon
{

    private float nextFireTime = 0;
    private bool firing = false;

    private void Update() 
    {
        if(firing)
        {
            Fire();
        }
    }

    public override void Fire()
    {
        if(Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + (1 / base.fireRate);

        GameObject projectile = base.projectilePool.GetPooledObject();
        projectile.SetActive(true);
        projectile.transform.position = base.projectileSpawn.position;
        projectile.transform.rotation = base.projectileSpawn.rotation;
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
    }
    public override void StartFiring()
    {
        firing = true;
    }

    public override void StopFiring()
    {
        firing = false;
    }
}
