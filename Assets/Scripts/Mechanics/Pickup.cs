using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class Pickup : MonoBehaviour
{
    [SerializeField] ParticleSystem pickupFx;
    AudioSource audioSource;
    [SerializeField] AudioClip pickupSound;

    [SerializeField] public enum PTypes
    {
        Armour
    }

    [SerializeField] public PTypes PType;



    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public void PickItUp()
    {
        audioSource.PlayOneShot(pickupSound);
        ParticleSystem fx = Instantiate(pickupFx, transform.position + (Vector3.up * 2), Quaternion.identity, transform);

        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<Collider>().enabled = false;
        Destroy(GetComponentInChildren<ParticleSystem>().gameObject);

        Destroy(this.gameObject, 5f);
    }
}
