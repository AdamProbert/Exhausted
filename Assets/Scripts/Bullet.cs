using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseProjectile
{
    [SerializeField] ParticleSystem bulletHitEffectPrefab;
    private ParticleSystem hiteffect;

    private void Awake() 
    {
        base.Init();
        hiteffect = Instantiate(bulletHitEffectPrefab, transform.position, Quaternion.identity, transform.parent);    
    }

    private void OnCollisionEnter(Collision other) 
    {
        base.HandleCollision(other);
        hiteffect.Play();
    }
}
