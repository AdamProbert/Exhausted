using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : BaseProjectile
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private LayerMask stickableSurfaces;

    private float startTime;

    bool active = false;
    

    private void OnEnable()
    {
        startTime = Time.time;
        base.rb.isKinematic = false;
        GetComponentInChildren<Collider>().enabled = true;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(active)
        {
            if(other.gameObject && other.gameObject.layer == 8 && other.transform.root != this.transform.root)
            {   
                other.gameObject.GetComponent<Player>().TakeDamage(base.damage);
            }

            if(other.transform.root != this.transform.root)
            {

                if((stickableSurfaces & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
                {
                    // Stick it in the thing
                    Vector3 dir = other.contacts[0].point - transform.position;
                    transform.position += dir * .5f;

                    // Parent us to the collided object
                    transform.parent = other.transform;
                    base.rb.isKinematic = true;
                    GetComponentInChildren<Collider>().enabled = false;
                }

                // Dat sweet audio
                base.audioSource.clip = base.collisionSound;
                base.audioSource.pitch = Random.Range(0.6f, 0.9f);
                base.audioSource.Play();

                active = false;
                StartCoroutine (disableMe());
            }
        }
    }

    IEnumerator disableMe()
    {
        yield return new WaitForSeconds(30);
        gameObject.SetActive(false);
    }
}
