using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NodeCanvas.StateMachines;

public class Player : MonoBehaviour
{
    [SerializeField] ParticleSystem carExplode;
    [SerializeField] float explosionForce;
    [SerializeField] float maxHealth;
    [SerializeField] float m_currentHealth;
    [SerializeField] public float currentHealth{
        get{return m_currentHealth;}
        set{
            if(m_currentHealth == value) return;
            m_currentHealth = value;
            if(OnHealthChange != null)
            {
                OnHealthChange(m_currentHealth);
            }
        }
    }

    public delegate void OnHealthChangeDelegate(float newVal);
    public event OnHealthChangeDelegate OnHealthChange;
    

    [SerializeField] playerState state;

    enum playerState{
        Alive,
        Dead
    };


    private void Start() 
    {
        currentHealth = maxHealth;
        state=playerState.Alive;
        this.OnHealthChange += StatusHandler;

    }

    private void StatusHandler(float newHealth)
    {
        if(newHealth <= 0)
        {
            KillPlayer();
            
        }
    }

    private void KillPlayer()
    {
        GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, 1f, 2f, ForceMode.VelocityChange);
        state = playerState.Dead;
        Instantiate(carExplode, transform.position, Quaternion.identity);
        
        // Deactivate input
        if(GetComponent<FSMOwner>())
        {
            GetComponent<FSMOwner>().enabled = false;
            GetComponent<AIInput2>().enabled = false;
        }

        if(GetComponent<UserInput>())
        {
            GetComponent<UserInput>().enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {   
        // if projectile hit us
        if(other.gameObject.layer == 10)
        {
            float forceHit = other.relativeVelocity.magnitude;
            forceHit *= other.rigidbody.mass;

            Debug.Log("Player: " + gameObject.name + " hit for " + forceHit + " damage");

            if(state == playerState.Alive)
            {
                currentHealth -= forceHit;
            }
            
        }    
    }
}
