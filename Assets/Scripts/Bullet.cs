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
        hiteffect = Instantiate(bulletHitEffectPrefab);    
    }

    private void OnCollisionEnter(Collision other) 
    {
        base.HandleCollision(other);
        hiteffect.Play();
    }
}
