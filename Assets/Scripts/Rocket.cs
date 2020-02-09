using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explode))]
[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour
{

    Rigidbody rb;
    Quaternion rotation;
    Explode explode;
    [SerializeField] private float flightTimeBeforeWiggle;
    [SerializeField] private bool wiggle;
    [SerializeField][Range(0, 10)] private float wiggleAmount;

    private float startTime;

    // Start is called before the first frame update
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();    
        explode = GetComponent<Explode>();
    }

    private void OnEnable() 
    {
        rb.AddForce(Vector3.up * Random.Range(3, 8), ForceMode.Impulse);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Face forward
        rotation = transform.rotation;
        rotation.SetLookRotation (rb.velocity);
        transform.rotation = rotation;

        if(wiggle && (startTime + flightTimeBeforeWiggle) < Time.time)
        {
            // Make it wiggle
            rb.AddForce(Vector3.up * Random.Range(-wiggleAmount, wiggleAmount), ForceMode.Impulse);   
            rb.AddForce(Vector3.right * Random.Range(-wiggleAmount, wiggleAmount), ForceMode.Impulse);   
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        explode.DOIT();
        gameObject.SetActive(false);
    }
}
