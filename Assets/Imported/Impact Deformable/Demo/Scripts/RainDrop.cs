using UnityEngine;
using System.Collections;

// Rain drop for metal rain demoscene
public class RainDrop : MonoBehaviour 
{
    AudioSource audioSource;

	void Start () 
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, 10);
	}

    // Play impact sound on collision
    public void OnCollisionEnter(Collision col)
    {
        if (audioSource.isPlaying)
            return;

        audioSource.volume = col.relativeVelocity.magnitude * 0.04f;
        audioSource.pitch = Random.Range(0.2f, 1.2f);
        audioSource.Play();
    }
}
