using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explode))]
public class BoomStick : BaseProjectile
{
    [SerializeField] float fuseTime;
    [SerializeField] LayerMask stickableSurfaces;
    private bool flying;
    private Transform previousParent;

    private void Awake() 
    {
        base.Init();    
    }
    
    private void OnEnable() 
    {
        flying = true;
        previousParent = transform.parent;
        GetComponent<Collider>().enabled = true;
    }

    private void OnDisable() 
    {
        flying = false;
    }

    private void Update() 
    {
        if(flying)
        {
            base.FaceForward();
        }
        
    }

    private IEnumerator ExplodeOnTimer()
    {
        yield return new WaitForSeconds(fuseTime);    
        GetComponent<Explode>().DOIT();
        transform.parent = previousParent;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(flying)
        {
            if(base.collideableLayers == (base.collideableLayers | (1 << other.gameObject.layer)) && other.transform.root != playerShooter)
            {
                other.gameObject.GetComponentInParent<Player>().TakeDamage(base.damage);
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
                // Player can be dead on first hit, so have to check dis
                if(this.isActiveAndEnabled)
                {
                    StartCoroutine(ExplodeOnTimer());    
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject && other.gameObject.layer == 8 && other.transform.root != base.playerShooter && flying)
        {
            // Dat sweet audio
            base.audioSource.clip = base.collisionSound;
            base.audioSource.pitch = Random.Range(0.6f, 0.9f);
            base.audioSource.Play();
        }
    }
}
