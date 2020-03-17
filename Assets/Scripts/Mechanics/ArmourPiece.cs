using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPiece : MonoBehaviour
{
    [SerializeField] public float MaxStrength;
    [SerializeField] float fallAwayForce;
    [SerializeField] ParticleSystem attachFX;
    [SerializeField] ParticleSystem trailFX;
    [SerializeField] AudioClip attachSound;

    Rigidbody rb;

    ParticleSystem trailPS;
    public float currentStrength;

    private Vector3 ogPosition;
    private Quaternion ogRotation;

    public enum State
    {
        Unequiped,
        Equipped,
        Broken
    }

    public State currentState;

    public void AlterStrength(float change)
    {
        if(currentState == State.Equipped)
        {
            currentStrength += change;
            if(currentStrength <= 0)
            {
                BreakAway();
            }
        }   
    }

    public void EnableFlyingMode()
    {
        if(trailPS == null)
        {
            trailPS =  Instantiate(trailFX, transform.position, Quaternion.identity, transform);
        }
        
        rb.AddExplosionForce(fallAwayForce*3, transform.position, 1f, 1f);
    }

    public void BreakAway()
    {
        currentState = State.Broken;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddExplosionForce(fallAwayForce, transform.position, 1f, 0.5f);
    }

    public void ReAttach()
    {
        if(trailPS != null)
        {
            Destroy(trailPS.gameObject);
            trailPS = null;
        }

        transform.localPosition = ogPosition;
        transform.localRotation = ogRotation;
        currentState = State.Equipped;
        rb.isKinematic = true;

        currentStrength = MaxStrength;

        ParticleSystem ps = Instantiate(attachFX, transform.position, Quaternion.identity, transform);
        GetComponentInParent<AudioSource>().PlayOneShot(attachSound);
        Destroy(ps.gameObject, 5f);       
    }

    public Vector3 GetOGPosition()
    {   
        return ogPosition;
    }

    public Quaternion GetOGRotation()
    {
        return ogRotation;
    }

    private void OnEnable() 
    {
        currentState = State.Equipped;
        currentStrength = MaxStrength;
        ogPosition = transform.localPosition;
        ogRotation = transform.localRotation;    
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.GetComponent<BaseProjectile>())
        {
            AlterStrength(-other.gameObject.GetComponent<BaseProjectile>().GetDamage());
        }
    }

}
