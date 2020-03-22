using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Explode : MonoBehaviour
{
    [Header("Physx")]
    [SerializeField][Range(0,5000)] public float force;
    [SerializeField][Range(0,20)] public float radius;
    [SerializeField][Range(0,20)] public float upwardMultiplier;
    [SerializeField] private LayerMask ignoreLayers;

    [Header("FX and sound")]
    [SerializeField]public ParticleSystem effect;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] AudioSource audioSource;

    [Header("Shrapnel")]
    [SerializeField] private List<GameObject> shrapnelItemPrefabs = new List<GameObject>();
    [SerializeField] private int shrapnelCount; 
    private List<GameObject> shrapnelItems = new List<GameObject>();

    public void DOIT()
    {
        // Sound
        if(audioSource != null & explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound, 1);
        }
        // Fx
        ParticleSystem x = Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(x.gameObject, 5f);

        // Get all affected colliders
        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, radius, ignoreLayers));
        Debug.Log("Colliders collected");
        if(shrapnelCount > 0)
        {
            Debug.Log("Getting shrapnel");
            foreach (GameObject s in shrapnelItems)
            {
                s.transform.parent = null;
                s.SetActive(true);
                colliders.Add(s.GetComponent<Collider>()); // Manually add shrapnel as they spawn inside parent collider
                Destroy(s, 10f);
            }
            shrapnelItems.Clear();
        }
        

        // Add forces
        foreach(Collider c in colliders)
        {
            Rigidbody rb = c.transform.root.GetComponent<Rigidbody>();
            if(rb != null && c.gameObject.layer == 11) //Shrapnel layer
            {
                Debug.Log("Adding forces to shrapnel");
                rb.AddForce(new Vector3(Random.Range(-force, force), force/5, Random.Range(-force, force)), ForceMode.Force);
            }

            else if(rb != null)
            {
                Debug.Log("Adding forces to rb: " + rb.gameObject.name);
                rb.AddExplosionForce(force, transform.position, radius, upwardMultiplier);
            }
        }

        // Handle screen shake
        SendImpulse();
    }

    public void SendImpulse()
    {
        if(GetComponent<CinemachineImpulseSource>())
        {
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        }
    }


    private void SpawnShrapnel()
    {
        for (int i = 0; i < shrapnelCount; i++)
        {
            GameObject x = Instantiate(shrapnelItemPrefabs[Random.Range(0, shrapnelItemPrefabs.Count)], transform.position, Quaternion.identity, transform);
            shrapnelItems.Add(x);
            x.SetActive(false);
        }
    }

    private void Awake() 
    {
        // Pre-load shrapnel for better performance
        if(shrapnelCount > 0)
        {
            SpawnShrapnel();
        }
        if(audioSource == null & GetComponent<AudioSource>() != null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnEnable() 
    {
        // Check we have enough shrapnel. (We should do)
        if(shrapnelItems.Count < shrapnelCount)
        {
            SpawnShrapnel();
        }
    }

    private void OnDisable() 
    {
        // If disabled, remove all old shrapnel, and spawn new shrapnel ready for re-enable.
        if(shrapnelCount > 0)
        {
            foreach (GameObject s in shrapnelItems)
            {
                Destroy(s, 10f);
            }
            shrapnelItems.Clear();
            SpawnShrapnel();
        }
    }
}
