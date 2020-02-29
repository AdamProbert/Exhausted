using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Explode : MonoBehaviour
{
    [SerializeField][Range(0,5000)] public float force;
    [SerializeField][Range(0,20)] public float radius;
    [SerializeField][Range(0,10)] public float upwardMultiplier;
    [SerializeField]public ParticleSystem effect;
    [SerializeField] private AudioClip explosionSound;

    public void DOIT()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider c in colliders)
        {
            Rigidbody rb = c.transform.root.GetComponent<Rigidbody>();
            if(rb != null && c.gameObject.layer != 10)
            {
                Debug.Log("Found rb: " +rb.gameObject.name);
                rb.AddExplosionForce(force, transform.position, radius, 2f);
            }
        }
        SendImpulse();
    }

    public void SendImpulse()
    {
        if(GetComponent<CinemachineImpulseSource>())
        {
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        }
    }
}
