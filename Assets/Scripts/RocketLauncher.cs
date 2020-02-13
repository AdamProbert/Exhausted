using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : BaseWeapon
{
    [SerializeField] int rocketsPerSalvo;
    [SerializeField] float timeBetweenRockets;

    private float nextSalvoFireTime = 0;
    private float nextRocketFireTime = 0;
    private bool firing;
    private int currentSalvoNumber;


     // Start is called before the first frame update
    void Start()
    {
        base.Init();
    }

    private void Update() 
    {
        if(firing)
        {
            if(currentSalvoNumber < rocketsPerSalvo)
            {
                if(Time.time > nextRocketFireTime)
                {
                    Fire();
                    nextRocketFireTime = Time.time + timeBetweenRockets;
                    currentSalvoNumber++;
                }
            }
            else
            {
                firing = false;
                currentSalvoNumber = 0;
                nextSalvoFireTime = Time.time + (1 / base.fireRate);
            }
        }    
    }

    public override void StartFiring()
    {
        if(Time.time < nextSalvoFireTime || firing)
        {
            return;
        }

        nextRocketFireTime = Time.time;
        firing = true;
    }

    public override void StopFiring()
    {
        // Used for automatic weapons
        return;
    }

    public override void Fire()
    {
        GameObject projectile = base.projectilePool.GetPooledObject();
        projectile.SetActive(true);
        projectile.transform.position = base.projectileSpawn.position;
        projectile.transform.rotation = base.projectileSpawn.rotation;
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed + base.GetParentVelocity();
    }
}
