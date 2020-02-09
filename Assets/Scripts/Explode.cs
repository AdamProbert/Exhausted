using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField][Range(0,5000)] public float force;
    [SerializeField][Range(0,20)] public float radius;
    [SerializeField]public ParticleSystem effect;

    public void DOIT()
    {
        Instantiate(effect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider c in colliders)
        {
            Rigidbody rb = c.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }
    }
}
