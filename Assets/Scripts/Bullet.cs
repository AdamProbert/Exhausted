using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseProjectile
{
    [SerializeField] ParticleSystem bulletDecalMetal;

    private void OnCollisionEnter(Collision other) 
    {
        base.HandleCollision(other);
        ContactPoint hit = other.contacts[0];
        GameObject bulletDecal = Instantiate(bulletDecalMetal, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, hit.normal)).gameObject;
        bulletDecal.transform.SetParent(other.transform);
        Destroy(bulletDecal, 10f);
    }
}
