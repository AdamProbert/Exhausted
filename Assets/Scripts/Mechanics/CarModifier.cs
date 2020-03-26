using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;


public class CarModifier : MonoBehaviour
{   
    [Header("Boooost")]
    [SerializeField] private ParticleSystem boostFXPrefab;
    [SerializeField] private AudioClip boostSound;
    [SerializeField] public float maxBoost = 100;
    [SerializeField] float boostRestoreSpeed = 0.3f;
    [SerializeField] float boostUsedSpeed = 0.7f;
    public float currentBoost;
    private bool boosting;
    private ParticleSystem boostFX;


    PlayerEventManager playerEventManager;
    CarController carController;
    AudioSource audioSource;

    private void Awake() 
    {
        carController = GetComponent<CarController>();
        audioSource = GetComponent<AudioSource>();
        playerEventManager = transform.root.GetComponent<PlayerEventManager>();
        currentBoost = maxBoost;
    }
    private void Update() 
    {
        if(currentBoost < maxBoost && !boosting)
        {
            currentBoost += boostRestoreSpeed;
        }

        if(currentBoost > 0 && boosting)
        {
            currentBoost -= boostUsedSpeed;
        }

        if(currentBoost <= 1)
        {
            boosting = false;
            carController.m_Topspeed = carController.OriginalMaxSpeed;
            playerEventManager.OnPlayerBoost(false);
        }
    }

    public void Boost(bool shouldBoost)
    {
        if(shouldBoost && !boosting && currentBoost > 0)
        {
            boosting = true;
            carController.m_Topspeed = carController.OriginalMaxSpeed * 1.5f;
            playerEventManager.OnPlayerBoost(true);    
        }
        else if (!shouldBoost && boosting)
        {
            boosting = false;
            playerEventManager.OnPlayerBoost(false);
            carController.m_Topspeed = carController.OriginalMaxSpeed;
        }
    }
}
