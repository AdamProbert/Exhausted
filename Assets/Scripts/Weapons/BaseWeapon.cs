using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ObjectPooler))]
abstract public class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected float projectileSpeed;
    [SerializeField] public Transform projectileSpawn;
    [Tooltip ("Bullets spawned per second")]
    [SerializeField] protected float fireRate;
    [SerializeField] protected Rigidbody parentRigidBody;

    [SerializeField] Sprite uiImage;

    protected ObjectPooler projectilePool;

    public status currentStatus = status.Inactive;
    public enum status
    {
        Active,
        Inactive
    }

    [Header("Audio")]
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip fireSound;

    [SerializeField] private ParticleSystem fireEffectPrefab;
    protected ParticleSystem fireEffect;

    protected void Init() 
    {
        projectilePool = GetComponent<ObjectPooler>();    
        audioSource = GetComponent<AudioSource>();
        parentRigidBody = transform.root.GetComponent<Rigidbody>();
        if(fireEffectPrefab)
        {
            fireEffect = Instantiate(fireEffectPrefab, projectileSpawn.transform.position, projectileSpawn.transform.rotation, transform);
        }
    }

    public void PlayerDied()
    {
        currentStatus = status.Inactive;
    }

    public void PlayerActive()
    {
        Debug.Log("Base weapon: PLayer active received");
        currentStatus = status.Active;
        Init();
    }

    protected Vector3 GetParentVelocity()
    {
        return parentRigidBody.velocity;
    }

    public abstract void Fire();
    public abstract void StartFiring();
    public abstract void StopFiring();

    public Sprite GetUIImage()
    {
        return uiImage;
    }

}
