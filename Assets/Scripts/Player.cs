using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NodeCanvas.StateMachines;

[RequireComponent(typeof(WeaponController))]
public class Player : MonoBehaviour
{
    private WeaponController weaponController;
    private AIInput2 aIInput;
    private bool isAI;
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
    

    [SerializeField] public playerState state;

    public enum playerState{
        Alive,
        Dead
    };


    private void Awake()
    {
        currentHealth = maxHealth;
        state=playerState.Alive;
        this.OnHealthChange += StatusHandler;
        weaponController = GetComponent<WeaponController>();

        // Is us AI?
        if(GetComponent<AIInput2>() && !GetComponent<UserInput>())
        {
            isAI = true;
            aIInput = GetComponent<AIInput2>();
            aIInput.OnTargetChange += OnNavTargetChange;
            weaponController.RegisterAsAI();
            // aIInput = GetComponent<AIInput2>();
            // weaponController.RegisterAsAI();
        }
        else if(!GetComponent<AIInput2>() && GetComponent<UserInput>())
        {
            isAI = false;
            // weaponController.SetAutoFind(false, null);
        }
        else
        {
            Debug.LogError("!Player must have either AIInput script or UserInput!");
        }

    }

    private void OnNavTargetChange(Player newTarget)
    {
        // Broadcast message to all components on this game object
        BroadcastMessage("NewTargetAcquired", newTarget);
        Debug.Log("Player: On nav target change received");
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
        if(isAI)
        {
            GetComponent<FSMOwner>().enabled = false;
            GetComponent<AIInput2>().enabled = false;
        }
        else
        {
            GetComponent<UserInput>().enabled = false;
        }

        BroadcastMessage("PlayerDied");

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
