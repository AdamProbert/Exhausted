using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explode))]
public class BoomStick : BaseProjectile
{
    [SerializeField] float fuseTime;
    [SerializeField] LayerMask stickableSurfaces;
    private float hitTime;
    private bool flying;

    private void OnEnable() 
    {
        flying = true;
    }

    private void OnDisable() 
    {
        flying = false;
    }

    private IEnumerator ExplodeOnTimer()
    {
        yield return new WaitForSeconds(fuseTime);    
        GetComponent<Explode>().DOIT();
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(flying)
        {
            if(other.gameObject && other.gameObject.layer == 8 && other.transform.root != base.playerShooter)
            {   
                other.gameObject.GetComponent<Player>().TakeDamage(base.damage);
            }

            // Stick it in the thing
            if((stickableSurfaces & 1 << other.gameObject.layer) == 1 << other.gameObject.layer && other.transform.root != base.playerShooter)
            {
                Debug.Log("Sticking to: " + other.gameObject.name);
                transform.position += transform.forward * .3f;
                
                transform.parent = other.transform;
                base.rb.isKinematic = true;
                GetComponent<Collider>().enabled = false;
                flying = false;

                StartCoroutine(ExplodeOnTimer());
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("trigger called");

        if(
            (stickableSurfaces & 1 << other.gameObject.layer) == 1 << other.gameObject.layer
            && flying
            && other.transform.root != base.playerShooter 
        )
        {
            Debug.Log("Should be playing audio");
            // Dat sweet audio
            base.audioSource.clip = base.collisionSound;
            base.audioSource.pitch = Random.Range(0.6f, 0.9f);
            base.audioSource.Play();
        }
    }
}
