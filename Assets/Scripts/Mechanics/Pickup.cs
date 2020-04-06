using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class Pickup : MonoBehaviour
{
    [SerializeField] ParticleSystem pickupFxPrefab;
    [SerializeField] ParticleSystem constantFX;
    ParticleSystem pickupFx;
    AudioSource audioSource;
    [SerializeField] AudioClip pickupSound;

    [SerializeField] public enum PTypes
    {
        Armour
    }

    [SerializeField] public PTypes PType;
    [SerializeField] private float RespawnDelay;
    [SerializeField] AudioClip RespawnSound;
    [SerializeField] ParticleSystem RespawnFXPrefab;
    [SerializeField] private GameObject graphic;
    ParticleSystem RespawnFX;

    public bool available = true;

    private void Start() 
    {
        pickupFx = Instantiate(pickupFxPrefab, transform.position + (Vector3.up * 2), Quaternion.identity, transform);
        RespawnFX = Instantiate(RespawnFXPrefab, transform.position, Quaternion.LookRotation(transform.up, Vector3.up), transform);
        audioSource = GetComponent<AudioSource>();    
    }

    public void PickItUp()
    {
        if(available)
        {
            available = false;
            audioSource.PlayOneShot(pickupSound);
            pickupFx.Play();
            constantFX.Stop();
            graphic.GetComponent<MeshRenderer>().enabled = false;
            graphic.GetComponent<Collider>().enabled = false;
            graphic.transform.position = transform.position + Vector3.up * 250;
            StartCoroutine(WaitForRespawn());
        }
    }

    IEnumerator WaitForRespawn()
    {
        float elapsedTime = 0;
        while(elapsedTime < RespawnDelay)
        {
            Debug.Log("Elapsed time: " + elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return StartCoroutine(FallFromSky());
    }

    IEnumerator FallFromSky()
    {
        float fallTime = 3;
        graphic.GetComponent<MeshRenderer>().enabled = true;
        graphic.GetComponent<Collider>().enabled = true;
        float elapsedTime = 0;
        Vector3 startingPos = graphic.transform.position;
        bool playerLandingSound = false;
        while (elapsedTime < fallTime)
        {
            if(elapsedTime >= 2 && !playerLandingSound)
            {
                audioSource.PlayOneShot(RespawnSound);
                playerLandingSound = true;
            }
            graphic.transform.position = Vector3.Lerp(startingPos, transform.position, (elapsedTime / fallTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        graphic.transform.position = transform.position;
        RespawnFX.Play();
        constantFX.Play();
        available = true;
    }
}
