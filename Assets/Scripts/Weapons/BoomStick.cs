using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explode))]
public class BoomStick : BaseProjectile
{
    [SerializeField] float fuseTime;
    [SerializeField] LayerMask stickableSurfaces;
    [SerializeField] float maxFlightTime = 5f;
    private float startTime;
    private bool flying;
    private Transform ogParent;

    IEnumerator disableSelfCoroutine;

    private void Awake() 
    {
        base.Init();
        ogParent = transform.parent;    
        disableSelfCoroutine = DisableSelfOnTimer();
    }
    
    private void OnEnable() 
    {
        flying = true;
        transform.parent = ogParent;
        base.rb.isKinematic = false;
        GetComponent<Collider>().enabled = true;
        startTime = Time.time;
    }

    private void OnDisable() 
    {
        flying = false;
        startTime = Mathf.Infinity;
    }

    private void Update() 
    {
        if(flying)
        {
            base.FaceForward();

            // Check max flight time
            if(startTime + maxFlightTime < Time.deltaTime)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator ExplodeOnTimer()
    {
        yield return new WaitForSeconds(fuseTime);    
        GetComponent<Explode>().DOIT();
        gameObject.SetActive(false);
    }

    private IEnumerator DisableSelfOnTimer()
    {
        yield return new WaitForSeconds(1f);    
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
                    StopCoroutine(disableSelfCoroutine);
                    StartCoroutine(ExplodeOnTimer());    
                }
            }
            else
            {
                StartCoroutine(disableSelfCoroutine);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(base.collideableLayers == (base.collideableLayers | (1 << other.gameObject.layer)) && other.transform.root != base.playerShooter && flying)
        {
            // Dat sweet audio
            base.audioSource.clip = base.collisionSound;
            base.audioSource.pitch = Random.Range(0.6f, 0.9f);
            base.audioSource.Play();
        }
    }
}
