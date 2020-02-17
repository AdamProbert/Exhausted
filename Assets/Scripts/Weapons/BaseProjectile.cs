using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseProjectile : MonoBehaviour
{
    [SerializeField] ParticleSystem bulletDecalMetal;
    Rigidbody rb;
    Quaternion rotation;

        // Start is called before the first frame update
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        // Face forward
        rotation = transform.rotation;
        rotation.SetLookRotation (rb.velocity);
        transform.rotation = rotation;
    }


    private void OnCollisionEnter(Collision other) 
    {
        gameObject.SetActive(false);
        ContactPoint hit = other.contacts[0];
        GameObject bulletDecal = Instantiate(bulletDecalMetal, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, hit.normal)).gameObject;
        bulletDecal.transform.SetParent(other.transform);
        Destroy(bulletDecal, 10f);
    }
}
