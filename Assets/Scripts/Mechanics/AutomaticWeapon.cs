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
        if(base.currentStatus == BaseWeapon.status.Active)
        {
            if(Time.time < nextFireTime)
            {
                return;
            }

            nextFireTime = Time.time + (1 / base.fireRate);

            audioSource.PlayOneShot(base.fireSound, 1f);

            if(fireEffect)
            {
                fireEffect.Play();
            }
            
            base.FireProjectile();
        }
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
