using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class BaseWeapon : MonoBehaviour
{

    PlayerEventManager playerEventManager;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] public Transform projectileSpawn;
    [Tooltip ("Bullets spawned per second")]
    [SerializeField] protected float fireRate;
    [SerializeField] Sprite uiImage;
    [SerializeField] string projectileName;

    public status currentStatus = status.Inactive;
    protected Rigidbody parentRigidBody;
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

    private void OnEnable()
    {
        playerEventManager = transform.root.GetComponent<PlayerEventManager>();
        playerEventManager.OnPlayerStateChanged += HandlePlayerStateChange;
    }

    private void OnDisable() 
    {
        playerEventManager.OnPlayerStateChanged -= HandlePlayerStateChange;
    }

    public void Init() 
    {
        audioSource = GetComponent<AudioSource>();
        parentRigidBody = transform.root.GetComponent<Rigidbody>();
        if(fireEffectPrefab)
        {
            fireEffect = Instantiate(fireEffectPrefab, projectileSpawn.transform.position, projectileSpawn.transform.rotation, transform);
        }
    }

    public void HandlePlayerStateChange(Player.state newstate)
    {
        if(newstate == Player.state.Alive)
        {
            currentStatus = status.Active;
            Init();
        }
        else if(newstate == Player.state.Dead || newstate == Player.state.Inactive)
        {
            currentStatus = status.Inactive;
        }
    }

    public GameObject GetProjectile()
    {
        GameObject x = PoolManager.Instance.GetPooledObject(projectileName);
        x.GetComponent<BaseProjectile>().playerShooter = transform.root;
        return  x;
    }

    public void FireProjectile()
    {
        if(currentStatus == status.Active)
        {
            GameObject projectile = GetProjectile();
            projectile.SetActive(true);
            projectile.transform.position = projectileSpawn.position;
            projectile.transform.rotation = projectileSpawn.rotation;
            projectile.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed + GetParentVelocity();
        }
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
